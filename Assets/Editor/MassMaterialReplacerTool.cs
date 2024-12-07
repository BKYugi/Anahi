using UnityEditor;
using UnityEngine;

public class MassMaterialReplacerTool : EditorWindow
{
    public Material newMaterial;

    [MenuItem("Tools/Mass Material Replacer")]
    public static void ShowWindow()
    {
        GetWindow<MassMaterialReplacerTool>("Material Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Substituir Materiais em Massa", EditorStyles.boldLabel);
        newMaterial = (Material)EditorGUILayout.ObjectField("Novo Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Substituir Material em Objetos Selecionados"))
        {
            ReplaceMaterials();
        }
    }

    private void ReplaceMaterials()
    {
        if (newMaterial == null)
        {
            Debug.LogError("Por favor, selecione um novo material.");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = newMaterial;
            }
        }
        Debug.Log("Materiais substituídos.");
    }
}
