using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace NetCodeIntegration
{
    public class PlayerManager
    {
        static int playerCount = 0;

        public static void CreatePlayer(Entity networkConnectionEnt)
        {
            if (ServerGameLoop.Instance == null)
                return;

            var world = ClientServerSystemManager.serverWorld;
            var em = world.EntityManager;

            // TODO: LZ:
            //      use server-specific prefab
            Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_robot_a_client", world, "lzPlayer");

            em.AddComponent(e, typeof(RepPlayerTagComponentData));
            em.AddComponent(e, typeof(RepPlayerComponentData));
#if false
            // LZ:
            //      Please note that, CharacterInterpolatedData is not supposed to be ghosted.
            em.AddComponent(e, typeof(CharacterInterpolatedData));
#endif
            em.AddBuffer<PlayerCommandData>(e);
            em.AddComponent(e, typeof(GhostComponent));

            // set CommandTargetComponent to this new player entity
            var ctc = em.GetComponentData<CommandTargetComponent>(networkConnectionEnt);
            ctc.targetEntity = e;
            em.SetComponentData(networkConnectionEnt, ctc);

            RepPlayerComponentData playerCompData = default(RepPlayerComponentData);
            playerCompData.position = new float3(-44.0f, 6.5f, -20.0f + playerCount * 3.0f);
            em.SetComponentData(e, playerCompData);

            var tr = em.GetComponentObject<Transform>(e);
            tr.position = playerCompData.position;

            var cps = em.GetComponentObject<CharacterPresentationSetup>(e);
            cps.character = e;
            cps.workaroundEntityManager = em;

            var asc = em.GetComponentObject<AnimStateController>(e);
            asc.Initialize(em, e, cps.character);

            ++playerCount;
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FPSSampleGhostSendSystem))]
    public class PlayerMovement : ComponentSystem
    {
        private ServerSimulationSystemGroup m_ServerSimulationSystemGroup;
        EntityQuery playerQuery;

        protected override void OnCreateManager()
        {
            m_ServerSimulationSystemGroup = World.GetOrCreateSystem<ServerSimulationSystemGroup>();

            playerQuery = GetEntityQuery(
                typeof(RepPlayerTagComponentData),
                typeof(RepPlayerComponentData),
                typeof(PlayerCommandData));
        }

        protected override void OnUpdate()
        {
            var playerEntities = playerQuery.GetEntityArraySt();
            for (int i = 0; i < playerEntities.Length; ++i)
            {
                var ent = playerEntities[i];

                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData inputData;
                cmdBuf.GetDataAtTick(m_ServerSimulationSystemGroup.ServerTick, out inputData);

                Debug.Log(inputData.ToString());
            }
        }
    }

#if false
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FPSSampleGhostSendSystem))]
    public class AnimStatControllerSystem : ComponentSystem
    {
        EntityQuery playerQuery;

        protected override void OnCreateManager()
        {
            playerQuery = GetEntityQuery(
                typeof(RepPlayerTagComponentData),
                typeof(AnimStateController));
        }

        protected override void OnUpdate()
        {
            var animStatControllerArray = playerQuery.ToComponentArray<AnimStateController>();

            for (int i = 0; i < animStatControllerArray.Length; ++i)
            {
                var animStatController = animStatControllerArray[i];
                // TODO: LZ:
                //      don't use ServerGameLoop.Instance.GameWorld
                animStatController.UpdatePresentationState(ServerGameLoop.Instance.GameWorld.worldTime,
                    ServerGameLoop.Instance.GameWorld.frameDuration);
                animStatController.ApplyPresentationState(ServerGameLoop.Instance.GameWorld.worldTime,
                    ServerGameLoop.Instance.GameWorld.frameDuration);
            }
        }
    }
#endif
}