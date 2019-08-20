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

            RepPlayerComponentData playerData = default(RepPlayerComponentData);
            playerData.position = new float3(-44.0f, 6.5f, -20.0f + playerCount * 3.0f);
            em.SetComponentData(e, playerData);

            var tr = em.GetComponentObject<Transform>(e);
            tr.position = playerData.position;

            var cps = em.GetComponentObject<CharacterPresentationSetup>(e);
            cps.character = e;
            cps.workaroundEntityManager = em;

            var asc = em.GetComponentObject<AnimStateController>(e);
            asc.Initialize(em, e, cps.character);

            ++playerCount;
        }
    }
}