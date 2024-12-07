using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SnapshotTool : EditorWindow
{
    private Dictionary<string, List<TransformSnapshot>> snapshots = new Dictionary<string, List<TransformSnapshot>>();
    private string snapshotName = "Snapshot 1";
    private string selectedSnapshot;

    [MenuItem("Tools/Snapshot Tool")]
    public static void ShowWindow()
    {
        GetWindow<SnapshotTool>("Snapshot Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gerenciar Snapshots", EditorStyles.boldLabel);

        snapshotName = EditorGUILayout.TextField("Nome do Snapshot", snapshotName);

        if (GUILayout.Button("Salvar Snapshot"))
        {
            SaveSnapshot();
        }

        if (snapshots.Count > 0)
        {
            GUILayout.Label("Restaurar Snapshot", EditorStyles.boldLabel);

            string[] snapshotKeys = new string[snapshots.Keys.Count];
            snapshots.Keys.CopyTo(snapshotKeys, 0);
            int index = System.Array.IndexOf(snapshotKeys, selectedSnapshot);
            index = EditorGUILayout.Popup("Selecionar Snapshot", index, snapshotKeys);

            if (index >= 0)
            {
                selectedSnapshot = snapshotKeys[index];
            }

            if (GUILayout.Button("Restaurar Snapshot") && selectedSnapshot != null)
            {
                RestoreSnapshot(selectedSnapshot);
            }
        }
    }

    private void SaveSnapshot()
    {
        List<TransformSnapshot> newSnapshots = new List<TransformSnapshot>();
        foreach (GameObject obj in Selection.gameObjects)
        {
            newSnapshots.Add(new TransformSnapshot(obj.transform));
        }

        if (snapshots.ContainsKey(snapshotName))
        {
            snapshots[snapshotName] = newSnapshots;
            Debug.Log("Snapshot '" + snapshotName + "' atualizado.");
        }
        else
        {
            snapshots.Add(snapshotName, newSnapshots);
            Debug.Log("Snapshot '" + snapshotName + "' salvo.");
        }
    }

    private void RestoreSnapshot(string snapshotName)
    {
        if (snapshots.ContainsKey(snapshotName))
        {
            foreach (TransformSnapshot snapshot in snapshots[snapshotName])
            {
                snapshot.Restore();
            }
            Debug.Log("Snapshot '" + snapshotName + "' restaurado.");
        }
        else
        {
            Debug.LogError("Snapshot '" + snapshotName + "' não encontrado.");
        }
    }
}

public class TransformSnapshot
{
    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;
    private Transform transform;

    public TransformSnapshot(Transform transform)
    {
        this.transform = transform;
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    public void Restore()
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
}
