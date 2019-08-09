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

[DisableAutoCreation]
public partial class RepGrenadeGhostDestroySystem : DefaultGhostDestroySystem<RepGrenadeSnapshotData>
{
    static readonly int positionID = UnityEngine.Shader.PropertyToID("position");

    UnityEngine.GameObject vfxGo;
    UnityEngine.Experimental.VFX.VisualEffect vfx;
    UnityEngine.Experimental.VFX.VFXEventAttribute vfxEventAttribute;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
    }

    protected override void OnUpdate()
    {
        var toBeDestroyedEntities = m_DestroyGroup.GetEntityArraySt();
        for (int i = 0; i < toBeDestroyedEntities.Length; ++i)
        {
            var toBeDestroyedEntity = toBeDestroyedEntities[i];
            var tr = EntityManager.GetComponentObject<UnityEngine.Transform>(toBeDestroyedEntity);

            if (tr != null)
            {
                if (vfxGo == null)
                {
                    vfxGo = new UnityEngine.GameObject("VfxGameObject");
                    vfx = vfxGo.AddComponent<UnityEngine.Experimental.VFX.VisualEffect>();

                    var grenadeClient = tr.GetComponent<GrenadeClient>();
                    vfx.visualEffectAsset = grenadeClient.explodeEffect.effect;
                    vfxEventAttribute = vfx.CreateVFXEventAttribute();
                }

                vfxEventAttribute.SetVector3(positionID, tr.position);
                vfx.Play(vfxEventAttribute);
                UnityEngine.Object.Destroy(tr.gameObject);
            }
        }
    }
}
