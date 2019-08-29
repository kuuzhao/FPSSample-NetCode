using Unity.Entities;
using Unity.Mathematics;

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
