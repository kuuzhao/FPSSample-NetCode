using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
public partial class RepPlayerGhostSpawnSystem : DefaultGhostSpawnSystem<RepPlayerSnapshotData>
{
    protected override EntityArchetype GetGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepPlayerSnapshotData>(),
            ComponentType.ReadWrite<RepPlayerTagComponentData>(),
            ComponentType.ReadWrite<RepPlayerComponentData>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>()
        );
    }
    protected override EntityArchetype GetPredictedGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepPlayerSnapshotData>(),
            ComponentType.ReadWrite<RepPlayerTagComponentData>(),
            ComponentType.ReadWrite<RepPlayerComponentData>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>(),
            ComponentType.ReadWrite<PredictedEntityComponent>()
        );
    }
}

[DisableAutoCreation]
public partial class RepPlayerGhostDestroySystem : DefaultGhostDestroySystem<RepPlayerSnapshotData>
{
}
