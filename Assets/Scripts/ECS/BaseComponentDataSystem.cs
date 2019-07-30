using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Profiling;

public abstract class BaseComponentSystem : ComponentSystem
{
    protected BaseComponentSystem(GameWorld world)
    {
        m_world = world;
    }

    readonly protected GameWorld m_world;
}

 public abstract class BaseComponentSystem<T1> : BaseComponentSystem
 	where T1 : MonoBehaviour
 {
 	EntityQuery Group;
 	protected ComponentType[] ExtraComponentRequirements;
	string name;

 	public BaseComponentSystem(GameWorld world) : base(world) {}

    protected override void OnCreateManager()
 	{
 		base.OnCreateManager();
		name = GetType().Name;
 		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
 		list.AddRange(new ComponentType[] { typeof(T1) } );
 		Group = GetEntityQuery(list.ToArray());
 	}
 
 	protected override void OnUpdate()
 	{
		Profiler.BeginSample(name);

 		var entityArray = Group.GetEntityArraySt();
 		var dataArray = Group.ToComponentArray<T1>();
 
 		for (var i = 0; i < entityArray.Length; i++)
 		{
 			Update(entityArray[i], dataArray[i]);
 		}

		Profiler.EndSample();
 	}
 	
 	protected abstract void Update(Entity entity,T1 data);
 }


public abstract class BaseComponentSystem<T1,T2> : BaseComponentSystem
	where T1 : MonoBehaviour
	where T2 : MonoBehaviour
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	string name; 
	
	public BaseComponentSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] {typeof(T1), typeof(T2)});
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray1 = Group.ToComponentArray<T1>();
		var dataArray2 = Group.ToComponentArray<T2>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray1[i], dataArray2[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data1,T2 data2);
}


public abstract class BaseComponentSystem<T1,T2,T3> : BaseComponentSystem
	where T1 : MonoBehaviour
	where T2 : MonoBehaviour
	where T3 : MonoBehaviour
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	string name;
	
	public BaseComponentSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] { typeof(T1), typeof(T2), typeof(T3) } );
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray1 = Group.ToComponentArray<T1>();
		var dataArray2 = Group.ToComponentArray<T2>();
		var dataArray3 = Group.ToComponentArray<T3>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray1[i], dataArray2[i], dataArray3[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data1,T2 data2,T3 data3);
}

public abstract class BaseComponentDataSystem<T1> : BaseComponentSystem
	where T1 : struct,IComponentData
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	string name;
	
	public BaseComponentDataSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] { typeof(T1) } );
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray = Group.GetComponentDataArraySt<T1>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data);
}

public abstract class BaseComponentDataSystem<T1,T2> : BaseComponentSystem
	where T1 : struct,IComponentData
	where T2 : struct,IComponentData
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	private string name;
	
	public BaseComponentDataSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		name = GetType().Name;
		base.OnCreateManager();
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] { typeof(T1), typeof(T2) } );
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray1 = Group.GetComponentDataArraySt<T1>();
		var dataArray2 = Group.GetComponentDataArraySt<T2>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray1[i], dataArray2[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data1,T2 data2);
}

public abstract class BaseComponentDataSystem<T1,T2,T3> : BaseComponentSystem
	where T1 : struct,IComponentData
	where T2 : struct,IComponentData
	where T3 : struct,IComponentData
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	string name;
	
	public BaseComponentDataSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] { typeof(T1), typeof(T2), typeof(T3) } );
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray1 = Group.GetComponentDataArraySt<T1>();
		var dataArray2 = Group.GetComponentDataArraySt<T2>();
		var dataArray3 = Group.GetComponentDataArraySt<T3>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray1[i], dataArray2[i], dataArray3[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data1,T2 data2,T3 data3);
}


public abstract class BaseComponentDataSystem<T1,T2,T3,T4> : BaseComponentSystem
	where T1 : struct,IComponentData
	where T2 : struct,IComponentData
	where T3 : struct,IComponentData
	where T4 : struct,IComponentData
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	string name;
	
	public BaseComponentDataSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) } );
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray1 = Group.GetComponentDataArraySt<T1>();
		var dataArray2 = Group.GetComponentDataArraySt<T2>();
		var dataArray3 = Group.GetComponentDataArraySt<T3>();
		var dataArray4 = Group.GetComponentDataArraySt<T4>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray1[i], dataArray2[i], dataArray3[i], dataArray4[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data1,T2 data2,T3 data3,T4 data4);
}

public abstract class BaseComponentDataSystem<T1,T2,T3,T4, T5> : BaseComponentSystem
	where T1 : struct,IComponentData
	where T2 : struct,IComponentData
	where T3 : struct,IComponentData
	where T4 : struct,IComponentData
	where T5 : struct,IComponentData
{
	EntityQuery Group;
	protected ComponentType[] ExtraComponentRequirements;
	string name;
	
	public BaseComponentDataSystem(GameWorld world) : base(world) {}
	
	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		var list = new List<ComponentType>(6);
		if(ExtraComponentRequirements != null)		
			list.AddRange(ExtraComponentRequirements);
		list.AddRange(new ComponentType[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) } );
		Group = GetEntityQuery(list.ToArray());
	}

	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

        var entityArray = Group.GetEntityArraySt();
        var dataArray1 = Group.GetComponentDataArraySt<T1>();
		var dataArray2 = Group.GetComponentDataArraySt<T2>();
		var dataArray3 = Group.GetComponentDataArraySt<T3>();
		var dataArray4 = Group.GetComponentDataArraySt<T4>();
		var dataArray5 = Group.GetComponentDataArraySt<T5>();

		for (var i = 0; i < entityArray.Length; i++)
		{
			Update(entityArray[i], dataArray1[i], dataArray2[i], dataArray3[i], dataArray4[i], dataArray5[i]);
		}

        Profiler.EndSample();
	}
	
	protected abstract void Update(Entity entity,T1 data1,T2 data2,T3 data3,T4 data4, T5 data5);
}

