using Unity.Entities;

[DisableAutoCreation]
public class TeleporterSystemClient : ComponentSystem
{
	EntityQuery Group;

	public TeleporterSystemClient(GameWorld gameWorld)
	{
		m_GameWorld = gameWorld;
	}

	protected override void OnCreateManager()
	{
		base.OnCreateManager();
		Group = GetEntityQuery(typeof(TeleporterPresentationData), typeof(TeleporterClient));
	}

	protected override void OnUpdate()
	{
		var teleporterClientArray = Group.ToComponentArray<TeleporterClient>();
		var teleporterPresentationArray = Group.ToComponentDataArray<TeleporterPresentationData>(Unity.Collections.Allocator.Persistent);
		
		for(int i = 0, c = teleporterClientArray.Length; i < c; i++)
		{
			var teleporterClient = teleporterClientArray[i];
			var teleporterPresentation = teleporterPresentationArray[i];
			
			if (teleporterClient.effectEvent.Update(m_GameWorld.worldTime, teleporterPresentation.effectTick))
			{
				if (teleporterClient.effect != null)
				{
					World.GetExistingSystem<HandleSpatialEffectRequests>().Request(teleporterClient.effect, 
						teleporterClient.effectTransform.position, teleporterClient.effectTransform.rotation);
				}
			}
		}
	}

	GameWorld m_GameWorld;
}
