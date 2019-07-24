using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public struct PlayerCommandData : ICommandData<PlayerCommandData>
{
    public uint Tick => tick;
    public uint tick;
    public byte left;
    public byte right;
    public byte forward;
    public byte shoot;

    public void Serialize(DataStreamWriter writer)
    {
        writer.Write(left);
        writer.Write(right);
        writer.Write(forward);
        writer.Write(shoot);
    }

    public void Deserialize(uint inputTick, DataStreamReader reader, ref DataStreamReader.Context ctx)
    {
        tick = inputTick;
        left = reader.ReadByte(ref ctx);
        right = reader.ReadByte(ref ctx);
        forward = reader.ReadByte(ref ctx);
        shoot = reader.ReadByte(ref ctx);
    }
}

[DisableAutoCreation]
public class PlayerCommandSendSystem : CommandSendSystem<PlayerCommandData>
{
}

#if false

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[UpdateAfter(typeof(GhostReceiveSystemGroup))]
[UpdateBefore(typeof(PlayerCommandSendSystem))]
public class NetCodeInputSystem : ComponentSystem
{
    EntityQuery cmdTargetGroup;

    protected override void OnCreateManager()
    {
        cmdTargetGroup = GetEntityQuery(ComponentType.ReadWrite<CommandTargetComponent>());
    }

    protected override void OnUpdate()
    {
        var entityArray = cmdTargetGroup.ToEntityArray(Unity.Collections.Allocator.TempJob);

        if (entityArray.Length == 1)
        {
            Entity ent = entityArray[0];
            if (!EntityManager.HasComponent<NetworkStreamDisconnected>(ent))
            {
                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData cmdData = default(PlayerCommandData);

                if (Input.GetKey("left"))
                    cmdData.left = 1;
                if (Input.GetKey("right"))
                    cmdData.right = 1;
                if (Input.GetKey("up"))
                    cmdData.forward = 1;
                if (Input.GetKey("space"))
                    cmdData.shoot = 1;
                cmdData.tick = NetworkTimeSystem.predictTargetTick;

                cmdBuf.AddCommandData(cmdData);
            }
        }

        entityArray.Dispose();
    }
}

#endif