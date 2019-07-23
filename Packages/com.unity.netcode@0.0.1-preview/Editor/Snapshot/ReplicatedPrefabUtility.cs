using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ReplicatedPrefabUtility
{
    [MenuItem("NetCode/BuildReplicatedPrefabs")]
    public static void BuildReplicatedPrefabs()
    {
        var abFolder = GetReplicatedPrefabsFolder();
        Directory.CreateDirectory(abFolder);

        var abBuilds = new List<AssetBundleBuild>();

        string[] replicatedPrefabGuids = AssetDatabase.FindAssets("l:ReplicatedPrefab");
        foreach(var replicatedPrefabGuid in replicatedPrefabGuids)
        {
            Debug.Log(replicatedPrefabGuid);
            string replicatePrefabPath = AssetDatabase.GUIDToAssetPath(replicatedPrefabGuid);
            Debug.Log(replicatePrefabPath);

            var abBuild = new AssetBundleBuild();
            abBuild.assetBundleName = replicatePrefabPath.Replace('/', '_');
            abBuild.assetBundleVariant = "";
            abBuild.assetNames = new string[] { replicatePrefabPath };

            abBuilds.Add(abBuild);
        }

        BuildPipeline.BuildAssetBundles(abFolder, abBuilds.ToArray(),
            BuildAssetBundleOptions.UncompressedAssetBundle,
            BuildTarget.StandaloneWindows64);

    }

    private static string GetReplicatedPrefabsFolder()
    {
        return Path.Combine(Directory.GetParent(Application.dataPath).FullName,
            "AutoBuild", "AssetBundles", "ReplicatedPrefabs");
    }
}