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
    [AlwaysUpdateSystem]
    public class ClientConnetionSystem : ComponentSystem
    {
        private ServerSimulationSystemGroup m_ServerSimulationSystemGroup;
        private EntityQuery m_NetworkConnection;

        // TODO: LZ:
        //      this is a temporary workaround
        private GameObject m_ThePlayer;
        private Transform m_ThePlayerHead;

        protected override void OnCreateManager()
        {
            m_ServerSimulationSystemGroup = World.GetOrCreateSystem<ServerSimulationSystemGroup>();
            m_NetworkConnection = GetEntityQuery(ComponentType.ReadWrite<NetworkIdComponent>());
        }

        protected override void OnUpdate()
        {
            var entities = m_NetworkConnection.GetEntityArraySt();

            for (int i = 0; i < entities.Length; ++i)
            {
                var ent = entities[i];

                if (!EntityManager.HasComponent<NetworkStreamInGame>(ent))
                    EntityManager.AddComponentData(ent, new NetworkStreamInGame());

                if (!EntityManager.HasComponent<PlayerCommandData>(ent))
                {
                    EntityManager.AddBuffer<PlayerCommandData>(ent);
                    var ctc = EntityManager.GetComponentData<CommandTargetComponent>(ent);
                    ctc.targetEntity = ent;
                    EntityManager.SetComponentData(ent, ctc);
                }

                FakePlayer.PrepareFakePlayerIfNeeded();

                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData inputData;
                cmdBuf.GetDataAtTick(m_ServerSimulationSystemGroup.ServerTick, out inputData);
                if (inputData.grenade != 0)
                {
                    Debug.Log("LZ: Receive Grenade #" + inputData.grenade);

                    if (FakePlayer.m_HeadTr)
                        GrenadeManager.CreateGrenade(FakePlayer.m_HeadTr);
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