using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientPresentationSystemGroup))]
[AlwaysUpdateSystem]
public class GrenadeGoSystem : ComponentSystem
{
    EntityQuery grenadeQuery;
    EntityQuery grenadeGoQuery;

    protected override void OnCreateManager()
    {
        grenadeQuery = GetEntityQuery(typeof(RepGrenadeTagComponentData));
        grenadeGoQuery = GetEntityQuery(typeof(RepGrenadeGoCreatedTag));
    }

    protected override void OnUpdate()
    {
        if (ClientGameLoop.Instance == null || !ClientGameLoop.Instance.IsLevelLoaded())
            return;

        if (!ReplicatedPrefabMgr.IsInitialized())
            return;

        // grenade entities
        var grenadeEntities = grenadeQuery.GetEntityArraySt();
        for (int i = 0; i < grenadeEntities.Length; ++i)
        {
            var grenadeEnt = grenadeEntities[i];
            if (!EntityManager.HasComponent<RepGrenadeGoCreatedTag>(grenadeEnt))
            {
                ReplicatedPrefabMgr.LoadPrefabIntoEntity("assets__newnetwork_prefab_robot_grenade", World, grenadeEnt);
                EntityManager.AddComponentData(grenadeEnt, default(RepGrenadeGoCreatedTag));

                UnityEngine.Debug.Log(string.Format("LZ: new grenade entity: {0}", grenadeEnt));
            }
        }

        // grenade GameObjects
        var grenadeGoEntities = grenadeGoQuery.GetEntityArraySt();
        for (int i = 0; i < grenadeGoEntities.Length; ++i)
        {
            var grenadeGoEnt = grenadeGoEntities[i];

            var transform = EntityManager.GetComponentObject<Transform>(grenadeGoEnt);
            var translation = EntityManager.GetComponentData<Translation>(grenadeGoEnt);
            transform.position = translation.Value;
        }
    }
}