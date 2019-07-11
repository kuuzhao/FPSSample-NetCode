using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Transforms;

public struct RepCubeGhostSerializer : IGhostSerializer<RepCubeSnapshotData>
{
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    private ComponentType componentTypeRepCubeTagComponentData;
    private ComponentType componentTypeTranslation;
    [NativeDisableContainerSafetyRestriction] private ArchetypeChunkComponentType<Translation> ghostTranslationType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public bool WantsPredictionDelta => true;

    public int SnapshotSize => UnsafeUtility.SizeOf<RepCubeSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeRepCubeTagComponentData = ComponentType.ReadWrite<RepCubeTagComponentData>();
        componentTypeTranslation = ComponentType.ReadWrite<Translation>();
        ghostTranslationType = system.GetArchetypeChunkComponentType<Translation>();

    }

    public bool CanSerialize(EntityArchetype arch)
    {
        var components = arch.GetComponentTypes();
        int matches = 0;
        for (int i = 0; i < components.Length; ++i)
        {
            if (components[i] == componentTypeRepCubeTagComponentData)
                ++matches;
            if (components[i] == componentTypeTranslation)
                ++matches;

        }
        return (matches == 2);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref RepCubeSnapshotData snapshot)
    {
        snapshot.tick = tick;
        var chunkDataTranslation = chunk.GetNativeArray(ghostTranslationType);
        snapshot.SetTranslationValue(chunkDataTranslation[ent].Value);

    }
}
