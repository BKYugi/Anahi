using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolingTool : EditorWindow
{
    public GameObject objectToPool;
    public int poolSize = 10;

    [MenuItem("Tools/Object Pooling Tool")]
    public static void ShowWindow()
    {
        GetWindow<ObjectPoolingTool>("Object Pooling");
    }

    private void OnGUI()
    {
        GUILayout.Label("Criar um Pool de Objetos", EditorStyles.boldLabel);
        objectToPool = (GameObject)EditorGUILayout.ObjectField("Objeto para Pool", objectToPool, typeof(GameObject), false);
        poolSize = EditorGUILayout.IntField("Tamanho do Pool", poolSize);

        if (GUILayout.Button("Criar Pool"))
        {
            CreatePool();
        }
    }

    private void CreatePool()
    {
        if (objectToPool == null)
        {
            Debug.LogError("Por favor, selecione um objeto para o pool.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject pooledObject = (GameObject)PrefabUtility.InstantiatePrefab(objectToPool);
            pooledObject.SetActive(false);
        }
        Debug.Log(poolSize + " objetos criados para o pool.");
    }
}
