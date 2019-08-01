using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(GhostUpdateSystemGroup))]
public class RepGrenadeGhostUpdateSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(RepGrenadeSnapshotData))]
    [ExcludeComponent(typeof(PredictedEntityComponent))]
    struct UpdateInterpolatedJob : IJobForEachWithEntity<Translation>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepGrenadeSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref Translation ghostTranslation)
        {
            var snapshot = snapshotFromEntity[entity];
            RepGrenadeSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostTranslation.Value = snapshotData.GetTranslationValue();

        }
    }
    [BurstCompile]
    [RequireComponentTag(typeof(RepGrenadeSnapshotData), typeof(PredictedEntityComponent))]
    struct UpdatePredictedJob : IJobForEachWithEntity<Translation>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepGrenadeSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref Translation ghostTranslation)
        {
            var snapshot = snapshotFromEntity[entity];
            RepGrenadeSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostTranslation.Value = snapshotData.GetTranslationValue();

        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var updateInterpolatedJob = new UpdateInterpolatedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepGrenadeSnapshotData>(),
            targetTick = NetworkTimeSystem.interpolateTargetTick
        };
        var updatePredictedJob = new UpdatePredictedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepGrenadeSnapshotData>(),
            targetTick = NetworkTimeSystem.predictTargetTick
        };
        inputDeps = updateInterpolatedJob.Schedule(this, inputDeps);
        return updatePredictedJob.Schedule(this, inputDeps);
    }
}
