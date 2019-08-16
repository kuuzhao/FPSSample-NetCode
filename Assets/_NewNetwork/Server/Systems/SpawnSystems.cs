using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace FpsSample.Server
{
    // TODO: LZ:
    //      move it into a proper file
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FPSSampleGhostSendSystem))]
    [AlwaysUpdateSystem]
    public class ClientConnetionSystem : ComponentSystem
    {
        private EntityQuery m_NetworkConnection;

        protected override void OnCreateManager()
        {
            m_NetworkConnection = GetEntityQuery(ComponentType.ReadWrite<NetworkIdComponent>());
        }

        protected override void OnUpdate()
        {
            var entities = m_NetworkConnection.GetEntityArraySt();

            for (int i = 0; i < entities.Length; ++i)
            {
                var ent = entities[i];

                if (!EntityManager.HasComponent<NetworkStreamInGame>(ent))
                {
                    EntityManager.AddComponentData(ent, new NetworkStreamInGame());
                    NetCodeIntegration.PlayerManager.CreatePlayer(ent);
                }
            }
        }
    }

    // TODO: LZ:
    //      move it into a proper file
    [DisableAutoCreation]
    public class PlayerCommandReceiveSystem : CommandReceiveSystem<PlayerCommandData>
    {
    }

}