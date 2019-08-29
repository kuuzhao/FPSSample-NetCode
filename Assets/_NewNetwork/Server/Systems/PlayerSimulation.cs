using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace NetCodeIntegration
{
    public class PlayerManager
    {
        static int playerCount = 0;

        public static Entity CreatePlayer(PlayerStateCompData pscd, Vector3 position, Quaternion rotation)
        {
            var networkConnectionEnt = pscd.networkConnectionEnt;
            var networkId = pscd.playerId;

            var world = ClientServerSystemManager.serverWorld;
            var em = world.EntityManager;

            // TODO: LZ:
            //      use server-specific prefab
            Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_robot_a_client", world, "lzPlayer");

            em.AddComponent(e, typeof(RepPlayerTagComponentData));
            em.AddComponent(e, typeof(RepPlayerComponentData));
            em.AddBuffer<PlayerCommandData>(e);
            em.AddComponent(e, typeof(GhostComponent));

            // set CommandTargetComponent to this new player entity
            var ctc = em.GetComponentData<CommandTargetComponent>(networkConnectionEnt);
            ctc.targetEntity = e;
            em.SetComponentData(networkConnectionEnt, ctc);

            RepPlayerComponentData playerCompData = default(RepPlayerComponentData);
            playerCompData.networkId = networkId;
            playerCompData.position = position;
            // TODO: LZ:
            //      we'll update the lookYaw according to the rotation here.
            // playerCompData.rotation = rotation;
            em.SetComponentData(e, playerCompData);

            var cps = em.GetComponentObject<CharacterPresentationSetup>(e);
            cps.character = e;
            cps.workaroundEntityManager = em;

            var asc = em.GetComponentObject<AnimStateController>(e);
            asc.Initialize(em, e, cps.character);

            // TODO: LZ:
            //      hard code its settings to be robot here right now
            var cmq = em.GetComponentObject<CharacterMoveQuery>(e);
            var cmqSettings = new CharacterMoveQuery.Settings
            {
                slopeLimit = 50,
                stepOffset = 0.5f,
                skinWidth = 0.08f,
                minMoveDistance = 0.001f,
                center = new float3(0, 1.18f, 0),
                radius = 0.5f,
                height = 2.2f,
            };
            cmq.Initialize(cmqSettings, e);

            ++playerCount;

            // RpcUpdatePlayerState
            // Load level RPC
            var rpcUpdatePlayerStateQueue = world.GetOrCreateSystem<FPSSampleRpcSystem>().GetRpcQueue<RpcUpdatePlayerState>();
            var rpcBuf = em.GetBuffer<OutgoingRpcDataStreamBufferComponent>(networkConnectionEnt);
            rpcUpdatePlayerStateQueue.Schedule(rpcBuf, new RpcUpdatePlayerState { cd = pscd });

            return e;
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(ProcessPlayerMovementQuery))]
    public class StartPlayerMovement : ComponentSystem
    {
        private ServerSimulationSystemGroup m_ServerSimulationSystemGroup;
        EntityQuery playerQuery;

        int m_platformLayer;
        int m_charCollisionALayer;
        int m_charCollisionBLayer;

        protected override void OnCreateManager()
        {
            m_ServerSimulationSystemGroup = World.GetOrCreateSystem<ServerSimulationSystemGroup>();

            playerQuery = GetEntityQuery(
                typeof(RepPlayerTagComponentData),
                typeof(RepPlayerComponentData),
                typeof(PlayerCommandData),
                typeof(Character),
                typeof(CharacterMoveQuery));

            m_platformLayer = LayerMask.NameToLayer("Platform");
            m_charCollisionALayer = LayerMask.NameToLayer("CharCollisionA");
            m_charCollisionBLayer = LayerMask.NameToLayer("CharCollisionB");
        }

        protected override void OnUpdate()
        {
            var playerEntities = playerQuery.GetEntityArraySt();
            var playerCompDatas = playerQuery.GetComponentDataArraySt<RepPlayerComponentData>();
            var characters = playerQuery.ToComponentArray<Character>();
            var moveQuerys = playerQuery.ToComponentArray<CharacterMoveQuery>();
            for (int i = 0; i < playerEntities.Length; ++i)
            {
                var ent = playerEntities[i];
                var playerCompData = playerCompDatas[i];
                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData cmd;
                cmdBuf.GetDataAtTick(m_ServerSimulationSystemGroup.ServerTick, out cmd);
                var character = characters[i];
                var moveQuery = moveQuerys[i];

                // TODO: LZ:
                //      we need a more decent & accurate frame duration utility
                float tickDuration = playerCompData.tick != 0 ? (m_ServerSimulationSystemGroup.ServerTick - playerCompData.tick) * 0.0166667f : 0.0166667f;

                var newPhase = CharacterPredictedData.LocoState.MaxValue;
                bool isOnGround = playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.Stand || playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.GroundMove;
                bool isMoveWanted = cmd.moveMagnitude != 0.0f;
                if (isOnGround)
                {
                    newPhase = isMoveWanted ? CharacterPredictedData.LocoState.GroundMove : CharacterPredictedData.LocoState.Stand;
                }

                if (cmd.buttons.IsSet(PlayerCommandData.Button.Jump))
                {
                    if (isOnGround)
                        newPhase = CharacterPredictedData.LocoState.Jump;
                    else if (playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.InAir)
                        newPhase = CharacterPredictedData.LocoState.DoubleJump;
                }

                if (playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.Jump || playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.DoubleJump)
                {
                    // TODO: LZ:
                    //      Revise the logic here, we need to make the following clear:
                    //          - simulation tick rate
                    //          - snapshot tick rate
                    //          - user command tick rate
                    //          - in case the server is very slow and can't meet the tick rates
                    //      Game.config.jumpAscentDuration = 0.2f;
                    if ((m_ServerSimulationSystemGroup.ServerTick - playerCompData.charLocoTick) * 0.0166667f > 0.2f)
                        newPhase = CharacterPredictedData.LocoState.InAir;
                }

                if (newPhase != CharacterPredictedData.LocoState.MaxValue && (int)newPhase != playerCompData.charLocoState)
                {
                    playerCompData.charLocoState = (int)newPhase;
                    playerCompData.charLocoTick = (int)m_ServerSimulationSystemGroup.ServerTick;
                }

                // Simple adjust of height while on platform
                if (playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.Stand && character.groundCollider != null && character.groundCollider.gameObject.layer == m_platformLayer)
                {
                    if (character.altitude < moveQuery.settings.skinWidth - 0.01f)
                    {
                        var platform = character.groundCollider;
                        var posY = platform.transform.position.y + moveQuery.settings.skinWidth;
                        playerCompData.position.y = posY;
                    }
                }

                // Calculate movement and move character
                var deltaPos = Vector3.zero;
                var velocity = playerCompData.velocity;
                switch (playerCompData.charLocoState)
                {
                    case (int)CharacterPredictedData.LocoState.Jump:
                    case (int)CharacterPredictedData.LocoState.DoubleJump:
                        velocity = CalculateGroundVelocity(velocity, ref cmd, Game.config.playerSpeed, tickDuration);
                        velocity.y = Game.config.jumpAscentHeight / Game.config.jumpAscentDuration;
                        deltaPos += (Vector3)velocity * tickDuration;

                        break;
                    case (int)CharacterPredictedData.LocoState.InAir:
                        var gravity = Game.config.playerGravity;
                        velocity += (float3)Vector3.down * gravity * tickDuration;
                        velocity = CalculateGroundVelocity(velocity, ref cmd, Game.config.playerSpeed, tickDuration);

                        if (velocity.y < -Game.config.maxFallVelocity)
                            velocity.y = -Game.config.maxFallVelocity;

                        // Cheat movement
                        if (cmd.buttons.IsSet(PlayerCommandData.Button.Boost) && (Game.GetGameLoop<PreviewGameLoop>() != null))
                        {
                            velocity.y += 25.0f * tickDuration;
                            velocity.y = Mathf.Clamp(velocity.y, -2.0f, 10.0f);
                        }

                        deltaPos = velocity * tickDuration;

                        break;
                    default:
                        var playerSpeed = playerCompData.sprinting == 1 ? Game.config.playerSprintSpeed : Game.config.playerSpeed;
                        velocity = CalculateGroundVelocity(velocity, ref cmd, playerSpeed, tickDuration);

                        // Simple follow ground code so character sticks to ground when running down hill
                        velocity.y = -400.0f * tickDuration;
                        deltaPos = velocity * tickDuration;
                        break;
                }

                // Setup movement query
                moveQuery.collisionLayer = character.teamId == 0 ? m_charCollisionALayer : m_charCollisionBLayer;
                moveQuery.moveQueryStart = playerCompData.position;
                moveQuery.moveQueryEnd = moveQuery.moveQueryStart + (float3)deltaPos;

                // Debug.Log(string.Format("LZ: deltaPos #1({0})", deltaPos.ToString()));

                playerCompData.aimYaw = cmd.lookYaw;
                playerCompData.aimPitch = cmd.lookPitch;

                EntityManager.SetComponentData(ent, playerCompData);

                // Debug.Log(cmd.ToString());
            }
        }

        Vector3 CalculateGroundVelocity(Vector3 velocity, ref PlayerCommandData command, float playerSpeed, float deltaTime)
        {
            var friction = Game.config.playerFriction;
            var acceleration = Game.config.playerAcceleration;

            var moveYawRotation = Quaternion.Euler(0, command.lookYaw + command.moveYaw, 0);
            var moveVec = moveYawRotation * Vector3.forward * command.moveMagnitude;

            if (moveVec.magnitude > 0.01f)
            {
                // Debug.Log(string.Format("LZ: moveVec({0})", moveVec.ToString()));
            }

            // Applying friction
            var groundVelocity = new Vector3(velocity.x, 0, velocity.z);
            var groundSpeed = groundVelocity.magnitude;
            var frictionSpeed = Mathf.Max(groundSpeed, 1.0f) * deltaTime * friction;
            var newGroundSpeed = groundSpeed - frictionSpeed;
            if (newGroundSpeed < 0)
                newGroundSpeed = 0;
            if (groundSpeed > 0)
                groundVelocity *= (newGroundSpeed / groundSpeed);

            // Doing actual movement (q2 style)
            var wantedGroundVelocity = moveVec * playerSpeed;
            var wantedGroundDir = wantedGroundVelocity.normalized;
            var currentSpeed = Vector3.Dot(wantedGroundDir, groundVelocity);
            var wantedSpeed = playerSpeed * command.moveMagnitude;
            var deltaSpeed = wantedSpeed - currentSpeed;
            if (deltaSpeed > 0.0f)
            {
                var accel = deltaTime * acceleration * playerSpeed;
                var speed_adjustment = Mathf.Clamp(accel, 0.0f, deltaSpeed) * wantedGroundDir;
                groundVelocity += speed_adjustment;
            }

            if (!Game.config.easterBunny)
            {
                newGroundSpeed = groundVelocity.magnitude;
                if (newGroundSpeed > playerSpeed)
                    groundVelocity *= playerSpeed / newGroundSpeed;
            }

            velocity.x = groundVelocity.x;
            velocity.z = groundVelocity.z;

            return velocity;
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FinalizePlayerMovement))]
    public class ProcessPlayerMovementQuery : ComponentSystem
    {
        EntityQuery playerQuery;

        protected override void OnCreateManager()
        {
            playerQuery = GetEntityQuery(
                typeof(RepPlayerTagComponentData),
                typeof(RepPlayerComponentData),
                typeof(PlayerCommandData),
                typeof(Character),
                typeof(CharacterMoveQuery));
        }

        protected override void OnUpdate()
        {
            var cmqs = playerQuery.ToComponentArray<CharacterMoveQuery>();
            for (int i = 0; i < cmqs.Length; ++i)
            {
                var cmq = cmqs[i];

                var charController = cmq.charController;

                if (charController.gameObject.layer != cmq.collisionLayer)
                    charController.gameObject.layer = cmq.collisionLayer;

                float3 currentControllerPos = charController.transform.position;
                if (math.distance(currentControllerPos, cmq.moveQueryStart) > 0.01f)
                {
                    currentControllerPos = cmq.moveQueryStart;
                    charController.transform.position = currentControllerPos;
                }

                var deltaPos = cmq.moveQueryEnd - currentControllerPos;
                // Debug.Log(string.Format("LZ: deltaPos #2({0})", deltaPos.ToString()));
                charController.Move(deltaPos);
                cmq.moveQueryResult = charController.transform.position;
                cmq.isGrounded = charController.isGrounded;
            }
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FPSSampleGhostSendSystem))]
    public class FinalizePlayerMovement : ComponentSystem
    {
        private ServerSimulationSystemGroup m_ServerSimulationSystemGroup;

        EntityQuery playerQuery;

        protected override void OnCreateManager()
        {
            m_ServerSimulationSystemGroup = World.GetOrCreateSystem<ServerSimulationSystemGroup>();

            playerQuery = GetEntityQuery(
                typeof(RepPlayerTagComponentData),
                typeof(RepPlayerComponentData),
                typeof(PlayerCommandData),
                typeof(CharacterMoveQuery),
                typeof(Transform));
        }

        protected override void OnUpdate()
        {
            var playerEntities = playerQuery.GetEntityArraySt();
            var playerCompDatas = playerQuery.GetComponentDataArraySt<RepPlayerComponentData>();
            var cmqs = playerQuery.ToComponentArray<CharacterMoveQuery>();
            var trs = playerQuery.ToComponentArray<Transform>();

            for (int i = 0; i < playerEntities.Length; ++i)
            {
                var ent = playerEntities[i];
                var playerCompData = playerCompDatas[i];
                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData cmd;
                cmdBuf.GetDataAtTick(m_ServerSimulationSystemGroup.ServerTick, out cmd);
                var cmq = cmqs[i];

                float tickDuration = playerCompData.tick != 0 ? (m_ServerSimulationSystemGroup.ServerTick - playerCompData.tick) * 0.0166667f : 0.0166667f;

                var isOnGround = playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.Stand || playerCompData.charLocoState == (int)CharacterPredictedData.LocoState.GroundMove;
                if (isOnGround != cmq.isGrounded)
                {
                    if (cmq.isGrounded)
                    {
                        playerCompData.charLocoState = (int)(cmd.moveMagnitude != 0.0f ? CharacterPredictedData.LocoState.GroundMove : CharacterPredictedData.LocoState.Stand);
                    }
                    else
                    {
                        playerCompData.charLocoState = (int)CharacterPredictedData.LocoState.InAir;
                    }

                    playerCompData.charLocoTick = (int)m_ServerSimulationSystemGroup.ServerTick;
                }

                var newPos = cmq.moveQueryResult;
                var oldPos = cmq.moveQueryStart;
                var velocity = (newPos - oldPos) / tickDuration;

                playerCompData.velocity = velocity;
                playerCompData.position = cmq.moveQueryResult;

                var groundMoveVec = Vector3.ProjectOnPlane(playerCompData.velocity, Vector3.up);
                playerCompData.moveYaw = Vector3.Angle(Vector3.forward, groundMoveVec);
                var cross = Vector3.Cross(Vector3.forward, groundMoveVec);
                if (cross.y < 0)
                    playerCompData.moveYaw = 360 - playerCompData.moveYaw;

                playerCompData.tick = m_ServerSimulationSystemGroup.ServerTick;
                EntityManager.SetComponentData(ent, playerCompData);

                // Debug.Log(string.Format("LZ: playerCompData.position({0})", playerCompData.position.ToString()));

                trs[i].position = playerCompData.position;
                trs[i].rotation = Quaternion.Euler(0f, playerCompData.rotation, 0f);
            }
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FPSSampleGhostSendSystem))]
    public class AnimStatControllerServer : ComponentSystem
    {
        EntityQuery playerQuery;

        protected override void OnCreateManager()
        {
            playerQuery = GetEntityQuery(
                typeof(RepPlayerTagComponentData),
                typeof(AnimStateController));
        }

        protected override void OnUpdate()
        {
            var animStatControllerArray = playerQuery.ToComponentArray<AnimStateController>();

            for (int i = 0; i < animStatControllerArray.Length; ++i)
            {
                var animStatController = animStatControllerArray[i];
                // TODO: LZ:
                //      don't use ServerGameLoop.Instance.GameWorld
                animStatController.UpdatePresentationState(ServerGameLoop.Instance.GameWorld.worldTime,
                    ServerGameLoop.Instance.GameWorld.frameDuration);
                animStatController.ApplyPresentationState(ServerGameLoop.Instance.GameWorld.worldTime,
                    ServerGameLoop.Instance.GameWorld.frameDuration);
            }
        }
    }
}