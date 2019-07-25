using Unity.Entities;
using Unity.Networking.Transport;

public struct PlayerCommandData : ICommandData<PlayerCommandData>
{
    public uint Tick => tick;
    public uint tick;
    public byte left;
    public byte right;
    public byte forward;
    public byte grenade;

    public void Serialize(DataStreamWriter writer)
    {
        writer.Write(left);
        writer.Write(right);
        writer.Write(forward);
        writer.Write(grenade);
    }

    public void Deserialize(uint inputTick, DataStreamReader reader, ref DataStreamReader.Context ctx)
    {
        tick = inputTick;
        left = reader.ReadByte(ref ctx);
        right = reader.ReadByte(ref ctx);
        forward = reader.ReadByte(ref ctx);
        grenade = reader.ReadByte(ref ctx);
    }
}
