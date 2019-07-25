using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace FpsSample.Server
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(AddNetworkIdSystem))]
    [AlwaysUpdateSystem]
    public class RepBarrelSpawnSystem : ComponentSystem
    {
        private bool mAlreadyCreated = false;

        protected override void OnUpdate()
        {
            if (mAlreadyCreated || ServerGameLoop.Instance == null || !ServerGameLoop.Instance.IsLevelLoaded())
                return;

            for (int i = 0; i < 5; ++i)
            {
                try
                {
                    var em = World.EntityManager;

                    Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_barrel_scifi_a_new", World);
                    em.AddComponent(e, typeof(RepBarrelTagComponentData));
                    em.AddComponent(e, typeof(Translation));
                    em.AddComponent(e, typeof(GhostComponent));

                    Transform tr = em.GetComponentObject<Transform>(e);
                    tr.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

                    Translation translation = new Translation { Value = new float3(-40.0f, 6.5f, -20.0f + i * 3.0f) };
                    em.SetComponentData(e, translation);
                    tr.position = translation.Value;
                }
                catch { }
            }

            mAlreadyCreated = true;
        }
    }

    // TODO: LZ:
    //      move it into a proper file
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [AlwaysUpdateSystem]
    public class ClientConnetionSystem : ComponentSystem
    {
        private ServerSimulationSystemGroup m_ServerSimulationSystemGroup;
        private EntityQuery m_NetworkConnection;

        protected override void OnCreateManager()
        {
            m_ServerSimulationSystemGroup = World.GetOrCreateSystem<ServerSimulationSystemGroup>();
            m_NetworkConnection = GetEntityQuery(ComponentType.ReadWrite<NetworkIdComponent>());
        }

        protected override void OnUpdate()
        {
            var entities = m_NetworkConnection.ToEntityArray(Allocator.TempJob);

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

                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData inputData;
                cmdBuf.GetDataAtTick(m_ServerSimulationSystemGroup.ServerTick, out inputData);
                if (inputData.grenade == 1)
                {
                    Debug.Log("LZ: Grenade");
                }
            }

            entities.Dispose();
        }
    }

    // TODO: LZ:
    //      move it into a proper file
    [DisableAutoCreation]
    public class PlayerCommandReceiveSystem : CommandReceiveSystem<PlayerCommandData>
    {
    }

}