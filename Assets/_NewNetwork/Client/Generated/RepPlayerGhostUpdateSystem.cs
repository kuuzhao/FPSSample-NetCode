using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(GhostUpdateSystemGroup))]
public class RepPlayerGhostUpdateSystem : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(RepPlayerSnapshotData))]
    [ExcludeComponent(typeof(PredictedEntityComponent))]
    struct UpdateInterpolatedJob : IJobForEachWithEntity<RepPlayerComponentData>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepPlayerSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref RepPlayerComponentData ghostRepPlayerComponentData)
        {
            var snapshot = snapshotFromEntity[entity];
            RepPlayerSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostRepPlayerComponentData.position = snapshotData.GetRepPlayerComponentDataposition();
            ghostRepPlayerComponentData.rotation = snapshotData.GetRepPlayerComponentDatarotation();
            ghostRepPlayerComponentData.aimYaw = snapshotData.GetRepPlayerComponentDataaimYaw();
            ghostRepPlayerComponentData.aimPitch = snapshotData.GetRepPlayerComponentDataaimPitch();
            ghostRepPlayerComponentData.moveYaw = snapshotData.GetRepPlayerComponentDatamoveYaw();
            ghostRepPlayerComponentData.charLocoState = snapshotData.GetRepPlayerComponentDatacharLocoState();
            ghostRepPlayerComponentData.charLocoTick = snapshotData.GetRepPlayerComponentDatacharLocoTick();
            ghostRepPlayerComponentData.charAction = snapshotData.GetRepPlayerComponentDatacharAction();
            ghostRepPlayerComponentData.charActionTick = snapshotData.GetRepPlayerComponentDatacharActionTick();
            ghostRepPlayerComponentData.damageTick = snapshotData.GetRepPlayerComponentDatadamageTick();
            ghostRepPlayerComponentData.damageDirection = snapshotData.GetRepPlayerComponentDatadamageDirection();
            ghostRepPlayerComponentData.sprinting = snapshotData.GetRepPlayerComponentDatasprinting();
            ghostRepPlayerComponentData.sprintWeight = snapshotData.GetRepPlayerComponentDatasprintWeight();
            ghostRepPlayerComponentData.previousCharLocoState = snapshotData.GetRepPlayerComponentDatapreviousCharLocoState();
            ghostRepPlayerComponentData.lastGroundMoveTick = snapshotData.GetRepPlayerComponentDatalastGroundMoveTick();
            ghostRepPlayerComponentData.moveAngleLocal = snapshotData.GetRepPlayerComponentDatamoveAngleLocal();
            ghostRepPlayerComponentData.shootPoseWeight = snapshotData.GetRepPlayerComponentDatashootPoseWeight();
            ghostRepPlayerComponentData.locomotionVector = snapshotData.GetRepPlayerComponentDatalocomotionVector();
            ghostRepPlayerComponentData.locomotionPhase = snapshotData.GetRepPlayerComponentDatalocomotionPhase();
            ghostRepPlayerComponentData.banking = snapshotData.GetRepPlayerComponentDatabanking();
            ghostRepPlayerComponentData.landAnticWeight = snapshotData.GetRepPlayerComponentDatalandAnticWeight();
            ghostRepPlayerComponentData.turnStartAngle = snapshotData.GetRepPlayerComponentDataturnStartAngle();
            ghostRepPlayerComponentData.turnDirection = snapshotData.GetRepPlayerComponentDataturnDirection();
            ghostRepPlayerComponentData.squashTime = snapshotData.GetRepPlayerComponentDatasquashTime();
            ghostRepPlayerComponentData.squashWeight = snapshotData.GetRepPlayerComponentDatasquashWeight();
            ghostRepPlayerComponentData.inAirTime = snapshotData.GetRepPlayerComponentDatainAirTime();
            ghostRepPlayerComponentData.jumpTime = snapshotData.GetRepPlayerComponentDatajumpTime();
            ghostRepPlayerComponentData.simpleTime = snapshotData.GetRepPlayerComponentDatasimpleTime();
            ghostRepPlayerComponentData.footIkOffset = snapshotData.GetRepPlayerComponentDatafootIkOffset();
            ghostRepPlayerComponentData.footIkNormalLeft = snapshotData.GetRepPlayerComponentDatafootIkNormalLeft();
            ghostRepPlayerComponentData.footIkNormaRight = snapshotData.GetRepPlayerComponentDatafootIkNormaRight();
            ghostRepPlayerComponentData.velocity = snapshotData.GetRepPlayerComponentDatavelocity();

        }
    }
    [BurstCompile]
    [RequireComponentTag(typeof(RepPlayerSnapshotData), typeof(PredictedEntityComponent))]
    struct UpdatePredictedJob : IJobForEachWithEntity<RepPlayerComponentData>
    {
        [NativeDisableParallelForRestriction] public BufferFromEntity<RepPlayerSnapshotData> snapshotFromEntity;
        public uint targetTick;
        public void Execute(Entity entity, int index,
            ref RepPlayerComponentData ghostRepPlayerComponentData)
        {
            var snapshot = snapshotFromEntity[entity];
            RepPlayerSnapshotData snapshotData;
            snapshot.GetDataAtTick(targetTick, out snapshotData);

            ghostRepPlayerComponentData.position = snapshotData.GetRepPlayerComponentDataposition();
            ghostRepPlayerComponentData.rotation = snapshotData.GetRepPlayerComponentDatarotation();
            ghostRepPlayerComponentData.aimYaw = snapshotData.GetRepPlayerComponentDataaimYaw();
            ghostRepPlayerComponentData.aimPitch = snapshotData.GetRepPlayerComponentDataaimPitch();
            ghostRepPlayerComponentData.moveYaw = snapshotData.GetRepPlayerComponentDatamoveYaw();
            ghostRepPlayerComponentData.charLocoState = snapshotData.GetRepPlayerComponentDatacharLocoState();
            ghostRepPlayerComponentData.charLocoTick = snapshotData.GetRepPlayerComponentDatacharLocoTick();
            ghostRepPlayerComponentData.charAction = snapshotData.GetRepPlayerComponentDatacharAction();
            ghostRepPlayerComponentData.charActionTick = snapshotData.GetRepPlayerComponentDatacharActionTick();
            ghostRepPlayerComponentData.damageTick = snapshotData.GetRepPlayerComponentDatadamageTick();
            ghostRepPlayerComponentData.damageDirection = snapshotData.GetRepPlayerComponentDatadamageDirection();
            ghostRepPlayerComponentData.sprinting = snapshotData.GetRepPlayerComponentDatasprinting();
            ghostRepPlayerComponentData.sprintWeight = snapshotData.GetRepPlayerComponentDatasprintWeight();
            ghostRepPlayerComponentData.previousCharLocoState = snapshotData.GetRepPlayerComponentDatapreviousCharLocoState();
            ghostRepPlayerComponentData.lastGroundMoveTick = snapshotData.GetRepPlayerComponentDatalastGroundMoveTick();
            ghostRepPlayerComponentData.moveAngleLocal = snapshotData.GetRepPlayerComponentDatamoveAngleLocal();
            ghostRepPlayerComponentData.shootPoseWeight = snapshotData.GetRepPlayerComponentDatashootPoseWeight();
            ghostRepPlayerComponentData.locomotionVector = snapshotData.GetRepPlayerComponentDatalocomotionVector();
            ghostRepPlayerComponentData.locomotionPhase = snapshotData.GetRepPlayerComponentDatalocomotionPhase();
            ghostRepPlayerComponentData.banking = snapshotData.GetRepPlayerComponentDatabanking();
            ghostRepPlayerComponentData.landAnticWeight = snapshotData.GetRepPlayerComponentDatalandAnticWeight();
            ghostRepPlayerComponentData.turnStartAngle = snapshotData.GetRepPlayerComponentDataturnStartAngle();
            ghostRepPlayerComponentData.turnDirection = snapshotData.GetRepPlayerComponentDataturnDirection();
            ghostRepPlayerComponentData.squashTime = snapshotData.GetRepPlayerComponentDatasquashTime();
            ghostRepPlayerComponentData.squashWeight = snapshotData.GetRepPlayerComponentDatasquashWeight();
            ghostRepPlayerComponentData.inAirTime = snapshotData.GetRepPlayerComponentDatainAirTime();
            ghostRepPlayerComponentData.jumpTime = snapshotData.GetRepPlayerComponentDatajumpTime();
            ghostRepPlayerComponentData.simpleTime = snapshotData.GetRepPlayerComponentDatasimpleTime();
            ghostRepPlayerComponentData.footIkOffset = snapshotData.GetRepPlayerComponentDatafootIkOffset();
            ghostRepPlayerComponentData.footIkNormalLeft = snapshotData.GetRepPlayerComponentDatafootIkNormalLeft();
            ghostRepPlayerComponentData.footIkNormaRight = snapshotData.GetRepPlayerComponentDatafootIkNormaRight();
            ghostRepPlayerComponentData.velocity = snapshotData.GetRepPlayerComponentDatavelocity();

        }
    }
    // TODO: LZ:
    //      we may not have predicted job
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var updateInterpolatedJob = new UpdateInterpolatedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepPlayerSnapshotData>(),
            targetTick = NetworkTimeSystem.interpolateTargetTick
        };
        var updatePredictedJob = new UpdatePredictedJob
        {
            snapshotFromEntity = GetBufferFromEntity<RepPlayerSnapshotData>(),
            targetTick = NetworkTimeSystem.predictTargetTick
        };
        inputDeps = updateInterpolatedJob.Schedule(this, inputDeps);
        return updatePredictedJob.Schedule(this, inputDeps);
    }
}
