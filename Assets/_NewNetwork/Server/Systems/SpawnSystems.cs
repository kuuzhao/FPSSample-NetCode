using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace NetCodeIntegration
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

        RpcQueue<RpcLoadLevel> rpcLoadLevelQueue;

        protected override void OnCreateManager()
        {
            m_NetworkConnection = GetEntityQuery(
                ComponentType.ReadOnly<NetworkIdComponent>(),
                ComponentType.Exclude<NetworkStreamInGame>());

            rpcLoadLevelQueue = World.GetOrCreateSystem<FPSSampleRpcSystem>().GetRpcQueue<RpcLoadLevel>();
        }

        protected override void OnUpdate()
        {
            var entities = m_NetworkConnection.GetEntityArraySt();
            var networkIds = m_NetworkConnection.GetComponentDataArraySt<NetworkIdComponent>();

            for (int i = 0; i < entities.Length; ++i)
            {
                var ent = entities[i];
                var networkId = networkIds[i];

                EntityManager.AddComponentData(ent, new NetworkStreamInGame());

                var rpcBuf = EntityManager.GetBuffer<OutgoingRpcDataStreamBufferComponent>(ent);

                if (Game.game.levelManager.currentLevel.name != null)
                    rpcLoadLevelQueue.Schedule(rpcBuf, new RpcLoadLevel { levelName = Game.game.levelManager.currentLevel.name });

                NetCodeIntegration.PlayerManager.CreatePlayer(ent, networkId.Value);
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