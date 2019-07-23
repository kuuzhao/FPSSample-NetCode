using UnityEngine;
using Unity.Entities;
using System.IO;
using System.Collections.Generic;

public class ReplicatedPrefabMgr
{
    static string abFolder;
    static List<string> abs;
    static Dictionary<string, GameObject> name2Prefab;

    public static void Initialize()
    {
        abFolder = GetReplicatedPrefabsPath();
        var abManifest = AssetBundle.LoadFromFile(Path.Combine(abFolder, "ReplicatedPrefabs"));
        var abm = abManifest.LoadAllAssets()[0] as AssetBundleManifest;
        abs = new List<string>(abm.GetAllAssetBundles());
        name2Prefab = new Dictionary<string, GameObject>();
    }

    public static bool IsInitialized()
    {
        return abFolder != null;
    }

    public static bool LoadPrefabIntoEntity(string abName, World ecsWorld, Entity ent)
    {
        EntityManager em = ecsWorld.EntityManager;
        if (em.HasComponent<Transform>(ent))
        {
            Debug.LogWarning("The input entity already has a prefab in it.");
            return false;
        }

        GameObject prefab = GetPrefab(abName);
        if (prefab == null)
            return false;

        var go = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.AddComponent<GameObjectEntity>();
        GameObjectEntity.AddToEntity(em, go, ent);

        return true;
    }

    public static Entity CreateEntity(string abName, World ecsWorld)
    {
        GameObject prefab = GetPrefab(abName);
        if (prefab == null)
            return Entity.Null;

        var em = ecsWorld.EntityManager;
        var ent = em.CreateEntity();

        var go = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.AddComponent<GameObjectEntity>();
        GameObjectEntity.AddToEntity(em, go, ent);

        return ent;
    }

    private static GameObject GetPrefab(string abName)
    {
        string abWithExt = abName + ".prefab";
        if (abs.IndexOf(abWithExt) == -1)
            return null;

        if (!name2Prefab.ContainsKey(abWithExt))
        {
            name2Prefab[abWithExt] = null;

            var abPath = Path.Combine(abFolder, abWithExt);
            var ab = AssetBundle.LoadFromFile(abPath);
            if (ab != null)
            {
                var objs = ab.LoadAllAssets();
                if (objs.Length > 0)
                {
                    var obj = objs[0];
                    if (obj is GameObject)
                        name2Prefab[abWithExt] = obj as GameObject;
                }

                if (name2Prefab[abWithExt] == null)
                    ab.Unload(true);
            }
        }

        return name2Prefab[abWithExt];
    }

    private static string GetReplicatedPrefabsPath()
    {
#if UNITY_EDITOR
        return Path.Combine(Directory.GetParent(Application.dataPath).FullName,
            "AutoBuild", "AssetBundles", "ReplicatedPrefabs");
#else
        return Path.Combine(Directory.GetParent(Application.dataPath).FullName,
            "AssetBundles", "ReplicatedPrefabs");
#endif
    }
}