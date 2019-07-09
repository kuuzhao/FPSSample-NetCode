using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Profiling;


public class ProjectileModuleServer 
{
    [ConfigVar(Name = "projectile.drawserverdebug", DefaultValue = "0", Description = "Show projectilesystem debug")]
    public static ConfigVar drawDebug;
    
    public ProjectileModuleServer(GameWorld gameWorld, BundledResourceManager resourceSystem)
    {
        m_GameWorld = gameWorld;

        m_handleRequests = m_GameWorld.GetECSWorld().CreateSystem<HandleServerProjectileRequests>(m_GameWorld, resourceSystem);
        m_CreateMovementQueries =  m_GameWorld.GetECSWorld().CreateSystem<CreateProjectileMovementCollisionQueries>(m_GameWorld);
        m_HandleMovementQueries = m_GameWorld.GetECSWorld().CreateSystem<HandleProjectileMovementCollisionQuery>(m_GameWorld);
        m_DespawnProjectiles = m_GameWorld.GetECSWorld().CreateSystem<DespawnProjectiles>(m_GameWorld);
    }

    public void Shutdown()
    {
        m_GameWorld.GetECSWorld().DestroySystem(m_handleRequests);
        m_GameWorld.GetECSWorld().DestroySystem(m_CreateMovementQueries);
        m_GameWorld.GetECSWorld().DestroySystem(m_HandleMovementQueries);
        m_GameWorld.GetECSWorld().DestroySystem(m_DespawnProjectiles);
    }

    public void HandleRequests()
    {
        Profiler.BeginSample("ProjectileModuleServer.CreateMovementQueries");
        
        m_handleRequests.Update();
        
        Profiler.EndSample();
    }

   
    public void MovementStart()
    {
        Profiler.BeginSample("ProjectileModuleServer.CreateMovementQueries");
        
        m_CreateMovementQueries.Update();
        
        Profiler.EndSample();
    }

    public void MovementResolve()
    {
        Profiler.BeginSample("ProjectileModuleServer.HandleMovementQueries");
        
        m_HandleMovementQueries.Update();
        m_DespawnProjectiles.Update();
        
        Profiler.EndSample();
    }

    readonly GameWorld m_GameWorld;
    readonly HandleServerProjectileRequests m_handleRequests;
    readonly CreateProjectileMovementCollisionQueries m_CreateMovementQueries;
    readonly HandleProjectileMovementCollisionQuery m_HandleMovementQueries;
    readonly DespawnProjectiles m_DespawnProjectiles;

}
