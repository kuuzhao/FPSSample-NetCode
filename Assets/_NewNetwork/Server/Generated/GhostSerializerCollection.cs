using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
public struct GhostSerializerCollection : IGhostSerializerCollection
{
    public int FindSerializer(EntityArchetype arch)
    {
        if (m_RepBarrelGhostSerializer.CanSerialize(arch))
            return 0;
        if (m_RepCubeGhostSerializer.CanSerialize(arch))
            return 1;

        throw new ArgumentException("Invalid serializer type");
    }

    public void BeginSerialize(ComponentSystemBase system)
    {
        m_RepBarrelGhostSerializer.BeginSerialize(system);
        m_RepCubeGhostSerializer.BeginSerialize(system);

    }

    public int CalculateImportance(int serializer, ArchetypeChunk chunk)
    {
        switch (serializer)
        {
            case 0:
                return m_RepBarrelGhostSerializer.CalculateImportance(chunk);
            case 1:
                return m_RepCubeGhostSerializer.CalculateImportance(chunk);

        }

        throw new ArgumentException("Invalid serializer type");
    }

    public bool WantsPredictionDelta(int serializer)
    {
        switch (serializer)
        {
            case 0:
                return m_RepBarrelGhostSerializer.WantsPredictionDelta;
            case 1:
                return m_RepCubeGhostSerializer.WantsPredictionDelta;

        }

        throw new ArgumentException("Invalid serializer type");
    }

    public int GetSnapshotSize(int serializer)
    {
        switch (serializer)
        {
            case 0:
                return m_RepBarrelGhostSerializer.SnapshotSize;
            case 1:
                return m_RepCubeGhostSerializer.SnapshotSize;

        }

        throw new ArgumentException("Invalid serializer type");
    }

    public unsafe int Serialize(int serializer, ArchetypeChunk chunk, int startIndex, uint currentTick,
        Entity* currentSnapshotEntity, void* currentSnapshotData,
        GhostSystemStateComponent* ghosts, NativeArray<Entity> ghostEntities,
        NativeArray<int> baselinePerEntity, NativeList<SnapshotBaseline> availableBaselines,
        DataStreamWriter dataStream, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_RepBarrelGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (RepBarrelSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }
            case 1:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_RepCubeGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (RepCubeSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }

            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    private RepBarrelGhostSerializer m_RepBarrelGhostSerializer;
    private RepCubeGhostSerializer m_RepCubeGhostSerializer;

}

[DisableAutoCreation]
public class FPSSampleGhostSendSystem : GhostSendSystem<GhostSerializerCollection>
{
}
