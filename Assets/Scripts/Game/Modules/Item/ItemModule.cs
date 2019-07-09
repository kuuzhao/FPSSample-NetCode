using System.Collections.Generic;
using Unity.Entities;

public class ItemModule
{
    List<ComponentSystem> m_handleSpawnSystems = new List<ComponentSystem>();
    List<ComponentSystem> m_systems = new List<ComponentSystem>();
    GameWorld m_world;
    
    public ItemModule(GameWorld world)
    {
        m_world = world;
        
        // TODO (mogensh) make server version without all this client stuff
        m_systems.Add(world.GetECSWorld().CreateSystem<RobotWeaponClientProjectileSpawnHandler>(world));
        m_systems.Add(world.GetECSWorld().CreateSystem<TerraformerWeaponClientProjectileSpawnHandler>(world));
        m_systems.Add(world.GetECSWorld().CreateSystem<UpdateTerraformerWeaponA>(world));
        m_systems.Add(world.GetECSWorld().CreateSystem<UpdateItemActionTimelineTrigger>(world));
        m_systems.Add(world.GetECSWorld().CreateSystem<System_RobotWeaponA>(world));
    }

    public void HandleSpawn()
    {
        foreach (var system in m_handleSpawnSystems)
            system.Update();
    }

    public void Shutdown()
    {
        foreach (var system in m_handleSpawnSystems)
            m_world.GetECSWorld().DestroySystem(system);
        foreach (var system in m_systems)
            m_world.GetECSWorld().DestroySystem(system);
    }

    public void LateUpdate()
    {        
        foreach (var system in m_systems)
            system.Update();
    }
}
