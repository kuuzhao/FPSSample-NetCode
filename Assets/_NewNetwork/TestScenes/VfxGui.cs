using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class VfxGui : MonoBehaviour
{
    static readonly int positionID = Shader.PropertyToID("position");
    static readonly int directionID = Shader.PropertyToID("direction");

    VisualEffect vfx1;
    VFXEventAttribute vfxEventAttribute1;

    VisualEffect vfx2;
    VFXEventAttribute vfxEventAttribute2;

    // Start is called before the first frame update
    void Start()
    {
        ReplicatedPrefabMgr.Initialize();
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 50), "play1"))
        {
            if (vfx1 == null)
            {
                var vfxGo1 = new UnityEngine.GameObject("VfxGo1");
                vfx1 = vfxGo1.AddComponent<UnityEngine.Experimental.VFX.VisualEffect>();

                var grenadeClient = GetComponent<GrenadeClient>();

                vfx1.visualEffectAsset = grenadeClient.explodeEffect.effect;
                vfxEventAttribute1 = vfx1.CreateVFXEventAttribute();
                vfxEventAttribute1.SetVector3(positionID, new Vector3(20.0f, 0.0f, 0.0f));
            }

            vfx1.Play(vfxEventAttribute1);
        }

#if false
        if (vfx1 != null && Time.frameCount % 30 == 0)
        {
            vfx1.Play(vfxEventAttribute1);
        }
#endif

        if (GUI.Button(new Rect(50, 150, 100, 50), "play2"))
        {
            if (vfx2 == null)
            {
                var vfxGo2 = new UnityEngine.GameObject("VfxGo2");
                vfx2 = vfxGo2.AddComponent<UnityEngine.Experimental.VFX.VisualEffect>();

                var grenadePrefab = ReplicatedPrefabMgr.GetPrefab("assets__newnetwork_prefab_robot_grenade");
                var grenadeClient = grenadePrefab.GetComponent<GrenadeClient>();

                vfx2.visualEffectAsset = grenadeClient.explodeEffect.effect;
                vfxEventAttribute2 = vfx2.CreateVFXEventAttribute();
                vfxEventAttribute2.SetVector3(positionID, new Vector3(20.0f, 0.0f, 0.0f));
            }

            vfx2.Play(vfxEventAttribute2);
        }
    }
}