[AlwaysUpdateSystem]
public abstract class InitializeComponentSystem<T> : BaseComponentSystem
	where T : MonoBehaviour
{
	struct SystemState : IComponentData {}
	EntityQuery IncomingGroup;
	string name;
	
	public InitializeComponentSystem(GameWorld world) : base(world) {}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		IncomingGroup = GetEntityQuery(typeof(T),ComponentType.Exclude<SystemState>());
	}
    
	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

		var incomingEntityArray = IncomingGroup.GetEntityArraySt();
		if (incomingEntityArray.Length > 0)
		{
			var incomingComponentArray = IncomingGroup.ToComponentArray<T>();
			for (var i = 0; i < incomingComponentArray.Length; i++)
			{
				var entity = incomingEntityArray[i];
				PostUpdateCommands.AddComponent(entity,new SystemState());

				Initialize(entity, incomingComponentArray[i]);
			}
		}

        Profiler.EndSample();
	}

	protected abstract void Initialize(Entity entity, T component);
}


[AlwaysUpdateSystem]
public abstract class InitializeComponentDataSystem<T,K> : BaseComponentSystem
	where T : struct, IComponentData
	where K : struct, IComponentData
{
	
	EntityQuery IncomingGroup;
	string name;
	
	public InitializeComponentDataSystem(GameWorld world) : base(world) {}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		IncomingGroup = GetEntityQuery(typeof(T),ComponentType.Exclude<K>());
	}
    
	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

		var incomingEntityArray = IncomingGroup.GetEntityArraySt();
		if (incomingEntityArray.Length > 0)
		{
			var incomingComponentDataArray = IncomingGroup.GetComponentDataArraySt<T>();
			for (var i = 0; i < incomingComponentDataArray.Length; i++)
			{
				var entity = incomingEntityArray[i];
				PostUpdateCommands.AddComponent(entity,new K());

				Initialize(entity, incomingComponentDataArray[i]);
			}
        }

        Profiler.EndSample();
	}

	protected abstract void Initialize(Entity entity, T component);
}



[AlwaysUpdateSystem]
public abstract class DeinitializeComponentSystem<T> : BaseComponentSystem
	where T : MonoBehaviour
{
	EntityQuery OutgoingGroup;
	string name;

	public DeinitializeComponentSystem(GameWorld world) : base(world) {}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		OutgoingGroup = GetEntityQuery(typeof(T), typeof(DespawningEntity));
	}
    
	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

		var outgoingComponentArray = OutgoingGroup.ToComponentArray<T>();
		var outgoingEntityArray = OutgoingGroup.GetEntityArraySt();
		for (var i = 0; i < outgoingComponentArray.Length; i++)
		{
			Deinitialize(outgoingEntityArray[i], outgoingComponentArray[i]);
		}

        Profiler.EndSample();
	}

	protected abstract void Deinitialize(Entity entity, T component);
}


[AlwaysUpdateSystem]
public abstract class DeinitializeComponentDataSystem<T> : BaseComponentSystem
	where T : struct, IComponentData
{
	EntityQuery OutgoingGroup;
	string name;

	public DeinitializeComponentDataSystem(GameWorld world) : base(world) {}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		OutgoingGroup = GetEntityQuery(typeof(T), typeof(DespawningEntity));
	}
    
	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

		var outgoingComponentArray = OutgoingGroup.GetComponentDataArraySt<T>();
		var outgoingEntityArray = OutgoingGroup.GetEntityArraySt();
		for (var i = 0; i < outgoingComponentArray.Length; i++)
		{
			Deinitialize(outgoingEntityArray[i], outgoingComponentArray[i]);
		}

        Profiler.EndSample();
	}

	protected abstract void Deinitialize(Entity entity, T component);
}

[AlwaysUpdateSystem]
public abstract class InitializeComponentGroupSystem<T,S> : BaseComponentSystem
	where T : MonoBehaviour
	where S : struct, IComponentData
{
	EntityQuery IncomingGroup;
	string name;

	public InitializeComponentGroupSystem(GameWorld world) : base(world) {}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		IncomingGroup = GetEntityQuery(typeof(T),ComponentType.Exclude<S>());
	}
    
	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

		var incomingEntityArray = IncomingGroup.GetEntityArraySt();
		if (incomingEntityArray.Length > 0)
		{
			for (var i = 0; i < incomingEntityArray.Length; i++)
			{
				var entity = incomingEntityArray[i];
				PostUpdateCommands.AddComponent(entity,new S());
			}
			Initialize(ref IncomingGroup);
		}

        Profiler.EndSample();
	}

	protected abstract void Initialize(ref EntityQuery group);
}



[AlwaysUpdateSystem]
public abstract class DeinitializeComponentGroupSystem<T> : BaseComponentSystem
	where T : MonoBehaviour
{
	EntityQuery OutgoingGroup;
	string name;

	public DeinitializeComponentGroupSystem(GameWorld world) : base(world) {}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		name = GetType().Name;
		OutgoingGroup = GetEntityQuery(typeof(T), typeof(DespawningEntity));
	}
    
	protected override void OnUpdate()
	{
		Profiler.BeginSample(name);

		if (OutgoingGroup.CalculateLength() > 0)
			Deinitialize(ref OutgoingGroup);
		
		Profiler.EndSample();
	}

	protected abstract void Deinitialize(ref EntityQuery group);
}
