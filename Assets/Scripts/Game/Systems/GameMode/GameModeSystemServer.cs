using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Profiling;

public interface IGameMode
{
    void Initialize(GameWorld world, GameModeSystemServer gameModeSystemServer);
    void Shutdown();

    void Restart();
    void Update();

    void OnPlayerJoin(PlayerStateCompData player);
    void OnPlayerRespawn(PlayerStateCompData player, ref Vector3 position, ref Quaternion rotation);
    void OnPlayerKilled(PlayerStateCompData victim, PlayerStateCompData killer);
}

public class NullGameMode : IGameMode
{
    public void Initialize(GameWorld world, GameModeSystemServer gameModeSystemServer) { }
    public void OnPlayerJoin(PlayerStateCompData teamMember) { }
    public void OnPlayerKilled(PlayerStateCompData victim, PlayerStateCompData killer) { }
    public void OnPlayerRespawn(PlayerStateCompData player, ref Vector3 position, ref Quaternion rotation) { }
    public void Restart() { }
    public void Shutdown() { }
    public void Update() { }
}

public class Team
{
    public string name;
    public int score;
}

// TODO: LZ:
//      add UpdateAfter. We want it to "Run last to allow picking up deaths etc".
[DisableAutoCreation]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateBefore(typeof(FPSSampleGhostSendSystem))]
public class GameModeSystemServer : ComponentSystem
{
    [ConfigVar(Name = "game.respawndelay", DefaultValue = "10", Description = "Time from death to respawning")]
    public static ConfigVar respawnDelay;
    [ConfigVar(Name = "game.modename", DefaultValue = "assault", Description = "Which gamemode to use")]
    public static ConfigVar modeName;

    public EntityQuery m_PlayerStateQuery;
    EntityQuery m_TeamBaseComponentGroup;
    EntityQuery m_SpawnPointComponentGroup;

    public RepGameMode repGameModeComp;
    public Entity repGameModeEnt;

    public ChatSystemServer chatSystem;
    public List<Team> teams = new List<Team>();
    public List<TeamBase> teamBases = new List<TeamBase>();

    public void Init(GameWorld world, ChatSystemServer chatSystem)
    {
        m_World = world;
        // m_ResourceSystem = null;
        this.chatSystem = chatSystem;
        m_CurrentGameModeName = "";

        // TODO (petera) Get rid of need for loading these 'settings' and the use of them below.
        // We need a way to spawn a 'naked' replicated entity, i.e. one that is not created from a prefab.
        m_Settings = Resources.Load<GameModeSystemSettings>("GameModeSystemSettings");

        repGameModeEnt = EntityManager.CreateEntity(typeof(RepGameMode), typeof(GhostComponent));
        repGameModeComp = default(RepGameMode);
        EntityManager.SetComponentData(repGameModeEnt, repGameModeComp);
    }

    public void Restart()
    {
        GameDebug.Log("Restarting gamdemode");
        var bases = m_TeamBaseComponentGroup.ToComponentArray<TeamBase>();
        teamBases.Clear();
        for (var i = 0; i < bases.Length; i++)
        {
            teamBases.Add(bases[i]);
        }

        for (int i = 0, c = teams.Count; i < c; ++i)
        {
            teams[i].score = -1;
        }

        var players = m_PlayerStateQuery.GetComponentDataArraySt<PlayerStateCompData>();
        for (int i = 0, c = players.Length; i < c; ++i)
        {
            var player = players[i];
            player.score = 0;
            player.displayGameScore = true;
            player.goalCompletion = -1.0f;
            player.ActionString = "";
        }

        m_EnableRespawning = true;

        m_GameMode.Restart();

        chatSystem.ResetChatTime();
    }


    public void Shutdown()
    {
        m_GameMode.Shutdown();

        Resources.UnloadAsset(m_Settings);
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        m_PlayerStateQuery = GetComponentGroup(typeof(PlayerStateCompData));
        m_TeamBaseComponentGroup = GetComponentGroup(typeof(TeamBase));
        m_SpawnPointComponentGroup = GetComponentGroup(typeof(SpawnPoint));
    }

