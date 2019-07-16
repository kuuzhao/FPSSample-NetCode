using System;
using System.Collections.Generic;
using System.Linq;

using Unity.Entities;
using UnityEngine;

internal struct FixedTimeLoop
{
    public float accumulatedTime;
    public const float fixedTimeStep = 1f / 60f;
    public const int maxTimeSteps = 4;
    public int timeSteps;

    public void BeginUpdate()
    {
        accumulatedTime += Time.deltaTime;
        timeSteps = 0;
    }
    public bool ShouldUpdate()
    {
        if (accumulatedTime < fixedTimeStep)
            return false;
        ++timeSteps;
        if (timeSteps > maxTimeSteps)
        {
            accumulatedTime = accumulatedTime % fixedTimeStep;
            return false;
        }
        accumulatedTime -= fixedTimeStep;
        return true;
    }
}
// Update loop for client and server worlds
[DisableAutoCreation]
[AlwaysUpdateSystem]
public class ServerSimulationSystemGroup : ComponentSystemGroup
{
    private BeginSimulationEntityCommandBufferSystem m_beginBarrier;
    private EndSimulationEntityCommandBufferSystem m_endBarrier;
    private uint m_ServerTick;
    public uint ServerTick => m_ServerTick;
    private FixedTimeLoop m_fixedTimeLoop;
    public float UpdateTime => Time.time - m_fixedTimeLoop.accumulatedTime;
    public float UpdateDeltaTime => FixedTimeLoop.fixedTimeStep;

    protected override void OnCreateManager()
    {
        m_beginBarrier = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_endBarrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        m_ServerTick = 1;
    }

    protected List<ComponentSystemBase> m_systemsInGroup = new List<ComponentSystemBase>();

    public override IEnumerable<ComponentSystemBase> Systems => m_systemsInGroup;

    protected override void OnUpdate()
    {
        m_fixedTimeLoop.BeginUpdate();
        while (m_fixedTimeLoop.ShouldUpdate())
        {
            m_beginBarrier.Update();
            base.OnUpdate();
            m_endBarrier.Update();
            ++m_ServerTick;
            if (m_ServerTick == 0)
                ++m_ServerTick;
        }
    }

    public override void SortSystemUpdateList()
    {
        base.SortSystemUpdateList();
        m_systemsInGroup = new List<ComponentSystemBase>(1 + m_systemsToUpdate.Count + 1);
        m_systemsInGroup.Add(m_beginBarrier);
        m_systemsInGroup.AddRange(m_systemsToUpdate);
        m_systemsInGroup.Add(m_endBarrier);
    }
}

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class ClientSimulationSystemGroup : ComponentSystemGroup
{
    private BeginSimulationEntityCommandBufferSystem m_beginBarrier;
    private EndSimulationEntityCommandBufferSystem m_endBarrier;
    private GhostSpawnSystemGroup m_ghostSpawnGroup;
#if UNITY_EDITOR
    public int ClientWorldIndex { get; set; }
#endif
    private FixedTimeLoop m_fixedTimeLoop;
    public float UpdateTime => Time.time - m_fixedTimeLoop.accumulatedTime;
    public float UpdateDeltaTime => FixedTimeLoop.fixedTimeStep;

    protected override void OnCreateManager()
    {
        m_beginBarrier = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_endBarrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        m_ghostSpawnGroup = World.GetOrCreateSystem<GhostSpawnSystemGroup>();
    }

    protected List<ComponentSystemBase> m_systemsInGroup = new List<ComponentSystemBase>();

    public override IEnumerable<ComponentSystemBase> Systems => m_systemsInGroup;

    protected override void OnUpdate()
    {
        m_fixedTimeLoop.BeginUpdate();
        while (m_fixedTimeLoop.ShouldUpdate())
        {
            m_beginBarrier.Update();
            m_ghostSpawnGroup.Update();
            base.OnUpdate();
            m_endBarrier.Update();
        }
    }

    public override void SortSystemUpdateList()
    {
        base.SortSystemUpdateList();
        m_systemsInGroup = new List<ComponentSystemBase>(1 + m_systemsToUpdate.Count + 1);
        m_systemsInGroup.Add(m_beginBarrier);
        m_systemsInGroup.AddRange(m_systemsToUpdate);
        m_systemsInGroup.Add(m_endBarrier);
    }
}

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class ClientPresentationSystemGroup : ComponentSystemGroup
{
    private BeginPresentationEntityCommandBufferSystem m_beginBarrier;
    private EndPresentationEntityCommandBufferSystem m_endBarrier;

    protected override void OnCreateManager()
    {
        m_beginBarrier = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();
        m_endBarrier = World.GetOrCreateSystem<EndPresentationEntityCommandBufferSystem>();
    }

    protected List<ComponentSystemBase> m_systemsInGroup = new List<ComponentSystemBase>();

    public override IEnumerable<ComponentSystemBase> Systems => m_systemsInGroup;

    protected override void OnUpdate()
    {
        m_beginBarrier.Update();
        base.OnUpdate();
        m_endBarrier.Update();
    }

    public override void SortSystemUpdateList()
    {
        base.SortSystemUpdateList();
        m_systemsInGroup = new List<ComponentSystemBase>(1 + m_systemsToUpdate.Count + 1);
        m_systemsInGroup.Add(m_beginBarrier);
        m_systemsInGroup.AddRange(m_systemsToUpdate);
        m_systemsInGroup.Add(m_endBarrier);
    }
}

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class ClientAndServerSimulationSystemGroup : ComponentSystemGroup
{
}

