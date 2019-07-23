using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
public partial class RepBarrelGhostSpawnSystem : DefaultGhostSpawnSystem<RepBarrelSnapshotData>
{
    protected override EntityArchetype GetGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepBarrelSnapshotData>(),
            ComponentType.ReadWrite<RepBarrelTagComponentData>(),
            ComponentType.ReadWrite<Translation>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>()
        );
    }
    protected override EntityArchetype GetPredictedGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepBarrelSnapshotData>(),
            ComponentType.ReadWrite<RepBarrelTagComponentData>(),
            ComponentType.ReadWrite<Translation>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>(),
            ComponentType.ReadWrite<PredictedEntityComponent>()
        );
    }
}