    new public EntityQuery GetComponentGroup(params ComponentType[] componentTypes)
    {
        return base.GetEntityQuery(componentTypes);
    }

    float m_TimerStart;
    ConfigVar m_TimerLength;
    public void StartGameTimer(ConfigVar seconds, string message)
    {
        m_TimerStart = Time.time;
        m_TimerLength = seconds;

        // TODO: LZ:
        //
        // We already make sure that StartGameTimer can only be invoked in
        // GameModeSystemServer.OnUpdate()
        // so we don't need to query its value from the entity
        // we also don't need to set the value to the entity after we change it.
        //
        // It's not safe to leave this method as "public"
        repGameModeComp.gameTimerMessage.CopyFrom(message);
    }

    public int GetGameTimer()
    {
        return Mathf.Max(0, Mathf.FloorToInt(m_TimerStart + m_TimerLength.FloatValue - Time.time));
    }

    public void SetRespawnEnabled(bool enable)
    {
        m_EnableRespawning = enable;
    }

    char[] _msgBuf = new char[256];
    protected override void OnUpdate()
    {
        if (m_World == null) // Not initialized yet
            return;

        // NO early out in this method !!!
        // the changed to repGameModeComp must be saved at the end of this method.
        repGameModeComp = EntityManager.GetComponentData<RepGameMode>(repGameModeEnt);

        // Handle change of game mode
        if (m_CurrentGameModeName != modeName.Value)
        {
            m_CurrentGameModeName = modeName.Value;

            switch (m_CurrentGameModeName)
            {
                case "deathmatch":
                    m_GameMode = new GameModeDeathmatch();
                    break;
                case "assault":
                    m_GameMode = new GameModeAssault();
                    break;
                default:
                    m_GameMode = new NullGameMode();
                    break;
            }
            m_GameMode.Initialize(m_World, this);
            GameDebug.Log("New gamemode : '" + m_GameMode.GetType().ToString() + "'");
            Restart();
        }
        else
        {

            // Handle joining players
            var playerEntities = m_PlayerStateQuery.GetEntityArraySt();
            var playerStates = m_PlayerStateQuery.GetComponentDataArraySt<PlayerStateCompData>();
            for (int i = 0, c = playerStates.Length; i < c; ++i)
            {
                var playerEnt = playerEntities[i];
                var playerState = playerStates[i];

                if (!playerState.gameModeSystemInitialized)
                {
                    playerState.score = 0;
                    playerState.displayGameScore = true;
                    playerState.goalCompletion = -1.0f;
                    m_GameMode.OnPlayerJoin(playerState);
                    playerState.gameModeSystemInitialized = true;

                    EntityManager.SetComponentData(playerEnt, playerState);
                }
            }

            m_GameMode.Update();

            // General rules
            repGameModeComp.gameTimerSeconds = GetGameTimer();

            // TODO: LZ:
            //      turn off the logic here
            for (int i = 0, c = playerStates.Length; i < c; ++i)
            {
                var playerEnt = playerEntities[i];
                var playerState = playerStates[i];

                playerState.ActionString = playerState.enableCharacterSwitch ? "Press H to change character" : "";

                // Spawn contolled entity (character) any missing
                if (playerState.controlledEntity == Entity.Null)
                {
                    var position = new Vector3(0.0f, 0.2f, 0.0f);
                    var rotation = Quaternion.identity;
                    GetRandomSpawnTransform(playerState.teamIndex, ref position, ref rotation);

                    m_GameMode.OnPlayerRespawn(playerState, ref position, ref rotation);

                    if (playerState.characterType == -1)
                    {
                        playerState.characterType = Game.characterType.IntValue;
                        if (Game.allowCharChange.IntValue == 1)
                        {
                            playerState.characterType = playerState.teamIndex;
                        }
                    }

                    playerState.controlledEntity = NetCodeIntegration.PlayerManager.CreatePlayer(playerState, position, rotation);

                    EntityManager.SetComponentData(playerEnt, playerState);

#if false
                if (charControl.characterType == 1000)
                    SpectatorCamSpawnRequest.Create(PostUpdateCommands, position, rotation, playerEntity);
                else
                    CharacterSpawnRequest.Create(PostUpdateCommands, charControl.characterType, position, rotation, playerEntity);
#endif
                    continue;
                }

#if false
            // Has new new entity been requested
            if (playerState.requestedCharacterType != -1)
            {
                if (playerState.requestedCharacterType != playerState.characterType)
                {
                    playerState.characterType = playerState.requestedCharacterType;
                    if (playerState.controlledEntity != Entity.Null)
                    {

                        // Despawn current controlled entity. New entity will be created later
                        if (EntityManager.HasComponent<Character>(controlledEntity))
                        {
                            var predictedState = EntityManager.GetComponentData<CharacterPredictedData>(controlledEntity);
                            var rotation = predictedState.velocity.magnitude > 0.01f ? Quaternion.LookRotation(predictedState.velocity.normalized) : Quaternion.identity;

                            CharacterDespawnRequest.Create(PostUpdateCommands, controlledEntity);
                            CharacterSpawnRequest.Create(PostUpdateCommands, playerState.characterType, predictedState.position, rotation, playerEntity);
                        }
                        playerState.controlledEntity = Entity.Null;
                    }
                }
                playerState.requestedCharacterType = -1;
                continue;
            }

            if (EntityManager.HasComponent<HealthStateData>(controlledEntity))
            {
                // Is character dead ?
                var healthState = EntityManager.GetComponentData<HealthStateData>(controlledEntity);
                if (healthState.health == 0)
                {
                    // Send kill msg
                    if (healthState.deathTick == m_World.worldTime.tick)
                    {
                        var killerEntity = healthState.killedBy;
                        var killerIndex = FindPlayerControlling(playerStates, killerEntity);
                        PlayerStateCompData killerPlayer = default(PlayerStateCompData);
                        killerPlayer.playerId = -1;
                        if (killerIndex != -1)
                        {
                            killerPlayer = playerStates[killerIndex];
                            var format = s_KillMessages[Random.Range(0, s_KillMessages.Length)];
                            var l = StringFormatter.Write(ref _msgBuf, 0, format, killerPlayer.PlayerName, playerState.PlayerName, m_TeamColors[killerPlayer.teamIndex], m_TeamColors[playerState.teamIndex]);
                            chatSystem.SendChatAnnouncement(new CharBufView(_msgBuf, l));
                        }
                        else
                        {
                            var format = s_SuicideMessages[Random.Range(0, s_SuicideMessages.Length)];
                            var l = StringFormatter.Write(ref _msgBuf, 0, format, playerState.PlayerName, m_TeamColors[playerState.teamIndex]);
                            chatSystem.SendChatAnnouncement(new CharBufView(_msgBuf, l));
                        }
                        m_GameMode.OnPlayerKilled(playerState, killerPlayer);
                    }

                    // Respawn dead players except if in ended mode
                    if (m_EnableRespawning && (m_World.worldTime.tick - healthState.deathTick) *
                        m_World.worldTime.tickInterval > respawnDelay.IntValue)
                    {
                        // Despawn current controlled entity. New entity will be created later
                        if (EntityManager.HasComponent<Character>(controlledEntity))
                            CharacterDespawnRequest.Create(PostUpdateCommands, controlledEntity);
                        playerState.controlledEntity = Entity.Null;
                    }
                }
            }
#endif
            }

        }

        EntityManager.SetComponentData(repGameModeEnt, repGameModeComp);
    }

