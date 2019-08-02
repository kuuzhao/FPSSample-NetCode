using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[DisableAutoCreation]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateBefore(typeof(AddNetworkIdSystem))]
[UpdateBefore(typeof(FPSSampleGhostSendSystem))]
[AlwaysUpdateSystem]
public class RepBarrelSpawnSystem : ComponentSystem
{
    private bool mAlreadyCreated = false;

    protected override void OnUpdate()
    {
        if (mAlreadyCreated || ServerGameLoop.Instance == null || !ServerGameLoop.Instance.IsLevelLoaded())
            return;

        for (int i = 0; i < 5; ++i)
        {
            try
            {
                var em = World.EntityManager;

                Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_barrel_scifi_a_new", World);
                em.AddComponent(e, typeof(RepBarrelTagComponentData));
                em.AddComponent(e, typeof(Translation));
                em.AddComponent(e, typeof(GhostComponent));

                Transform tr = em.GetComponentObject<Transform>(e);
                tr.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

                Translation translation = new Translation { Value = new float3(-40.0f, 6.5f, -20.0f + i * 3.0f) };
                em.SetComponentData(e, translation);
                tr.position = translation.Value;
            }
            catch { }
        }

        mAlreadyCreated = true;
    }
}

[DisableAutoCreation]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateBefore(typeof(FPSSampleGhostSendSystem))]
[AlwaysUpdateSystem]
public class MoveBarrel : ComponentSystem
{
    EntityQuery barrelQuery;

    protected override void OnCreateManager()
    {
        barrelQuery = GetEntityQuery(typeof(RepBarrelTagComponentData));
    }

    protected override void OnUpdate()
    {
        var barrelEntities = barrelQuery.GetEntityArraySt();

        for (int i = 0; i < barrelEntities.Length; ++i)
        {
            var barrelEnt = barrelEntities[i];
            Translation translation = EntityManager.GetComponentData<Translation>(barrelEnt);
            translation.Value.x = -40.0f + Mathf.Sin(Time.timeSinceLevelLoad) * 2.0f;
            EntityManager.SetComponentData(barrelEnt, translation);
            Transform tr = EntityManager.GetComponentObject<Transform>(barrelEnt);
            tr.position = translation.Value;
        }
    }
}