// Ticking of client and server worlds from the main world
#if !UNITY_CLIENT
[AlwaysUpdateSystem]
public class TickServerSimulationSystem : ComponentSystemGroup
{
    public override void SortSystemUpdateList()
    {
    }
}
#endif
#if !UNITY_SERVER
#if !UNITY_CLIENT
[UpdateAfter(typeof(TickServerSimulationSystem))]
#endif
[AlwaysUpdateSystem]
public class TickClientSimulationSystem : ComponentSystemGroup
{
    public override void SortSystemUpdateList()
    {
    }
}
[UpdateInGroup(typeof(PresentationSystemGroup))]
[AlwaysUpdateSystem]
public class TickClientPresentationSystem : ComponentSystemGroup
{
    public override void SortSystemUpdateList()
    {
    }
}
#endif

public class ClientServerSystemManager
{
    private static List<System.Type> allSystems;

    public static World serverWorld;
    public static ServerSimulationSystemGroup serverSimulationSystemGroup;

    public static World clientWorld;
    static ClientSimulationSystemGroup clientSimulationSystemGroup;
    static ClientPresentationSystemGroup clientPresentationSystemGroup;

    public static void CollectAllSystems()
    {
        allSystems = new List<System.Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!TypeManager.IsAssemblyReferencingEntities(assembly))
                continue;

            IReadOnlyList<Type> allTypes;
            try
            {
                allTypes = assembly.GetTypes();
            }
            catch (System.Reflection.ReflectionTypeLoadException e)
            {
                allTypes = e.Types.Where(t => t != null).ToList();
                Debug.LogWarning(
                    $"GameMain failed loading assembly: {(assembly.IsDynamic ? assembly.ToString() : assembly.Location)}");
            }

