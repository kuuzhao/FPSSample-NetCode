using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;


[DisableAutoCreation]
public class PlayerCommandSendSystem : CommandSendSystem<PlayerCommandData>
{
}

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[UpdateAfter(typeof(GhostReceiveSystemGroup))]
[UpdateBefore(typeof(PlayerCommandSendSystem))]
public class NetCodeInputSystem : ComponentSystem
{
    EntityQuery cmdTargetGroup;

    byte grenadeDebugNo = 1;

    protected override void OnCreateManager()
    {
        cmdTargetGroup = GetEntityQuery(ComponentType.ReadWrite<CommandTargetComponent>());
    }

    protected override void OnUpdate()
    {
        if (cmdTargetGroup.IsEmptyIgnoreFilter)
            return;

        var entityArray = cmdTargetGroup.GetEntityArraySt();

        if (entityArray.Length == 1)
        {
            Entity ent = entityArray[0];
            if (!EntityManager.HasComponent<PlayerCommandData>(ent))
            {
                EntityManager.AddBuffer<PlayerCommandData>(ent);
                var ctc = EntityManager.GetComponentData<CommandTargetComponent>(ent);
                ctc.targetEntity = ent;
                EntityManager.SetComponentData(ent, ctc);
            }

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
                if (Input.GetMouseButtonUp(1))
                {
                    cmdData.grenade = grenadeDebugNo;
                    Debug.Log("LZ: Send Grenade #" + cmdData.grenade + " at frame #" + Time.frameCount);
                    grenadeDebugNo++;
                }
                cmdData.tick = NetworkTimeSystem.predictTargetTick;

                cmdBuf.AddCommandData(cmdData);
            }
        }
    }
}
