using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(GhostUpdateSystemGroup))]
public class RepBarrelGhostUpdateSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(RepBarrelSnapshotData))]
    [ExcludeComponent(typeof(PredictedEntityComponent))]
    struct UpdateInterpolatedJob : IJobForEachWithEntity<Translation>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepBarrelSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref Translation ghostTranslation)
        {
            var snapshot = snapshotFromEntity[entity];
            RepBarrelSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostTranslation.Value = snapshotData.GetTranslationValue();

        }
    }
    [BurstCompile]
    [RequireComponentTag(typeof(RepBarrelSnapshotData), typeof(PredictedEntityComponent))]
    struct UpdatePredictedJob : IJobForEachWithEntity<Translation>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepBarrelSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref Translation ghostTranslation)
        {
            var snapshot = snapshotFromEntity[entity];
            RepBarrelSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostTranslation.Value = snapshotData.GetTranslationValue();

        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var updateInterpolatedJob = new UpdateInterpolatedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepBarrelSnapshotData>(),
            targetTick = NetworkTimeSystem.interpolateTargetTick
        };
        var updatePredictedJob = new UpdatePredictedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepBarrelSnapshotData>(),
            targetTick = NetworkTimeSystem.predictTargetTick
        };
        inputDeps = updateInterpolatedJob.Schedule(this, inputDeps);
        return updatePredictedJob.Schedule(this, inputDeps);
    }
}
