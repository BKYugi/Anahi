using UnityEditor;
using UnityEngine;

public class PlaceholderTool : EditorWindow
{
    // Variáveis para os placeholders
    public GameObject placeholder;
    public GameObject newPlaceholder;

    [MenuItem("Tools/Placeholder Tool")]
    public static void ShowWindow()
    {
        // Cria a janela no Editor
        GetWindow<PlaceholderTool>("Placeholder Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Trocar Placeholders", EditorStyles.boldLabel);

        // Campo para selecionar o placeholder atual
        placeholder = (GameObject)EditorGUILayout.ObjectField("Placeholder Atual", placeholder, typeof(GameObject), true);

        // Campo para selecionar o novo placeholder
        newPlaceholder = (GameObject)EditorGUILayout.ObjectField("Novo Placeholder", newPlaceholder, typeof(GameObject), true);

        // Botão para trocar os placeholders
        if (GUILayout.Button("Trocar Placeholder"))
        {
            TrocarPlaceholders();
        }
    }

    private void TrocarPlaceholders()
    {
        if (placeholder == null || newPlaceholder == null)
        {
            Debug.LogError("Por favor, selecione ambos os placeholders.");
            return;
        }

        // Encontra todos os placeholders na cena
        GameObject[] allPlaceholders = GameObject.FindGameObjectsWithTag(placeholder.tag);

        foreach (GameObject obj in allPlaceholders)
        {
            // Instancia o novo placeholder na posição do antigo
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(newPlaceholder);
            newObj.transform.position = obj.transform.position;
            newObj.transform.rotation = obj.transform.rotation;

            // Remove o placeholder antigo
            DestroyImmediate(obj);
        }
    }
}
