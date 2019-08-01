using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
public partial class RepGrenadeGhostSpawnSystem : DefaultGhostSpawnSystem<RepGrenadeSnapshotData>
{
    protected override EntityArchetype GetGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepGrenadeSnapshotData>(),
            ComponentType.ReadWrite<RepGrenadeTagComponentData>(),
            ComponentType.ReadWrite<Translation>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>()
        );
    }
    protected override EntityArchetype GetPredictedGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepGrenadeSnapshotData>(),
            ComponentType.ReadWrite<RepGrenadeTagComponentData>(),
            ComponentType.ReadWrite<Translation>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>(),
            ComponentType.ReadWrite<PredictedEntityComponent>()
        );
    }
}
