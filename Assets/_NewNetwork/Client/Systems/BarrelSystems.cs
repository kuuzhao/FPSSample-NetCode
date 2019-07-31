using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[AlwaysUpdateSystem]
public class BarrelGoSystem : ComponentSystem
{
    EntityQuery barrelQuery;
    EntityQuery barrelGoQuery;

    protected override void OnCreateManager()
    {
        barrelQuery = GetEntityQuery(typeof(RepBarrelTagComponentData));
        barrelGoQuery = GetEntityQuery(typeof(RepBarrelGoCreatedTag));
    }

    protected override void OnUpdate()
    {
        if (ClientGameLoop.Instance == null || !ClientGameLoop.Instance.IsLevelLoaded())
            return;

        if (!ReplicatedPrefabMgr.IsInitialized())
            return;

        // barrel entities
        var barrelEntities = barrelQuery.GetEntityArraySt();
        for (int i = 0; i < barrelEntities.Length; ++i)
        {
            var barrelEnt = barrelEntities[i];
            if (!EntityManager.HasComponent<RepBarrelGoCreatedTag>(barrelEnt))
            {
                ReplicatedPrefabMgr.LoadPrefabIntoEntity("assets__newnetwork_prefab_barrel_scifi_a_new", World, barrelEnt);
                EntityManager.AddComponentData(barrelEnt, default(RepBarrelGoCreatedTag));

                // TODO: LZ:
                //      rotate it with correct value for now
                var transform = EntityManager.GetComponentObject<Transform>(barrelEnt);
                transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            }
        }

        // barrel GameObjects
        var barrelGoEntities = barrelGoQuery.GetEntityArraySt();
        for (int i = 0; i < barrelGoEntities.Length; ++i)
        {
            var barrelGoEnt = barrelGoEntities[i];

            var transform = EntityManager.GetComponentObject<Transform>(barrelGoEnt);
            var translation = EntityManager.GetComponentData<Translation>(barrelGoEnt);
            transform.position = translation.Value;
        }
    }
}