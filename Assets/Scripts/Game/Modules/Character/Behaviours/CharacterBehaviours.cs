using System.Collections.Generic;
using Unity.Entities;

public class CharacterBehaviours   
{
    public static void CreateHandleSpawnSystems(GameWorld world,SystemCollection systems, BundledResourceManager resourceManager, bool server)
    {        
        systems.Add(world.GetECSWorld().CreateSystem<HandleCharacterSpawn>(world, resourceManager, server)); // TODO (mogensh) needs to be done first as it creates presentation
        systems.Add(world.GetECSWorld().CreateSystem<HandleAnimStateCtrlSpawn>(world));
    }

    public static void CreateHandleDespawnSystems(GameWorld world,SystemCollection systems)
    {
        systems.Add(world.GetECSWorld().CreateSystem<HandleCharacterDespawn>(world));  // TODO (mogens) HandleCharacterDespawn dewpans char presentation and needs to be called before other HandleDespawn. How do we ensure this ?   
        systems.Add(world.GetECSWorld().CreateSystem<HandleAnimStateCtrlDespawn>(world));
    }

    public static void CreateAbilityRequestSystems(GameWorld world, SystemCollection systems)
    {
        systems.Add(world.GetECSWorld().CreateSystem<Movement_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<RocketJump_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Dead_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<AutoRifle_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Chaingun_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<GrenadeLauncher_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<ProjectileLauncher_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Sprint_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Melee_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Emote_RequestActive>(world));
        
        // Update main abilities
        systems.Add(world.GetECSWorld().CreateSystem<DefaultBehaviourController_Update>(world));
    }
    
    public static void CreateMovementStartSystems(GameWorld world, SystemCollection systems)
    {
        systems.Add(world.GetECSWorld().CreateSystem<GroundTest>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Movement_Update>(world));
    }

    public static void CreateMovementResolveSystems(GameWorld world, SystemCollection systems)
    {
        systems.Add(world.GetECSWorld().CreateSystem<HandleMovementQueries>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Movement_HandleCollision>(world));
    }

    public static void CreateAbilityStartSystems(GameWorld world, SystemCollection systems)
    {
        
        systems.Add(world.GetECSWorld().CreateSystem<RocketJump_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Sprint_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<AutoRifle_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<ProjectileLauncher_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Chaingun_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<GrenadeLauncher_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Melee_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Emote_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Dead_Update>(world));
    }

    public static void CreateAbilityResolveSystems(GameWorld world, SystemCollection systems)
    {
        systems.Add(world.GetECSWorld().CreateSystem<AutoRifle_HandleCollisionQuery>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Melee_HandleCollision>(world));
    }
    
}
