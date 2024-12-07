using UnityEditor;
using UnityEngine;

public class SceneStatisticsTool : EditorWindow
{
    [MenuItem("Tools/Scene Statistics")]
    public static void ShowWindow()
    {
        GetWindow<SceneStatisticsTool>("Scene Statistics");
    }

    private void OnGUI()
    {
        GUILayout.Label("Estatísticas da Cena", EditorStyles.boldLabel);

        if (GUILayout.Button("Mostrar Estatísticas"))
        {
            ShowStatistics();
        }
    }

    private void ShowStatistics()
    {
        int objectCount = FindObjectsOfType<GameObject>().Length;
        int triangleCount = 0;
        int vertexCount = 0;

        foreach (MeshFilter mf in FindObjectsOfType<MeshFilter>())
        {
            Mesh mesh = mf.sharedMesh;
            if (mesh != null)
            {
                triangleCount += mesh.triangles.Length / 3;
                vertexCount += mesh.vertexCount;
            }
        }

        Debug.Log("Número de Objetos: " + objectCount);
        Debug.Log("Número de Vértices: " + vertexCount);
        Debug.Log("Número de Triângulos: " + triangleCount);
    }
}
