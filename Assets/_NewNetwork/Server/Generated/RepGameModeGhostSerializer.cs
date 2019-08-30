using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Transforms;

public struct RepGameModeGhostSerializer : IGhostSerializer<RepGameModeSnapshotData>
{
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    private ComponentType componentTypeRepGameMode;
    [NativeDisableContainerSafetyRestriction] private ArchetypeChunkComponentType<RepGameMode> ghostRepGameModeType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public bool WantsPredictionDelta => true;

    public int SnapshotSize => UnsafeUtility.SizeOf<RepGameModeSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeRepGameMode = ComponentType.ReadWrite<RepGameMode>();
        ghostRepGameModeType = system.GetArchetypeChunkComponentType<RepGameMode>();

    }

    public bool CanSerialize(EntityArchetype arch)
    {
        var components = arch.GetComponentTypes();
        int matches = 0;
        for (int i = 0; i < components.Length; ++i)
        {
            if (components[i] == componentTypeRepGameMode)
                ++matches;

        }
        return (matches == 1);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref RepGameModeSnapshotData snapshot)
    {
        snapshot.tick = tick;
        var chunkDataRepGameMode = chunk.GetNativeArray(ghostRepGameModeType);
        snapshot.SetRepGameModegameTimerSeconds(chunkDataRepGameMode[ent].gameTimerSeconds);
        snapshot.SetRepGameModegameTimerMessage(chunkDataRepGameMode[ent].gameTimerMessage);
        snapshot.SetRepGameModeteamName0(chunkDataRepGameMode[ent].teamName0);
        snapshot.SetRepGameModeteamName1(chunkDataRepGameMode[ent].teamName1);
        snapshot.SetRepGameModeteamScore0(chunkDataRepGameMode[ent].teamScore0);
        snapshot.SetRepGameModeteamScore1(chunkDataRepGameMode[ent].teamScore1);

    }
}
