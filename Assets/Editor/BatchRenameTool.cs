using UnityEditor;
using UnityEngine;

public class BatchRenameTool : EditorWindow
{
    private string baseName = "Object";
    private int startIndex = 0;

    [MenuItem("Tools/Batch Rename Tool")]
    public static void ShowWindow()
    {
        GetWindow<BatchRenameTool>("Batch Rename");
    }

    private void OnGUI()
    {
        GUILayout.Label("Renomear Objetos em Massa", EditorStyles.boldLabel);
        baseName = EditorGUILayout.TextField("Nome Base", baseName);
        startIndex = EditorGUILayout.IntField("Índice Inicial", startIndex);

        if (GUILayout.Button("Renomear Objetos Selecionados"))
        {
            RenameObjects();
        }
    }

    private void RenameObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].name = baseName + (startIndex + i);
        }
        Debug.Log("Objetos renomeados.");
    }
}
