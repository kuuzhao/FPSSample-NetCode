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
            //      EntityQuery could return ManagedComponents from another ECS world
            //      here is the logic to check this bug
#if false
            var otherWorld = ServerGameLoop.Instance.ECSWorld;
            var otherEM = otherWorld.EntityManager;
            var otherQuery = otherEM.CreateEntityQuery(new ComponentType[] { typeof(CharacterPresentationSetup) });
            var cpsBefore = otherQuery.GetEntityArraySt();
#endif

            // TODO: LZ:
            //      use server-specific prefab
            Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_robot_a_client", world, "lzPlayer");

#if false
            var cpsAfter = otherQuery.GetEntityArraySt();
            if (cpsBefore.Length != cpsAfter.Length)
            {
                Debug.Log("Something very wrong!");
            }
#endif

            em.AddComponent(e, typeof(RepPlayerTagComponentData));
            em.AddComponent(e, typeof(RepPlayerComponentData));
            em.AddComponent(e, typeof(CharacterInterpolatedData));
            em.AddBuffer<PlayerCommandData>(e);

            // set CommandTargetComponent to this new player entity
            var ctc = em.GetComponentData<CommandTargetComponent>(networkConnectionEnt);
            ctc.targetEntity = e;
            em.SetComponentData(networkConnectionEnt, ctc);

            RepPlayerComponentData playerData = default(RepPlayerComponentData);
            playerData.position = new float3(-44.0f, 6.5f, -20.0f + playerCount * 3.0f);
            em.SetComponentData(e, playerData);

            var cid = em.GetComponentData<CharacterInterpolatedData>(e);
            cid.position = playerData.position;
            em.SetComponentData(e, cid);

            var cps = em.GetComponentObject<CharacterPresentationSetup>(e);
            cps.character = e;
            cps.workaroundEntityManager = em;

            var asc = em.GetComponentObject<AnimStateController>(e);
            asc.Initialize(em, e, cps.character);

            ++playerCount;
        }
    }
}