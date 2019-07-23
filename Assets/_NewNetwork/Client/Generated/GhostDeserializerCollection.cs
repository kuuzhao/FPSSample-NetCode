using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
public struct GhostDeserializerCollection : IGhostDeserializerCollection
{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
    public string[] CreateSerializerNameList()
    {
        var arr = new string[]
        {
            "RepBarrelGhostSerializer",
            "RepCubeGhostSerializer",

        };
        return arr;
    }

    public int Length => 2;
#endif
    public void Initialize(World world)
    {
        var curRepBarrelGhostSpawnSystem = world.GetOrCreateSystem<RepBarrelGhostSpawnSystem>();
        m_RepBarrelSnapshotDataNewGhostIds = curRepBarrelGhostSpawnSystem.NewGhostIds;
        m_RepBarrelSnapshotDataNewGhosts = curRepBarrelGhostSpawnSystem.NewGhosts;
        curRepBarrelGhostSpawnSystem.GhostType = 0;
        var curRepCubeGhostSpawnSystem = world.GetOrCreateSystem<RepCubeGhostSpawnSystem>();
        m_RepCubeSnapshotDataNewGhostIds = curRepCubeGhostSpawnSystem.NewGhostIds;
        m_RepCubeSnapshotDataNewGhosts = curRepCubeGhostSpawnSystem.NewGhosts;
        curRepCubeGhostSpawnSystem.GhostType = 1;

    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_RepBarrelSnapshotDataFromEntity = system.GetBufferFromEntity<RepBarrelSnapshotData>();
        m_RepCubeSnapshotDataFromEntity = system.GetBufferFromEntity<RepCubeSnapshotData>();

    }
    public void Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
        case 0:
            GhostReceiveSystem<GhostDeserializerCollection>.InvokeDeserialize(m_RepBarrelSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            break;
        case 1:
            GhostReceiveSystem<GhostDeserializerCollection>.InvokeDeserialize(m_RepCubeSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            break;

        default:
            throw new ArgumentException("Invalid serializer type");
        }
    }
    public void Spawn(int serializer, int ghostId, uint snapshot, DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                m_RepBarrelSnapshotDataNewGhostIds.Add(ghostId);
                m_RepBarrelSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<RepBarrelSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;
            case 1:
                m_RepCubeSnapshotDataNewGhostIds.Add(ghostId);
                m_RepCubeSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<RepCubeSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;

            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }

    private BufferFromEntity<RepBarrelSnapshotData> m_RepBarrelSnapshotDataFromEntity;
    private NativeList<int> m_RepBarrelSnapshotDataNewGhostIds;
    private NativeList<RepBarrelSnapshotData> m_RepBarrelSnapshotDataNewGhosts;
    private BufferFromEntity<RepCubeSnapshotData> m_RepCubeSnapshotDataFromEntity;
    private NativeList<int> m_RepCubeSnapshotDataNewGhostIds;
    private NativeList<RepCubeSnapshotData> m_RepCubeSnapshotDataNewGhosts;

}
[DisableAutoCreation]
public class FPSSampleGhostReceiveSystem : GhostReceiveSystem<GhostDeserializerCollection>
{
}
