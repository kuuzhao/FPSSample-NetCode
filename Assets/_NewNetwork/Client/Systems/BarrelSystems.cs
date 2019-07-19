using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[UpdateBefore(typeof(FPSSampleGhostSendSystem))]
[AlwaysUpdateSystem]
public class BarrelGoSystem : ComponentSystem
{
    EntityQuery barrelQuery;
    EntityQuery barrelGoQuery;

    protected override void OnCreateManager()
    {
        barrelQuery = GetEntityQuery(typeof(RepCubeTagComponentData));
        barrelGoQuery = GetEntityQuery(typeof(RepCubeGoCreatedTag));
    }

    protected override void OnUpdate()
    {
        BundledResourceManager brm = ClientGameLoop.Instance.BundledResourceManager;
        if (brm == null)
            return;

        // barrel entities
        var barrelEntities = barrelQuery.ToEntityArray(Unity.Collections.Allocator.TempJob);
        for (int i = 0; i < barrelEntities.Length; ++i)
        {
            var barrelEnt = barrelEntities[i];
            if (!EntityManager.HasComponent<RepCubeGoCreatedTag>(barrelEnt))
            {
                brm.LoadPrefabIntoEntity("2682a5c3bf95e45448fe4b6656605666", World, barrelEnt);
                EntityManager.AddComponentData(barrelEnt, default(RepCubeGoCreatedTag));

                // TODO: LZ:
                //      rotate it with correct value for now
                var transform = EntityManager.GetComponentObject<Transform>(barrelEnt);
                transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            }
        }

        barrelEntities.Dispose();

        // barrel GameObjects
        var barrelGoEntities = barrelGoQuery.ToEntityArray(Unity.Collections.Allocator.TempJob);
        for (int i = 0; i < barrelGoEntities.Length; ++i)
        {
            var barrelGoEnt = barrelGoEntities[i];

            var transform = EntityManager.GetComponentObject<Transform>(barrelGoEnt);
            var translation = EntityManager.GetComponentData<Translation>(barrelGoEnt);
            transform.position = translation.Value;
        }

        barrelGoEntities.Dispose();
    }
}