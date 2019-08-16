using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace NetCodeIntegration
{
    public struct GrenadeSimSettings : IComponentData
    {
        public float maxLifetime;
        public SplashDamageSettings splashDamage;
        public float proximityTriggerDist;
        public float gravity;
        public float bounciness;
        public float collisionRadius;
    }

    public struct GrenadeInternalState : IComponentData
    {
        public int active;
        public int rayQueryId;
        public float3 position;
        public float3 velocity;
        public Entity owner;
        public int teamId;
        public int startTick;   // TODO: LZ: use uint for ticks.
        public int explodeTick;
    }

    // TODO: LZ:
    //      We'd like to use a pool for better performance
    public class GrenadeManager
    {
        // TODO: LZ:
        //      the input parameter should be the player
        //      we can spawn the grenade with correct settings according to the input player
        public static void CreateGrenade(Transform headTr)
        {
            // TODO: LZ:
            //      We may also need a nice server time system in NetCode.
            //      Right now we only have NetCode.NetworkTimeSystem. This is client only.
            if (ServerGameLoop.Instance == null)
                return;
            var time = ServerGameLoop.Instance.GameWorld.worldTime;

            var pitchRot = quaternion.AxisAngle(headTr.right,
                            -math.radians(10.0f));
            var velocityDir = math.mul(pitchRot, headTr.forward);

            var world = ClientServerSystemManager.serverWorld;
            var em = world.EntityManager;
            var grenadeStartPos = headTr.position + headTr.forward * 2.0f;

            // TODO: LZ:
            //      we only need to use a simple sphere collider on the server side
            Entity e = ReplicatedPrefabMgr.CreateEntity("assets__newnetwork_prefab_robot_grenade", world);
            em.AddComponent(e, typeof(RepGrenadeTagComponentData));
            em.AddComponent(e, typeof(Translation));
            em.AddComponent(e, typeof(GhostComponent));

            em.AddComponent(e, typeof(GrenadeSimSettings));
            em.AddComponent(e, typeof(GrenadeInternalState));

            Translation translation = new Translation { Value = grenadeStartPos };
            em.SetComponentData(e, translation);

            GrenadeSimSettings settings = new GrenadeSimSettings {
                maxLifetime = 4.0f,
                proximityTriggerDist = 1.0f,
                gravity = 18.0f,
                bounciness = 0.4f,
                collisionRadius = 0.2f
            };
            em.SetComponentData(e, settings);
            GrenadeInternalState internalState = new GrenadeInternalState
            {
                active = 1,
                rayQueryId = -1,
                position = grenadeStartPos,
                velocity = velocityDir * 40.0f,
                owner = e,
                teamId = 0,         // TODO: LZ:
                startTick = time.tick,
                explodeTick = 0     // TODO: LZ:
            };
            em.SetComponentData(e, internalState);

            Transform tr = em.GetComponentObject<Transform>(e);
            tr.position = translation.Value;

            Debug.Log(string.Format("LZ: Create grenade {0}!", e.ToString()));
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(StartGrenadeMovement))]
    public class SpawnGrenade : ComponentSystem
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
            // TODO: LZ:
            //      to be removed
            FakePlayer.PrepareFakePlayerIfNeeded();

            var playerEntities = playerQuery.GetEntityArraySt();
            for (int i = 0; i < playerEntities.Length; ++i)
            {
                var ent = playerEntities[i];

                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);
                PlayerCommandData inputData;
                cmdBuf.GetDataAtTick(m_ServerSimulationSystemGroup.ServerTick, out inputData);
                if (inputData.grenade != 0)
                {
                    Debug.Log("LZ: Receive Grenade #" + inputData.grenade);

                    if (FakePlayer.m_HeadTr)
                        NetCodeIntegration.GrenadeManager.CreateGrenade(FakePlayer.m_HeadTr);
                }
            }
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FinalizeGrenadeMovement))]
    public class StartGrenadeMovement : ComponentSystem
    {
        EntityQuery grenadeQuery;

        protected override void OnCreateManager()
        {
            grenadeQuery = GetEntityQuery(
                typeof(RepGrenadeTagComponentData),
                typeof(GrenadeSimSettings),
                typeof(GrenadeInternalState));
        }

        protected override void OnUpdate()
        {
            // TODO: LZ:
            //      We may also need a nice server time system in NetCode.
            //      Right now we only have NetCode.NetworkTimeSystem. This is client only.
            if (ServerGameLoop.Instance == null)
                return;
            var time = ServerGameLoop.Instance.GameWorld.worldTime;

            var grenadeEntities = grenadeQuery.GetEntityArraySt();
            var settingsArray = grenadeQuery.GetComponentDataArraySt<GrenadeSimSettings>();
            var internalStateArray = grenadeQuery.GetComponentDataArraySt<GrenadeInternalState>();

            for (var i = 0; i < grenadeEntities.Length; ++i)
            {
                var internalState = internalStateArray[i];
                if (internalState.active == 0 || math.length(internalState.velocity) < 0.5f)
                    continue;
                var entity = grenadeEntities[i];
                var settings = settingsArray[i];

                // Crate movement query
                var startPos = internalState.position;
                var newVelocity = internalState.velocity - new float3(0, 1, 0) * settings.gravity * time.tickDuration;
                var deltaPos = newVelocity * time.tickDuration;

                internalState.position = startPos + deltaPos;
                internalState.velocity = newVelocity;

                var collisionMask = ~(1U << internalState.teamId);

                // TODO: LZ:
                //      Revise RaySphereQueryReciever.
                //      - naming is not good
                //      - move it into proper system group
                //      - is it faster than the PhysX system?
                // Setup new collision query
                var queryReciever = ServerGameLoop.Instance.ECSWorld.GetExistingSystem<RaySphereQueryReciever>();
                internalState.rayQueryId = queryReciever.RegisterQuery(new RaySphereQueryReciever.Query()
                {
                    hitCollisionTestTick = time.tick,
                    origin = startPos,
                    direction = math.normalize(newVelocity),
                    distance = math.length(deltaPos) + settings.collisionRadius,
                    radius = settings.proximityTriggerDist,
                    mask = collisionMask,
                    ExcludeOwner = time.DurationSinceTick(internalState.startTick) < 0.2f ? internalState.owner : Entity.Null,
                });

                EntityManager.SetComponentData(entity, internalState);
            }
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateBefore(typeof(FPSSampleGhostSendSystem))]
    public class FinalizeGrenadeMovement : ComponentSystem
    {
        EntityQuery grenadeQuery;

        protected override void OnCreateManager()
        {
            grenadeQuery = GetEntityQuery(
                typeof(RepGrenadeTagComponentData),
                typeof(GrenadeSimSettings),
                typeof(GrenadeInternalState));
        }

        protected override void OnUpdate()
        {
            var time = ServerGameLoop.Instance.GameWorld.worldTime;
            var queryReciever = ServerGameLoop.Instance.ECSWorld.GetExistingSystem<RaySphereQueryReciever>();

            var grenadeEntityArray = grenadeQuery.GetEntityArraySt();
            var settingsArray = grenadeQuery.GetComponentDataArraySt<GrenadeSimSettings>();
            var internalStateArray = grenadeQuery.GetComponentDataArraySt<GrenadeInternalState>();

            for (var i = 0; i < internalStateArray.Length; i++)
            {
                var internalState = internalStateArray[i];
                var entity = grenadeEntityArray[i];

                if (internalState.active == 0)
                {
                    // Keep grenades around for a short duration so shortlived grenades gets a chance to get replicated 
                    // and explode effect played

                    if (time.DurationSinceTick(internalState.explodeTick) > 1.0f)
                    {
                        // TODO: LZ:
                        // World.RequestDespawn(PostUpdateCommands, entity);
                        Debug.Log(string.Format("LZ: Grenade {0} should be destroyed!", entity.ToString()));
                        var tr = EntityManager.GetComponentObject<Transform>(entity);
                        
                        if (tr != null)
                            Object.Destroy(tr.gameObject);
                        else
                            EntityManager.DestroyEntity(entity);
                    }

                    continue;
                }

                var settings = settingsArray[i];
                var hitCollisionOwner = Entity.Null;
                if (internalState.rayQueryId != -1)
                {
                    RaySphereQueryReciever.Query query;
                    RaySphereQueryReciever.QueryResult queryResult;
                    queryReciever.GetResult(internalState.rayQueryId, out query, out queryResult);
                    internalState.rayQueryId = -1;

                    // If grenade hit something that was no hitCollision it is environment and grenade should bounce
                    if (queryResult.hit == 1 && queryResult.hitCollisionOwner == Entity.Null)
                    {
                        var moveDir = math.normalize(internalState.velocity);
                        var moveVel = math.length(internalState.velocity);

                        internalState.position = queryResult.hitPoint + queryResult.hitNormal * settings.collisionRadius;

                        moveDir = Vector3.Reflect(moveDir, queryResult.hitNormal);
                        internalState.velocity = moveDir * moveVel * settings.bounciness;

                        // TODO: LZ:
                        //      Figure out a nice way to send the bounce event to the client
                        //      so the client can play the bounce sound at the correct time
#if false
                        if (moveVel > 1.0f)
                            interpolatedState.bouncetick = m_world.worldTime.tick;
#endif
                    }

                    if (queryResult.hitCollisionOwner != Entity.Null)
                    {
                        internalState.position = queryResult.hitPoint;
                    }

                    hitCollisionOwner = queryResult.hitCollisionOwner;
                }

                // Should we explode ?
                var timeout = time.DurationSinceTick(internalState.startTick) > settings.maxLifetime;
                if (timeout || hitCollisionOwner != Entity.Null)
                {
                    internalState.active = 0;
                    internalState.explodeTick = time.tick;
                    Debug.Log(string.Format("LZ: Grenade {0} explode!", entity.ToString()));

                    if (settings.splashDamage.radius > 0)
                    {
                        var collisionMask = ~(1 << internalState.teamId);

                        // TODO: LZ:
                        //      calculate damage
#if false
                        SplashDamageRequest.Create(PostUpdateCommands, time.tick, internalState.owner, internalState.position,
                            collisionMask, settings.splashDamage);
#endif
                    }
                }

                EntityManager.SetComponentData(entity, internalState);

                //  internal state ==> ghost component
                Translation translation = new Translation { Value = internalState.position };
                EntityManager.SetComponentData(entity, translation);

                // represent it on the server
                Transform representationTransform = EntityManager.GetComponentObject<Transform>(entity);
                representationTransform.position = internalState.position;
            }
        }
    }

} // namespace NetCodeIntegration
