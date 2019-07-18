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
    [AlwaysUpdateSystem]
    public class RepBarrelSpawnSystem : ComponentSystem
    {
        private bool mAlreadyCreated = false;

        protected override void OnUpdate()
        {
            if (mAlreadyCreated)
                return;

            if (ServerGameLoop.Instance == null || ServerGameLoop.Instance.BundledResourceManager == null)
                return;

            for (int i = 0; i < 5; ++i)
            {
                try
                {
                    World oldActive = World.Active;

                    World ecsWorld = World;
                    var em = ecsWorld.EntityManager;
                    World.Active = ecsWorld;

                    Entity e = ServerGameLoop.Instance.BundledResourceManager.CreateEntity("7e230bcf143deea4aaf52c00c7289ac2", ecsWorld);
                    em.AddComponent(e, typeof(RepCubeTagComponentData));
                    em.AddComponent(e, typeof(Translation));
                    em.AddComponent(e, typeof(GhostComponent));

                    Transform tr = em.GetComponentObject<Transform>(e);
                    tr.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

                    Translation translation = new Translation { Value = new float3(-40.0f, 6.5f, -20.0f + i * 3.0f) };
                    em.SetComponentData(e, translation);
                    tr.position = translation.Value;

                    World.Active = oldActive;
                }
                catch { }
            }

            mAlreadyCreated = true;
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [AlwaysUpdateSystem]
    public class LoadLevelSystem : ComponentSystem
    {
        private EntityQuery m_NetworkConnection;

        protected override void OnCreateManager()
        {
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
            }

            entities.Dispose();
        }
    }
}