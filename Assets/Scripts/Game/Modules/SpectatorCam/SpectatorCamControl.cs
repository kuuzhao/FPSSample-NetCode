using Unity.Entities;
using UnityEngine;

public class SpectatorCamControl : MonoBehaviour 
{
}

[DisableAutoCreation]
public class UpdateSpectatorCamControl : BaseComponentSystem
{
    // TODO: LZ:
    //      to be removed
#if false
    struct GroupType
    {
        public ComponentArray<LocalPlayer> localPlayers;
        public ComponentArray<PlayerCameraSettings> cameraSettings;
        public ComponentArray<SpectatorCamControl> spectatorCamCtrls;
    }
#endif


    EntityQuery Group;
    
    public UpdateSpectatorCamControl(GameWorld world) : base(world)
    {}

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetEntityQuery(typeof(LocalPlayer), typeof(PlayerCameraSettings), typeof(SpectatorCamControl));
    }

    protected override void OnUpdate()
    {
        var localPlayerArray = Group.ToComponentArray<LocalPlayer>();
        var cameraSettingsArray = Group.ToComponentArray<PlayerCameraSettings>();
        
        for (var i = 0; i < localPlayerArray.Length; i++)
        {
            var controlledEntity = localPlayerArray[i].controlledEntity;
            
            if (controlledEntity == Entity.Null || !EntityManager.HasComponent<SpectatorCamData>(controlledEntity))
                continue;

            var spectatorCam = EntityManager.GetComponentData<SpectatorCamData>(controlledEntity);
            var cameraSettings = cameraSettingsArray[i];
            cameraSettings.isEnabled = true;
            cameraSettings.position = spectatorCam.position;
            cameraSettings.rotation = spectatorCam.rotation;
        }            
    }
}