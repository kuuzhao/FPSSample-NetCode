using Unity.Entities;
using Unity.Mathematics;
using Unity.Networking.Transport;

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
