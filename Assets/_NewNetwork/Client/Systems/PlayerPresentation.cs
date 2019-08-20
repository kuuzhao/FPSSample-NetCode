using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientPresentationSystemGroup))]
[AlwaysUpdateSystem]
public class PlayerPresentationSystem : ComponentSystem
{
    EntityQuery playerEntityQuery;
    EntityQuery playerGoQuery;

    protected override void OnCreateManager()
    {
        playerEntityQuery = GetEntityQuery(typeof(RepPlayerTagComponentData));
        playerGoQuery = GetEntityQuery(typeof(RepPlayerGoCreatedTag));
    }

    protected override void OnUpdate()
    {
        if (ClientGameLoop.Instance == null || !ClientGameLoop.Instance.IsLevelLoaded())
            return;

        if (!ReplicatedPrefabMgr.IsInitialized())
            return;

        // player entities
        var playerEntities = playerEntityQuery.GetEntityArraySt();
        for (int i = 0; i < playerEntities.Length; ++i)
        {
            var playerEnt = playerEntities[i];
            if (!EntityManager.HasComponent<RepPlayerGoCreatedTag>(playerEnt))
            {
                ReplicatedPrefabMgr.LoadPrefabIntoEntity("assets__newnetwork_prefab_robot_a_client", World, playerEnt, "lzPlayer");
#if false
                // LZ:
                //      Please note that, CharacterInterpolatedData is not supposed to be ghosted.
                EntityManager.AddComponentData(playerEnt, default(CharacterInterpolatedData));
#endif
                EntityManager.AddComponentData(playerEnt, default(RepPlayerGoCreatedTag));

                var cps = EntityManager.GetComponentObject<CharacterPresentationSetup>(playerEnt);
                cps.character = playerEnt;
                cps.workaroundEntityManager = EntityManager;

                var asc = EntityManager.GetComponentObject<AnimStateController>(playerEnt);
                asc.Initialize(EntityManager, playerEnt, cps.character);
            }
        }

        // player GameObjects
        var playerGoEntities = playerGoQuery.GetEntityArraySt();
        for (int i = 0; i < playerGoEntities.Length; ++i)
        {
            var playerGoEnt = playerGoEntities[i];

            var playerCompData = EntityManager.GetComponentData<RepPlayerComponentData>(playerGoEnt);

            var tr = EntityManager.GetComponentObject<Transform>(playerGoEnt);
            tr.position = playerCompData.position;
        }
    }
}