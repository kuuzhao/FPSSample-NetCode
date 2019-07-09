using UnityEngine;
using Unity.Entities;



[DisableAutoCreation]
public class ApplyGrenadePresentation : BaseComponentSystem
{
    EntityQuery Group;   
    
    public ApplyGrenadePresentation(GameWorld world) : base(world) { }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetEntityQuery(typeof(GrenadeClient),typeof(PresentationEntity),ComponentType.Exclude<DespawningEntity>());
    }

    protected override void OnUpdate()
    {
        var grenadeClientArray = Group.ToComponentArray<GrenadeClient>();
        var presentationArray = Group.ToComponentArray<PresentationEntity>();
        
        for (var i = 0; i < grenadeClientArray.Length; i++)
        {
            var grenadeClient = grenadeClientArray[i];
            var presentation = presentationArray[i];
            if (!EntityManager.Exists(presentation.ownerEntity))
            {
                GameDebug.LogError("ApplyGrenadePresentation. Entity does not exist;" + presentation.ownerEntity);
                continue;
            }
            
            var interpolatedState = EntityManager.GetComponentData<Grenade.InterpolatedState>(presentation.ownerEntity);
            
            grenadeClient.transform.position = interpolatedState.position;
            
            if(interpolatedState.bouncetick > grenadeClient.bounceTick)
            {
                grenadeClient.bounceTick = interpolatedState.bouncetick;
                Game.SoundSystem.Play(grenadeClient.bounceSound, interpolatedState.position);
            }
            
            if (interpolatedState.exploded == 1 && !grenadeClient.exploded)
            {
                grenadeClient.exploded = true;
                
                grenadeClient.geometry.SetActive(false);
                
                if (grenadeClient.explodeEffect != null)
                {
                    World.GetExistingSystem<HandleSpatialEffectRequests>().Request(grenadeClient.explodeEffect, 
                        interpolatedState.position,Quaternion.identity);
                }
            }
        }
    }
}

