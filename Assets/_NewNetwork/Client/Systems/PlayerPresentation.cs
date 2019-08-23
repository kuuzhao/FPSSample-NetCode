using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace NetCodeIntegration
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ClientPresentationSystemGroup))]
    [AlwaysUpdateSystem]
    public class PlayerPresentationSystem : ComponentSystem
    {
        EntityQuery playerEntityQuery;
        EntityQuery playerGoQuery;

        protected override void OnCreateManager()
        {
            playerEntityQuery = GetEntityQuery(
                ComponentType.ReadOnly<RepPlayerTagComponentData>(),
                ComponentType.ReadOnly<RepPlayerComponentData>(),
                ComponentType.Exclude<RepPlayerGoCreatedTag>());
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
            var playerComps = playerEntityQuery.GetComponentDataArraySt<RepPlayerComponentData>();
            for (int i = 0; i < playerEntities.Length; ++i)
            {
                var playerEnt = playerEntities[i];
                var playerComp = playerComps[i];

                ReplicatedPrefabMgr.LoadPrefabIntoEntity("assets__newnetwork_prefab_robot_a_client", World, playerEnt, "lzPlayer");

                EntityManager.AddComponentData(playerEnt, default(RepPlayerGoCreatedTag));

                var cps = EntityManager.GetComponentObject<CharacterPresentationSetup>(playerEnt);
                cps.character = playerEnt;
                cps.workaroundEntityManager = EntityManager;

                var asc = EntityManager.GetComponentObject<AnimStateController>(playerEnt);
                asc.Initialize(EntityManager, playerEnt, cps.character);

                if (playerComp.networkId == NetworkConnectionMgr.sNetworkId)
                {
                    // setup local camera
                    var camEnt = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_localplayercamera", World, "LocalPlayerCamera");
                    var localPlayerCameraControl = EntityManager.GetComponentObject<LocalPlayerCameraControl>(camEnt);
                    var camera = localPlayerCameraControl.GetComponent<Camera>();
                    localPlayerCameraControl.localPlayer = playerEnt;
                    Game.game.PushCamera(camera);
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
}