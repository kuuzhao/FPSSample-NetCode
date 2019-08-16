using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Transforms;

public struct RepPlayerGhostSerializer : IGhostSerializer<RepPlayerSnapshotData>
{
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    private ComponentType componentTypeRepPlayerTagComponentData;
    private ComponentType componentTypeRepPlayerComponentData;
    [NativeDisableContainerSafetyRestriction] private ArchetypeChunkComponentType<RepPlayerComponentData> ghostRepPlayerComponentDataType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public bool WantsPredictionDelta => true;

    public int SnapshotSize => UnsafeUtility.SizeOf<RepPlayerSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeRepPlayerTagComponentData = ComponentType.ReadWrite<RepPlayerTagComponentData>();
        componentTypeRepPlayerComponentData = ComponentType.ReadWrite<RepPlayerComponentData>();
        ghostRepPlayerComponentDataType = system.GetArchetypeChunkComponentType<RepPlayerComponentData>();

    }

    public bool CanSerialize(EntityArchetype arch)
    {
        var components = arch.GetComponentTypes();
        int matches = 0;
        for (int i = 0; i < components.Length; ++i)
        {
            if (components[i] == componentTypeRepPlayerTagComponentData)
                ++matches;
            if (components[i] == componentTypeRepPlayerComponentData)
                ++matches;

        }
        return (matches == 2);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref RepPlayerSnapshotData snapshot)
    {
        snapshot.tick = tick;
        var chunkDataRepPlayerComponentData = chunk.GetNativeArray(ghostRepPlayerComponentDataType);
        snapshot.SetRepPlayerComponentDataposition(chunkDataRepPlayerComponentData[ent].position);
        snapshot.SetRepPlayerComponentDatarotation(chunkDataRepPlayerComponentData[ent].rotation);
        snapshot.SetRepPlayerComponentDataaimYaw(chunkDataRepPlayerComponentData[ent].aimYaw);
        snapshot.SetRepPlayerComponentDataaimPitch(chunkDataRepPlayerComponentData[ent].aimPitch);
        snapshot.SetRepPlayerComponentDatamoveYaw(chunkDataRepPlayerComponentData[ent].moveYaw);
        snapshot.SetRepPlayerComponentDatacharLocoState(chunkDataRepPlayerComponentData[ent].charLocoState);
        snapshot.SetRepPlayerComponentDatacharLocoTick(chunkDataRepPlayerComponentData[ent].charLocoTick);
        snapshot.SetRepPlayerComponentDatacharAction(chunkDataRepPlayerComponentData[ent].charAction);
        snapshot.SetRepPlayerComponentDatacharActionTick(chunkDataRepPlayerComponentData[ent].charActionTick);
        snapshot.SetRepPlayerComponentDatadamageTick(chunkDataRepPlayerComponentData[ent].damageTick);
        snapshot.SetRepPlayerComponentDatadamageDirection(chunkDataRepPlayerComponentData[ent].damageDirection);
        snapshot.SetRepPlayerComponentDatasprinting(chunkDataRepPlayerComponentData[ent].sprinting);
        snapshot.SetRepPlayerComponentDatasprintWeight(chunkDataRepPlayerComponentData[ent].sprintWeight);
        snapshot.SetRepPlayerComponentDatapreviousCharLocoState(chunkDataRepPlayerComponentData[ent].previousCharLocoState);
        snapshot.SetRepPlayerComponentDatalastGroundMoveTick(chunkDataRepPlayerComponentData[ent].lastGroundMoveTick);
        snapshot.SetRepPlayerComponentDatamoveAngleLocal(chunkDataRepPlayerComponentData[ent].moveAngleLocal);
        snapshot.SetRepPlayerComponentDatashootPoseWeight(chunkDataRepPlayerComponentData[ent].shootPoseWeight);
        snapshot.SetRepPlayerComponentDatalocomotionVector(chunkDataRepPlayerComponentData[ent].locomotionVector);
        snapshot.SetRepPlayerComponentDatalocomotionPhase(chunkDataRepPlayerComponentData[ent].locomotionPhase);
        snapshot.SetRepPlayerComponentDatabanking(chunkDataRepPlayerComponentData[ent].banking);
        snapshot.SetRepPlayerComponentDatalandAnticWeight(chunkDataRepPlayerComponentData[ent].landAnticWeight);
        snapshot.SetRepPlayerComponentDataturnStartAngle(chunkDataRepPlayerComponentData[ent].turnStartAngle);
        snapshot.SetRepPlayerComponentDataturnDirection(chunkDataRepPlayerComponentData[ent].turnDirection);
        snapshot.SetRepPlayerComponentDatasquashTime(chunkDataRepPlayerComponentData[ent].squashTime);
        snapshot.SetRepPlayerComponentDatasquashWeight(chunkDataRepPlayerComponentData[ent].squashWeight);
        snapshot.SetRepPlayerComponentDatainAirTime(chunkDataRepPlayerComponentData[ent].inAirTime);
        snapshot.SetRepPlayerComponentDatajumpTime(chunkDataRepPlayerComponentData[ent].jumpTime);
        snapshot.SetRepPlayerComponentDatasimpleTime(chunkDataRepPlayerComponentData[ent].simpleTime);
        snapshot.SetRepPlayerComponentDatafootIkOffset(chunkDataRepPlayerComponentData[ent].footIkOffset);
        snapshot.SetRepPlayerComponentDatafootIkNormalLeft(chunkDataRepPlayerComponentData[ent].footIkNormalLeft);
        snapshot.SetRepPlayerComponentDatafootIkNormaRight(chunkDataRepPlayerComponentData[ent].footIkNormaRight);

    }
}
