using Unity.Entities;
using Unity.Networking.Transport;

public struct PlayerCommandData : ICommandData<PlayerCommandData>
{
    public enum Button : uint
    {
        None = 0,
        Jump = 1 << 0,
        Boost = 1 << 1,
        PrimaryFire = 1 << 2,
        SecondaryFire = 1 << 3,
        Reload = 1 << 4,
        Melee = 1 << 5,
        Use = 1 << 6,
        Ability1 = 1 << 7,
        Ability2 = 1 << 8,
        Ability3 = 1 << 9,
    }

    public struct ButtonBitField
    {
        public uint flags;

        public bool IsSet(Button button)
        {
            return (flags & (uint)button) > 0;
        }

        public void Or(Button button, bool val)
        {
            if (val)
                flags = flags | (uint)button;
        }


        public void Set(Button button, bool val)
        {
            if (val)
                flags = flags | (uint)button;
            else
            {
                flags = flags & ~(uint)button;
            }
        }
    }

    public uint Tick => tick;
    public uint tick;

    public float moveYaw;
    public float moveMagnitude;
    public float lookYaw;
    public float lookPitch;
    public ButtonBitField buttons;

    // TODO: LZ:
    //      for debug only, to be removed, use buttons.IsSet(SecondaryFire) instead
    public byte grenade;

    public void Serialize(DataStreamWriter writer)
    {
        writer.Write((int)(moveYaw * 100));
        writer.Write((int)(moveMagnitude * 100));
        writer.Write((int)(lookYaw * 100));
        writer.Write((int)(lookPitch * 100));
        writer.Write(buttons.flags);

        writer.Write(grenade);
    }

    public void Deserialize(uint inputTick, DataStreamReader reader, ref DataStreamReader.Context ctx)
    {
        tick = inputTick;
        moveYaw = reader.ReadInt(ref ctx) * 0.01f;
        moveMagnitude = reader.ReadInt(ref ctx) * 0.01f;
        lookYaw = reader.ReadInt(ref ctx) * 0.01f;
        lookPitch = reader.ReadInt(ref ctx) * 0.01f;
        buttons.flags = reader.ReadUInt(ref ctx);

        grenade = reader.ReadByte(ref ctx);
    }

    public override string ToString()
    {
        return string.Format("moveYaw({0}), moveMagnitude({1}), lookYaw({2}), lookPitch({3}), grenade({4})",
            moveYaw, moveMagnitude, lookYaw, lookPitch, grenade);
    }
}
