using Unity.Entities;

[DisableAutoCreation]
public class TeleporterSystemServer : ComponentSystem
{

    public TeleporterSystemServer(GameWorld gameWorld)
    {
        m_GameWorld = gameWorld;
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        m_TeleporterServerGroup = GetEntityQuery(typeof(TeleporterServer), typeof(TeleporterPresentationData));
    }

    protected override void OnUpdate()
    {
        var teleporters = m_TeleporterServerGroup.ToComponentArray<TeleporterServer>();
        var presentationArray = m_TeleporterServerGroup.ToComponentDataArray<TeleporterPresentationData>(Unity.Collections.Allocator.Persistent);
        var entities = m_TeleporterServerGroup.ToEntityArray(Unity.Collections.Allocator.Persistent);
        for (int i = 0, c = teleporters.Length; i < c; i++)
        {
            var t = teleporters[i];

            if (t.characterInside != null)
            {
                

                if (t.characterInside.owner != Entity.Null && EntityManager.HasComponent<Character>(t.characterInside.owner))
                {
                    var character = EntityManager.GetComponentObject<Character>(t.characterInside.owner);    
                    
                    var dstPos = t.targetTeleporter.GetSpawnPositionWorld();
                    var dstRot = t.targetTeleporter.GetSpawnRotationWorld();

                    character.TeleportTo(dstPos, dstRot);

                    var presentation = presentationArray[i];
                    presentation.effectTick = m_GameWorld.worldTime.tick;
                    EntityManager.SetComponentData(entities[i],presentation);
                }
                t.characterInside = null;

            }
        }

        presentationArray.Dispose();
        entities.Dispose();
    }

    GameWorld m_GameWorld;
    private EntityQuery m_TeleporterServerGroup;
}
