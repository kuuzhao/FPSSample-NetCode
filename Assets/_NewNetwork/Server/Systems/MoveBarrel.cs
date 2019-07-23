using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

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
        var barrelEntities = barrelQuery.ToEntityArray(Unity.Collections.Allocator.TempJob);

        for (int i = 0; i < barrelEntities.Length; ++i)
        {
            var barrelEnt = barrelEntities[i];
            Translation translation = EntityManager.GetComponentData<Translation>(barrelEnt);
            translation.Value.x = -40.0f + Mathf.Sin(Time.timeSinceLevelLoad) * 2.0f;
            EntityManager.SetComponentData(barrelEnt, translation);
            Transform tr = EntityManager.GetComponentObject<Transform>(barrelEnt);
            tr.position = translation.Value;
        }

        barrelEntities.Dispose();
    }
}
