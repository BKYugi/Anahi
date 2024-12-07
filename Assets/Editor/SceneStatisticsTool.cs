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
        GUILayout.Label("Estat�sticas da Cena", EditorStyles.boldLabel);

        if (GUILayout.Button("Mostrar Estat�sticas"))
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

        Debug.Log("N�mero de Objetos: " + objectCount);
        Debug.Log("N�mero de V�rtices: " + vertexCount);
        Debug.Log("N�mero de Tri�ngulos: " + triangleCount);
    }
}
