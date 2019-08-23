using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace NetCodeIntegration
{
    public class LocalPlayerCameraControl : MonoBehaviour
    {
        public Entity localPlayer;
    }


    [DisableAutoCreation]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateAfter(typeof(NetCodeInputSystem))] // TODO: LZ: revise the update order here!
    public class UpdateLocalPlayerCamera : ComponentSystem
    {
        EntityQuery localPlayerCameraQuery;
        protected override void OnCreateManager()
        {
            localPlayerCameraQuery = GetEntityQuery(
                ComponentType.ReadWrite<LocalPlayerCameraControl>()
                );
        }

        protected override void OnUpdate()
        {
            var localPlayerCameraEnts = localPlayerCameraQuery.GetEntityArraySt();
            if (localPlayerCameraEnts.Length == 1)
            {
                var localPlayerCameraEnt = localPlayerCameraEnts[0];
                var localPlayerCameraControl = EntityManager.GetComponentObject<LocalPlayerCameraControl>(localPlayerCameraEnt);
                var localPlayerCameraTr = localPlayerCameraControl.GetComponent<Transform>();
                var localPlayerEnt = localPlayerCameraControl.localPlayer;
                var localPlayerCompData = EntityManager.GetComponentData<RepPlayerComponentData>(localPlayerEnt);
                localPlayerCameraTr.position = localPlayerCompData.position + new float3(0.0f, 2.0f, -3.0f);
            }
        }
    }
}
