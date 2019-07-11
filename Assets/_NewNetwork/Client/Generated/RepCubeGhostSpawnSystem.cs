using Unity.Entities;
using Unity.Transforms;

public partial class RepCubeGhostSpawnSystem : DefaultGhostSpawnSystem<RepCubeSnapshotData>
{
    protected override EntityArchetype GetGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepCubeSnapshotData>(),
            ComponentType.ReadWrite<RepCubeTagComponentData>(),
            ComponentType.ReadWrite<Translation>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>()
        );
    }
    protected override EntityArchetype GetPredictedGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepCubeSnapshotData>(),
            ComponentType.ReadWrite<RepCubeTagComponentData>(),
            ComponentType.ReadWrite<Translation>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>(),
            ComponentType.ReadWrite<PredictedEntityComponent>()
        );
    }
}
