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
            "RepCubeGhostSerializer",

        };
        return arr;
    }

    public int Length => 1;
#endif
    public void Initialize(World world)
    {
        var curRepCubeGhostSpawnSystem = world.GetOrCreateSystem<RepCubeGhostSpawnSystem>();
        m_RepCubeSnapshotDataNewGhostIds = curRepCubeGhostSpawnSystem.NewGhostIds;
        m_RepCubeSnapshotDataNewGhosts = curRepCubeGhostSpawnSystem.NewGhosts;
        curRepCubeGhostSpawnSystem.GhostType = 0;

    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_RepCubeSnapshotDataFromEntity = system.GetBufferFromEntity<RepCubeSnapshotData>();

    }
    public void Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
        case 0:
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
                m_RepCubeSnapshotDataNewGhostIds.Add(ghostId);
                m_RepCubeSnapshotDataNewGhosts.Add(GhostReceiveSystem<GhostDeserializerCollection>.InvokeSpawn<RepCubeSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;

            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }

    private BufferFromEntity<RepCubeSnapshotData> m_RepCubeSnapshotDataFromEntity;
    private NativeList<int> m_RepCubeSnapshotDataNewGhostIds;
    private NativeList<RepCubeSnapshotData> m_RepCubeSnapshotDataNewGhosts;

}
[DisableAutoCreation]
public class FPSSampleGhostReceiveSystem : GhostReceiveSystem<GhostDeserializerCollection>
{
}