            allSystems.AddRange(allTypes.Where(FilterSystemType));
        }
    }

    public static void InitServerSystems()
    {
        serverWorld = new World("ServerWorld");
        serverSimulationSystemGroup = serverWorld.GetOrCreateSystem<ServerSimulationSystemGroup>();

        foreach (var sys in allSystems)
        {
            var groups = sys.GetCustomAttributes(typeof(UpdateInGroupAttribute), true);
            foreach (var grp in groups)
            {
                var group = grp as UpdateInGroupAttribute;

                if (group.GroupType == typeof(ServerSimulationSystemGroup) ||
                    group.GroupType == typeof(ClientAndServerSimulationSystemGroup))
                {
                    serverSimulationSystemGroup.AddSystemToUpdateList(serverWorld.GetOrCreateSystem(sys) as ComponentSystemBase);
                }
                else if (group.GroupType == typeof(ClientSimulationSystemGroup) ||
                    group.GroupType == typeof(ClientPresentationSystemGroup))
                {
                    // do nothing
                }
                else
                {
                    var mask = GetTopLevelWorldMask(group.GroupType);
                    if ((mask & WorldType.ServerWorld) != 0)
                    {
                        var groupSys = serverWorld.GetOrCreateSystem(group.GroupType) as ComponentSystemGroup;
                        groupSys.AddSystemToUpdateList(serverWorld.GetOrCreateSystem(sys) as ComponentSystemBase);
                    }
                }
            }
        }

        serverSimulationSystemGroup.SortSystemUpdateList();
        World.Active.GetOrCreateSystem<TickServerSimulationSystem>().AddSystemToUpdateList(serverSimulationSystemGroup);
    }

    public static void InitClientSystems()
    {
        clientWorld = new World("ClientWorld");
        clientSimulationSystemGroup = clientWorld.GetOrCreateSystem<ClientSimulationSystemGroup>();
        clientPresentationSystemGroup = clientWorld.GetOrCreateSystem<ClientPresentationSystemGroup>();

        foreach (var sys in allSystems)
        {
            var groups = sys.GetCustomAttributes(typeof(UpdateInGroupAttribute), true);
            foreach (var grp in groups)
            {
                var group = grp as UpdateInGroupAttribute;

                if (group.GroupType == typeof(ClientSimulationSystemGroup) ||
                    group.GroupType == typeof(ClientAndServerSimulationSystemGroup))
                {
                    clientSimulationSystemGroup.AddSystemToUpdateList(clientWorld.GetOrCreateSystem(sys) as ComponentSystemBase);
                }
                else if (group.GroupType == typeof(ClientPresentationSystemGroup))
                {
                    clientPresentationSystemGroup.AddSystemToUpdateList(clientWorld.GetOrCreateSystem(sys) as ComponentSystemBase);
                }
                else if (group.GroupType == typeof(ServerSimulationSystemGroup))
                {
                    // do nothing
                }
                else
                {
                    var mask = GetTopLevelWorldMask(group.GroupType);
                    if ((mask & WorldType.ClientWorld) != 0)
                    {
                        var groupSys = clientWorld.GetOrCreateSystem(group.GroupType) as ComponentSystemGroup;
                        groupSys.AddSystemToUpdateList(clientWorld.GetOrCreateSystem(sys) as ComponentSystemBase);
                    }
                }
            }
        }

        clientSimulationSystemGroup.SortSystemUpdateList();
        clientPresentationSystemGroup.SortSystemUpdateList();
        World.Active.GetOrCreateSystem<TickClientSimulationSystem>().AddSystemToUpdateList(clientSimulationSystemGroup);
        World.Active.GetOrCreateSystem<TickClientPresentationSystem>().AddSystemToUpdateList(clientPresentationSystemGroup);
    }

    static bool FilterSystemType(Type type)
    {
        if (!type.IsSubclassOf(typeof(ComponentSystemBase)))
            return false;
        if (type.IsAbstract || type.ContainsGenericParameters)
            return false;
        if (type.GetConstructors().All(c => c.GetParameters().Length != 0))
            return false;

        return true;
    }

    [Flags]
    enum WorldType
    {
        NoWorld = 0,
        DefaultWorld = 1,
        ClientWorld = 2,
        ServerWorld = 4
    }
    static WorldType GetTopLevelWorldMask(Type type)
    {
        var groups = type.GetCustomAttributes(typeof(UpdateInGroupAttribute), true);
        if (groups.Length == 0)
        {
            if (type == typeof(ClientAndServerSimulationSystemGroup))
                return WorldType.ClientWorld | WorldType.ServerWorld;
            if (type == typeof(ServerSimulationSystemGroup))
                return WorldType.ServerWorld;
            if (type == typeof(ClientSimulationSystemGroup) ||
                type == typeof(ClientPresentationSystemGroup))
                return WorldType.ClientWorld;
            return WorldType.DefaultWorld;
        }

        WorldType mask = WorldType.NoWorld;
        foreach (var grp in groups)
        {
            var group = grp as UpdateInGroupAttribute;
            mask |= GetTopLevelWorldMask(group.GroupType);
        }

        return mask;
    }

}