    internal void RequestNextChar(PlayerState player)
    {
        if (!player.enableCharacterSwitch)
            return;

        //var heroTypeRegistry = m_ResourceSystem.GetResourceRegistry<HeroTypeRegistry>();
        //var c = player.GetComponent<PlayerCharacterControl>();
        //c.requestedCharacterType = (c.characterType + 1) % heroTypeRegistry.entries.Count;

        //chatSystem.SendChatMessage(player.playerId, "Switched to: " + heroTypeRegistry.entries[c.requestedCharacterType].name);
    }

    public void CreateTeam(string name)
    {
        var team = new Team();
        team.name = name;
        teams.Add(team);

        // Update clients
        var idx = teams.Count - 1;
        if (idx == 0)
            repGameModeComp.teamName0.CopyFrom(name);
        if (idx == 1)
            repGameModeComp.teamName1.CopyFrom(name);
    }

    // Assign to team with fewest members
    public void AssignTeam(PlayerStateCompData player)
    {
        // Count team sizes
        var players = m_PlayerStateQuery.GetComponentDataArraySt<PlayerStateCompData>();
        int[] teamCount = new int[teams.Count];
        for (int i = 0, c = players.Length; i < c; ++i)
        {
            var idx = players[i].teamIndex;
            if (idx < teamCount.Length)
                teamCount[idx]++;
        }

        // Pick smallest
        int joinIndex = -1;
        int smallestTeamSize = 1000;
        for (int i = 0, c = teams.Count; i < c; i++)
        {
            if (teamCount[i] < smallestTeamSize)
            {
                smallestTeamSize = teamCount[i];
                joinIndex = i;
            }
        }

        // Join 
        player.teamIndex = joinIndex < 0 ? 0 : joinIndex;
        GameDebug.Log("Assigned team " + joinIndex + " to player " + player);
    }

