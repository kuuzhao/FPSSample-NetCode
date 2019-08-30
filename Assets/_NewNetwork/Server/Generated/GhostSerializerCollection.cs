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
        if (m_RepGameModeGhostSerializer.CanSerialize(arch))
            return 1;
        if (m_RepGrenadeGhostSerializer.CanSerialize(arch))
            return 2;
        if (m_RepPlayerGhostSerializer.CanSerialize(arch))
            return 3;

        throw new ArgumentException("Invalid serializer type");
    }

    public void BeginSerialize(ComponentSystemBase system)
    {
        m_RepBarrelGhostSerializer.BeginSerialize(system);
        m_RepGameModeGhostSerializer.BeginSerialize(system);
        m_RepGrenadeGhostSerializer.BeginSerialize(system);
        m_RepPlayerGhostSerializer.BeginSerialize(system);

    }

    public int CalculateImportance(int serializer, ArchetypeChunk chunk)
    {
        switch (serializer)
        {
            case 0:
                return m_RepBarrelGhostSerializer.CalculateImportance(chunk);
            case 1:
                return m_RepGameModeGhostSerializer.CalculateImportance(chunk);
            case 2:
                return m_RepGrenadeGhostSerializer.CalculateImportance(chunk);
            case 3:
                return m_RepPlayerGhostSerializer.CalculateImportance(chunk);

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
                return m_RepGameModeGhostSerializer.WantsPredictionDelta;
            case 2:
                return m_RepGrenadeGhostSerializer.WantsPredictionDelta;
            case 3:
                return m_RepPlayerGhostSerializer.WantsPredictionDelta;

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
                return m_RepGameModeGhostSerializer.SnapshotSize;
            case 2:
                return m_RepGrenadeGhostSerializer.SnapshotSize;
            case 3:
                return m_RepPlayerGhostSerializer.SnapshotSize;

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
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_RepGameModeGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (RepGameModeSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }
            case 2:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_RepGrenadeGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (RepGrenadeSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }
            case 3:
            {
                return GhostSendSystem<GhostSerializerCollection>.InvokeSerialize(m_RepPlayerGhostSerializer, serializer,
                    chunk, startIndex, currentTick, currentSnapshotEntity, (RepPlayerSnapshotData*)currentSnapshotData,
                    ghosts, ghostEntities, baselinePerEntity, availableBaselines,
                    dataStream, compressionModel);
            }

            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    private RepBarrelGhostSerializer m_RepBarrelGhostSerializer;
    private RepGameModeGhostSerializer m_RepGameModeGhostSerializer;
    private RepGrenadeGhostSerializer m_RepGrenadeGhostSerializer;
    private RepPlayerGhostSerializer m_RepPlayerGhostSerializer;

}

[DisableAutoCreation]
public class FPSSampleGhostSendSystem : GhostSendSystem<GhostSerializerCollection>
{
}
