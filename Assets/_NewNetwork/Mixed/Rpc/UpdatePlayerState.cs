using Unity.Entities;
using Unity.Mathematics;
using Unity.Networking.Transport;

// TODO: LZ:
//      Use NativeString64 instead of the hard-coded fixed-length string
public unsafe struct PlayerStateCompData : IComponentData
{
    public string PlayerName
    {
        set
        {
            byte[] strBytes = System.Text.Encoding.Unicode.GetBytes(value);
            if (strBytes.Length <= MAX_STR_LENGTH)
            {
                playerNameLen = strBytes.Length;
                for (int i = 0; i < playerNameLen; ++i)
                    playerName[i] = strBytes[i];
            }
            else
                playerNameLen = 0;
        }
        get
        {
            if (playerNameLen > 0)
            {
                fixed (byte* pPlayerName = playerName)
                {
                    return System.Text.Encoding.Unicode.GetString(pPlayerName, playerNameLen);
                }
            }
            else
                return "";
        }
    }

    public string GameResult
    {
        set
        {
            byte[] strBytes = System.Text.Encoding.Unicode.GetBytes(value);
            if (strBytes.Length <= MAX_STR_LENGTH)
            {
                gameResultLen = strBytes.Length;
                for (int i = 0; i < gameResultLen; ++i)
                    gameResult[i] = strBytes[i];
            }
            else
                gameResultLen = 0;
        }
        get
        {
            if (gameResultLen > 0)
            {
                fixed (byte* pGameResult = gameResult)
                {
                    return System.Text.Encoding.Unicode.GetString(pGameResult, gameResultLen);
                }
            }
            else
                return "";
        }
    }

    public string GoalString
    {
        set
        {
            byte[] strBytes = System.Text.Encoding.Unicode.GetBytes(value);
            if (strBytes.Length <= MAX_STR_LENGTH)
            {
                goalStringLen = strBytes.Length;
                for (int i = 0; i < goalStringLen; ++i)
                    goalString[i] = strBytes[i];
            }
            else
                goalStringLen = 0;
        }
        get
        {
            if (goalStringLen > 0)
            {
                fixed (byte* pGoalString = goalString)
                {
                    return System.Text.Encoding.Unicode.GetString(pGoalString, goalStringLen);
                }
            }
            else
                return "";
        }
    }

    public string ActionString
    {
        set
        {
            byte[] strBytes = System.Text.Encoding.Unicode.GetBytes(value);
            if (strBytes.Length <= MAX_STR_LENGTH)
            {
                actionStringLen = strBytes.Length;
                for (int i = 0; i < actionStringLen; ++i)
                    actionString[i] = strBytes[i];
            }
            else
                actionStringLen = 0;
        }
        get
        {
            if (actionStringLen > 0)
            {
                fixed (byte* pActionString = actionString)
                {
                    return System.Text.Encoding.Unicode.GetString(pActionString, actionStringLen);
                }
            }
            else
                return "";
        }
    }

    private const int MAX_STR_LENGTH = 128;

    public int playerId;

    public fixed byte playerName[MAX_STR_LENGTH];
    public int playerNameLen;

    public int teamIndex;
    public int score;
    public bool displayScoreBoard;
    public bool displayGameScore;
    public bool displayGameResult;

    public fixed byte gameResult[MAX_STR_LENGTH];
    public int gameResultLen;

    public bool displayGoal;
    public float3 goalPosition;
    public uint goalDefendersColor;
    public uint goalAttackersColor;
    public uint goalAttackers;
    public uint goalDefenders;

    public fixed byte goalString[MAX_STR_LENGTH];
    public int goalStringLen;

    public fixed byte actionString[MAX_STR_LENGTH];
    public int actionStringLen;

    public float goalCompletion;

    // TODO: LZ:
    //      to be removed
    public Entity controlledEntity;

    // TODO: LZ:
    //      figure out 
    public int characterType;
    public int requestedCharacterType;
    public bool enableCharacterSwitch;
    public bool gameModeSystemInitialized;
    public Entity networkConnectionEnt;
}

// TODO: LZ:
//      auto-generate RPC codes.
public struct RpcUpdatePlayerState : IRpcCommand
{
    public PlayerStateCompData cd;

    public void Execute(Entity connection, EntityCommandBuffer.Concurrent commandBuffer, int jobIndex)
    {
        throw new System.NotImplementedException();
    }

    public void Execute(Entity connection, EntityCommandBuffer commandBuffer)
    {
        UnityEngine.Debug.Log(string.Format("LZ: UpdatePlayerState ({0})", cd.playerId));

        ClientGameLoop.Instance.ClientPlayerStateMgr.UpdatePlayerState(cd, commandBuffer);
    }

    public void Serialize(DataStreamWriter writer)
    {
        writer.Write(cd.playerId);
        writer.WriteUnicodeString(cd.PlayerName);
        writer.Write(cd.teamIndex);
        writer.Write(cd.score);
        writer.Write(System.Convert.ToByte(cd.displayScoreBoard));
        writer.Write(System.Convert.ToByte(cd.displayGameScore));
        writer.Write(System.Convert.ToByte(cd.displayGameResult));
        writer.WriteUnicodeString(cd.GameResult);
        writer.Write(System.Convert.ToByte(cd.displayGoal));
        writer.Write(cd.goalPosition.x);   // TODO: LZ: add support for float3 to DataStreamWriter
        writer.Write(cd.goalPosition.y);
        writer.Write(cd.goalPosition.z);
        writer.Write(cd.goalDefendersColor);
        writer.Write(cd.goalAttackersColor);
        writer.Write(cd.goalAttackers);
        writer.Write(cd.goalDefenders);
        writer.WriteUnicodeString(cd.GoalString);
        writer.WriteUnicodeString(cd.ActionString);
        writer.Write(cd.goalCompletion);
    }

    public void Deserialize(DataStreamReader reader, ref DataStreamReader.Context ctx)
    {
        cd.playerId = reader.ReadInt(ref ctx);
        cd.PlayerName = reader.ReadUnicodeString(ref ctx);
        cd.teamIndex = reader.ReadInt(ref ctx);
        cd.score = reader.ReadInt(ref ctx);
        cd.displayScoreBoard = System.Convert.ToBoolean(reader.ReadByte(ref ctx));
        cd.displayGameScore = System.Convert.ToBoolean(reader.ReadByte(ref ctx));
        cd.displayGameResult = System.Convert.ToBoolean(reader.ReadByte(ref ctx));
        cd.GameResult = reader.ReadUnicodeString(ref ctx);
        cd.displayGoal = System.Convert.ToBoolean(reader.ReadByte(ref ctx));
        cd.goalPosition.x = reader.ReadFloat(ref ctx);
        cd.goalPosition.y = reader.ReadFloat(ref ctx);
        cd.goalPosition.z = reader.ReadFloat(ref ctx);
        cd.goalDefendersColor = reader.ReadUInt(ref ctx);
        cd.goalAttackersColor = reader.ReadUInt(ref ctx);
        cd.goalAttackers = reader.ReadUInt(ref ctx);
        cd.goalDefenders = reader.ReadUInt(ref ctx);
        cd.GoalString = reader.ReadUnicodeString(ref ctx);
        cd.ActionString = reader.ReadUnicodeString(ref ctx);
        cd.goalCompletion = reader.ReadFloat(ref ctx);
    }
}
