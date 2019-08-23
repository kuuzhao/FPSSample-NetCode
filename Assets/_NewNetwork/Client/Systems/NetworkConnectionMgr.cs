using Unity.Entities;

namespace NetCodeIntegration
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class NetworkConnectionMgr : ComponentSystem
    {
        EntityQuery connectionQuery;
        protected override void OnCreateManager()
        {
            connectionQuery = GetEntityQuery(
                typeof(NetworkIdComponent),
                ComponentType.Exclude<NetworkStreamInGame>()
            );
        }

        protected override void OnUpdate()
        {
            var connectionEntities = connectionQuery.GetEntityArraySt();
            var networkIdComps = connectionQuery.GetComponentDataArraySt<NetworkIdComponent>();

            if (connectionEntities.Length == 1)
            {
                var connectionEntity = connectionEntities[0];
                var networkIdComp = networkIdComps[0];

                EntityManager.AddComponentData(connectionEntity, new NetworkStreamInGame());
            }
        }
    }
}
