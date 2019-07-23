using System.Reflection;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public static class SceneCamFovFixer
{
    [MenuItem(k_MenuName, true)]
    public static bool ToggleFovValidate()
    {
        Menu.SetChecked(k_MenuName, s_Enabled);
        return true;
    }

    [MenuItem(k_MenuName)]
    public static void ToggleFov()
    {
        SetEnabled(!s_Enabled);
        EditorPrefs.SetBool(k_EditorPrefKey, s_Enabled);
    }

    static SceneCamFovFixer()
    {
        var enabled = EditorPrefs.GetBool(k_EditorPrefKey);
        SetEnabled(enabled);
    }

    static void SetEnabled(bool enabled)
    {
        if (enabled)
            OnEnable();
        else
            OnDisable();
    }

    static void OnEnable()
    {
        if (s_Enabled)
            return;
        SceneView.beforeSceneGui += preSceneGUICallback;
        SceneView.RepaintAll();
        s_Enabled = true;
    }

    static void OnDisable()
    {
        if (!s_Enabled)
            return;
        SceneView.beforeSceneGui -= preSceneGUICallback;
        SceneView.RepaintAll();
        s_Enabled = false;
    }

    static bool rightMouseIsDown = false;
    static void preSceneGUICallback(SceneView sceneView)
    {
        if (Camera.main != null)
        {
            sceneView.camera.fieldOfView = Camera.main.fieldOfView;
            sceneView.camera.nearClipPlane = Camera.main.nearClipPlane;
            sceneView.camera.farClipPlane = Camera.main.farClipPlane;
            sceneView.camera.cullingMask = Camera.main.cullingMask;

            if (sceneView.orthographic)
                return;
        }

        sceneView.size = 10.0f;

        var currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
            rightMouseIsDown = true;
        if (currentEvent.type == EventType.MouseUp && currentEvent.button == 1)
            rightMouseIsDown = false;

        if (currentEvent.type == EventType.ScrollWheel && !rightMouseIsDown)
            sceneView.pivot += -Camera.current.transform.forward * currentEvent.delta.y * 0.50f;
    }

    const string k_MenuName = "FPS Sample/Fix sceneview fov";

    static bool s_Enabled;
    static readonly string k_EditorPrefKey = "EnableFovFixer";
}
