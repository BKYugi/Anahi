using UnityEditor;
using UnityEngine;

public class AssetFinderTool : EditorWindow
{
    private string searchKeyword = "";

    [MenuItem("Tools/Asset Finder Tool")]
    public static void ShowWindow()
    {
        GetWindow<AssetFinderTool>("Asset Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Buscar Assets", EditorStyles.boldLabel);
        searchKeyword = EditorGUILayout.TextField("Palavra-chave", searchKeyword);

        if (GUILayout.Button("Buscar Assets"))
        {
            SearchAssets();
        }
    }

    private void SearchAssets()
    {
        string[] guids = AssetDatabase.FindAssets(searchKeyword);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            Debug.Log("Encontrado: " + path);
        }
    }
}
