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
        EntityQuery cmdTargetQuery;

        private const float k_default3PDisst = 2.5f;
        private float camDist3P = k_default3PDisst;

        protected override void OnCreateManager()
        {
            localPlayerCameraQuery = GetEntityQuery(
                ComponentType.ReadWrite<LocalPlayerCameraControl>());

            cmdTargetQuery = GetEntityQuery(
                ComponentType.ReadOnly<CommandTargetComponent>());

        }

        protected override void OnUpdate()
        {
            var localPlayerCameraEnts = localPlayerCameraQuery.GetEntityArraySt();
            var cmdTargetEntities = cmdTargetQuery.GetEntityArraySt();

            if (localPlayerCameraEnts.Length == 1 && cmdTargetEntities.Length == 1)
            {
                var localPlayerCameraEnt = localPlayerCameraEnts[0];
                Entity cmdTargetEnt = cmdTargetEntities[0];

                var localPlayerCameraControl = EntityManager.GetComponentObject<LocalPlayerCameraControl>(localPlayerCameraEnt);
                var localPlayerCameraTr = localPlayerCameraControl.GetComponent<Transform>();
                var localPlayerEnt = localPlayerCameraControl.localPlayer;
                var playerComp = EntityManager.GetComponentData<RepPlayerComponentData>(localPlayerEnt);

                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(cmdTargetEnt);
                PlayerCommandData cmd;
                cmdBuf.GetDataAtTick(NetworkTimeSystem.predictTargetTick, out cmd);

                // TODO: LZ:
                //      I only implement a 3rd person camera here
                //      We also need a 1st person camera
                {
                    var lookRotation = Quaternion.Euler(new Vector3(90.0f - cmd.lookPitch, cmd.lookYaw, 0.0f));

                    float3 eyePos = playerComp.position + (float3)Vector3.up * 1.8f /*playerComp.eyeHeight*/;

                    // Simple offset of camera for better 3rd person view. This is only for animation debug atm
                    var viewDir = (float3)(lookRotation * Vector3.forward);
                    eyePos += -camDist3P * viewDir;
                    eyePos += (float3)(lookRotation * Vector3.right * 0.5f) + (float3)(lookRotation * Vector3.up * 0.5f);

                    localPlayerCameraTr.position = eyePos;
                    localPlayerCameraTr.rotation = lookRotation;
                }
            }
        }
    }
}
