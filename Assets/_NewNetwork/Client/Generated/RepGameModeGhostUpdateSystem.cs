using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(GhostUpdateSystemGroup))]
public class RepGameModeGhostUpdateSystem : JobComponentSystem
{
    // TODO: LZ:
    //    turn it off, because of the error : Loading a managed string literal is not supported by burst    //    at RepGameModeSnapshotData.GetRepGameModegameTimerMessage
    // [BurstCompile]
    [RequireComponentTag(typeof(RepGameModeSnapshotData))]
    [ExcludeComponent(typeof(PredictedEntityComponent))]
    struct UpdateInterpolatedJob : IJobForEachWithEntity<RepGameMode>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepGameModeSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref RepGameMode ghostRepGameMode)
        {
            var snapshot = snapshotFromEntity[entity];
            RepGameModeSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostRepGameMode.gameTimerSeconds = snapshotData.GetRepGameModegameTimerSeconds();
            ghostRepGameMode.gameTimerMessage = snapshotData.GetRepGameModegameTimerMessage();
            ghostRepGameMode.teamName0 = snapshotData.GetRepGameModeteamName0();
            ghostRepGameMode.teamName1 = snapshotData.GetRepGameModeteamName1();
            ghostRepGameMode.teamScore0 = snapshotData.GetRepGameModeteamScore0();
            ghostRepGameMode.teamScore1 = snapshotData.GetRepGameModeteamScore1();

        }
    }
    // TODO: LZ:
    //    turn it off, because of the error : Loading a managed string literal is not supported by burst    //    at RepGameModeSnapshotData.GetRepGameModegameTimerMessage
    // [BurstCompile]
    [RequireComponentTag(typeof(RepGameModeSnapshotData), typeof(PredictedEntityComponent))]
    struct UpdatePredictedJob : IJobForEachWithEntity<RepGameMode>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepGameModeSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref RepGameMode ghostRepGameMode)
        {
            var snapshot = snapshotFromEntity[entity];
            RepGameModeSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostRepGameMode.gameTimerSeconds = snapshotData.GetRepGameModegameTimerSeconds();
            ghostRepGameMode.gameTimerMessage = snapshotData.GetRepGameModegameTimerMessage();
            ghostRepGameMode.teamName0 = snapshotData.GetRepGameModeteamName0();
            ghostRepGameMode.teamName1 = snapshotData.GetRepGameModeteamName1();
            ghostRepGameMode.teamScore0 = snapshotData.GetRepGameModeteamScore0();
            ghostRepGameMode.teamScore1 = snapshotData.GetRepGameModeteamScore1();

        }
    }
    // TODO: LZ:
    //      we may not have predicted job
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var updateInterpolatedJob = new UpdateInterpolatedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepGameModeSnapshotData>(),
            targetTick = NetworkTimeSystem.interpolateTargetTick
        };
        var updatePredictedJob = new UpdatePredictedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepGameModeSnapshotData>(),
            targetTick = NetworkTimeSystem.predictTargetTick
        };
        inputDeps = updateInterpolatedJob.Schedule(this, inputDeps);
        return updatePredictedJob.Schedule(this, inputDeps);
    }
}
