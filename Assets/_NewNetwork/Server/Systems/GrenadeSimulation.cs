using UnityEngine;
using Unity.Entities;
using Unity.Transforms;


public class GrenadeManager
{
    // TODO: LZ:
    //      the input parameter should be the player
    //      we can spawn the grenade with correct settings according to the input player
    public static void CreateGrenade(Transform headTr)
    {
        var world = ClientServerSystemManager.serverWorld;
        var em = world.EntityManager;
        var grenadeStartPos = headTr.position + headTr.forward;

        // TODO: LZ:
        //      we only need to use a simple sphere collider on the server side
        Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_robot_grenade", world);
        em.AddComponent(e, typeof(RepGrenadeTagComponentData));
        em.AddComponent(e, typeof(Translation));
        em.AddComponent(e, typeof(GhostComponent));

        Transform tr = em.GetComponentObject<Transform>(e);

        Translation translation = new Translation { Value = grenadeStartPos };
        em.SetComponentData(e, translation);
        tr.position = translation.Value;
    }
}