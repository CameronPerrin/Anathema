using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class SaveOnPlay
{
    static SaveOnPlay()
    {
        EditorApplication.playModeStateChanged += SaveProject;
    }

    private static void SaveProject(PlayModeStateChange playModeStateChange)
    {
        if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}