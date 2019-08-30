using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
public partial class RepGameModeGhostSpawnSystem : DefaultGhostSpawnSystem<RepGameModeSnapshotData>
{
    protected override EntityArchetype GetGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepGameModeSnapshotData>(),
            ComponentType.ReadWrite<RepGameMode>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>()
        );
    }
    protected override EntityArchetype GetPredictedGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepGameModeSnapshotData>(),
            ComponentType.ReadWrite<RepGameMode>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>(),
            ComponentType.ReadWrite<PredictedEntityComponent>()
        );
    }
}

[DisableAutoCreation]
public partial class RepGameModeGhostDestroySystem : DefaultGhostDestroySystem<RepGameModeSnapshotData>
{
}
