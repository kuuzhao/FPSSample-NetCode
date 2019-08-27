using Unity.Entities;

namespace NetCodeIntegration
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class NetworkConnectionMgr : ComponentSystem
    {
        public static int sNetworkId = -1;

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

                sNetworkId = networkIdComp.Value;
                UnityEngine.Debug.Log(string.Format("LZ: Connection to server established. Assigned NetworkId({0}).", sNetworkId));

                EntityManager.AddComponentData(connectionEntity, new NetworkStreamInGame());

                // TODO: LZ:
                //      hard code it here. we'll use a reliable RPC for loading levels.
                if (ClientGameLoop.Instance != null)
                    ClientGameLoop.Instance.OnMapUpdate("testlevel");
            }
        }
    }
}
