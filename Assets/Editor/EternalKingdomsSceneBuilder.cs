#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using EternalKingdoms;

public static class EternalKingdomsSceneBuilder
{
    [MenuItem("Eternal Kingdoms/Create Demo Scene")]
    public static void CreateDemoScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var cameraGo = new GameObject("Main Camera");
        var camera = cameraGo.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.035f, 0.045f, 0.075f);
        cameraGo.transform.position = new Vector3(0, 0, -10);
        cameraGo.AddComponent<AudioListener>();

        var ui = new GameObject("Eternal Kingdoms UI");
        ui.AddComponent<EternalKingdomsBootstrap>();

        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/EternalKingdomsDemo.unity");
        EditorBuildSettings.scenes = new[] {
            new EditorBuildSettingsScene("Assets/Scenes/EternalKingdomsDemo.unity", true)
        };
        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Eternal Kingdoms", "Escena creada en Assets/Scenes/EternalKingdomsDemo.unity", "Aceptar");
    }
}
#endif
