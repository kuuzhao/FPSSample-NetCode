using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace FpsSample.Server
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [AlwaysUpdateSystem]

    public class RepCubeSpawnSystem : JobComponentSystem
    {
        struct SpawnJob : IJob
        {
            public EntityCommandBuffer commandBuffer;
            public EntityArchetype repCubeArchetype;
            public int targetCount;

            public void Execute()
            {
                for (int i = 0; i < targetCount; ++i)
                {
                    var pos = new Translation { Value = new float3(-40.0f, 6.5f, -20.0f + i * 3.0f) };

                    var e = commandBuffer.CreateEntity(repCubeArchetype);
                    commandBuffer.SetComponent(e, pos);
                }
            }
        }

        private BeginSimulationEntityCommandBufferSystem barrier;
        private EntityArchetype repCubeArchetype;
        private bool mAlreadyCreated = false;

        protected override void OnCreateManager()
        {
            barrier = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            repCubeArchetype = ClientServerBootstrap.serverWorld.EntityManager.CreateArchetype(
                typeof(RepCubeTagComponentData), typeof(GhostComponent),
                typeof(Translation));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (mAlreadyCreated)
            {
                inputDeps.Complete();
                return default(JobHandle);
            }

            mAlreadyCreated = true;

            var spawnJob = new SpawnJob
            {
                commandBuffer = barrier.CreateCommandBuffer(),
                repCubeArchetype = repCubeArchetype,
                targetCount = 2,
            };
            var handle = spawnJob.Schedule(inputDeps);
            barrier.AddJobHandleForProducer(handle);
            return handle;
        }
    }
}