    int FindPlayerControlling(ComponentDataArraySt<PlayerStateCompData> players, Entity entity)
    {
        if (entity == Entity.Null)
            return -1;

        for (int i = 0, c = players.Length; i < c; ++i)
        {
            var playerState = players[i];
            if (playerState.controlledEntity == entity)
                return i;
        }
        return -1;
    }

    public bool GetRandomSpawnTransform(int teamIndex, ref Vector3 pos, ref Quaternion rot)
    {
        // Make list of spawnpoints for team 
        var teamSpawns = new List<SpawnPoint>();
        var spawnPoints = m_SpawnPointComponentGroup.ToComponentArray<SpawnPoint>();
        for (var i = 0; i < spawnPoints.Length; i++)
        {
            var spawnPoint = spawnPoints[i];
            if (spawnPoint.teamIndex == teamIndex)
                teamSpawns.Add(spawnPoint);
        }

        if (teamSpawns.Count == 0)
            return false;

        var index = (m_prevTeamSpawnPointIndex[teamIndex] + 1) % teamSpawns.Count;
        m_prevTeamSpawnPointIndex[teamIndex] = index;
        pos = teamSpawns[index].transform.position;
        rot = teamSpawns[index].transform.rotation;

        GameDebug.Log("spawning at " + teamSpawns[index].name);

        return true;
    }

    static string[] s_KillMessages = new string[]
    {
        "<color={2}>{0}</color> killed <color={3}>{1}</color>",
        "<color={2}>{0}</color> terminated <color={3}>{1}</color>",
        "<color={2}>{0}</color> ended <color={3}>{1}</color>",
        "<color={2}>{0}</color> owned <color={3}>{1}</color>",
    };

    static string[] s_SuicideMessages = new string[]
    {
        "<color={1}>{0}</color> rebooted",
        "<color={1}>{0}</color> gave up",
        "<color={1}>{0}</color> slipped and accidently killed himself",
        "<color={1}>{0}</color> wanted to give the enemy team an edge",
    };

    static string[] m_TeamColors = new string[]
    {
        "#1EA00000", //"#FF19E3FF",
        "#1EA00001", //"#00FFEAFF",
    };

    GameWorld m_World;
    // readonly BundledResourceManager m_ResourceSystem;
    GameModeSystemSettings m_Settings;
    int[] m_prevTeamSpawnPointIndex = new int[2];
    IGameMode m_GameMode;
    bool m_EnableRespawning = true;
    string m_CurrentGameModeName;
}
