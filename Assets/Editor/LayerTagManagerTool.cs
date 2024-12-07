using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LayerTagManagerTool : EditorWindow
{
    private string newTag = "";
    private string newLayer = "";
    private int selectedTagIndex = 0;
    private int selectedLayerIndex = 0;

    [MenuItem("Tools/Layer & Tag Manager Tool")]
    public static void ShowWindow()
    {
        GetWindow<LayerTagManagerTool>("Layer & Tag Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gerenciamento de Layers e Tags", EditorStyles.boldLabel);

        newTag = EditorGUILayout.TextField("Nova Tag", newTag);
        if (GUILayout.Button("Adicionar Tag"))
        {
            AddTag(newTag);
        }

        /*if (GUILayout.Button("Aplicar Tag aos Objetos Selecionados"))
        {
            SetTagToSelectedObjects();
        }*/

        newLayer = EditorGUILayout.TextField("Nova Layer", newLayer);
        if (GUILayout.Button("Adicionar Layer"))
        {
            AddLayer(newLayer);
        }
        /*if (GUILayout.Button("Aplicar Layer aos Objetos Selecionados"))
        {
            SetLayerToSelectedObjects();
        }*/

        // Seção de Tags
        GUILayout.Label("Tags", EditorStyles.boldLabel);
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags; // Pega todas as tags existentes
        selectedTagIndex = EditorGUILayout.Popup("Selecionar Tag", selectedTagIndex, tags);

        if (GUILayout.Button("Aplicar Tag aos Objetos Selecionados"))
        {
            SetTagToSelectedObjects(tags[selectedTagIndex]);
        }

        // Seção de Layers
        GUILayout.Label("Layers", EditorStyles.boldLabel);
        string[] layers = UnityEditorInternal.InternalEditorUtility.layers; // Pega todas as layers existentes
        selectedLayerIndex = EditorGUILayout.Popup("Selecionar Layer", selectedLayerIndex, layers);

        if (GUILayout.Button("Aplicar Layer aos Objetos Selecionados"))
        {
            SetLayerToSelectedObjects(layers[selectedLayerIndex]);
        }
        // Seção de Tags
        GUILayout.Label("Seleção por Tag", EditorStyles.boldLabel);
        selectedTagIndex = EditorGUILayout.Popup("Selecionar Tag", selectedTagIndex, tags);

        if (GUILayout.Button("Selecionar Objetos por Tag"))
        {
            SelectObjectsByTag(tags[selectedTagIndex]);
        }

        // Seção de Layers
        GUILayout.Label("Seleção por Layer", EditorStyles.boldLabel);
        selectedLayerIndex = EditorGUILayout.Popup("Selecionar Layer", selectedLayerIndex, layers);

        if (GUILayout.Button("Selecionar Objetos por Layer"))
        {
            SelectObjectsByLayer(layers[selectedLayerIndex]);
        }

    }

    private void AddTag(string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            bool exists = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tag)) { exists = true; break; }
            }

            if (!exists)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
                newTagProp.stringValue = tag;
                tagManager.ApplyModifiedProperties();
                Debug.Log("Tag adicionada: " + tag);
            }
            else
            {
                Debug.LogWarning("A tag já existe: " + tag);
            }
        }
    }

    private void AddLayer(string layer)
    {
        if (!string.IsNullOrEmpty(layer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");

            bool exists = false;
            for (int i = 0; i < layersProp.arraySize; i++)
            {
                SerializedProperty l = layersProp.GetArrayElementAtIndex(i);
                if (l.stringValue.Equals(layer)) { exists = true; break; }
            }

            if (!exists)
            {
                for (int i = 8; i < layersProp.arraySize; i++)
                {
                    SerializedProperty newLayerProp = layersProp.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(newLayerProp.stringValue))
                    {
                        newLayerProp.stringValue = layer;
                        tagManager.ApplyModifiedProperties();
                        Debug.Log("Layer adicionada: " + layer);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("A layer já existe: " + layer);
            }
        }
    }

    private void SetTagToSelectedObjects(string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.tag = tag;
            }
            Debug.Log("Tag '" + tag + "' aplicada aos objetos selecionados.");
        }
        else
        {
            Debug.LogError("Por favor, selecione uma tag antes de aplicar.");
        }
    }

    private void SetLayerToSelectedObjects(string layer)
    {
        int layerIndex = LayerMask.NameToLayer(layer);

        if (layerIndex != -1) // Verifica se a layer existe
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.layer = layerIndex;
            }
            Debug.Log("Layer '" + layer + "' aplicada aos objetos selecionados.");
        }
        else
        {
            Debug.LogError("Layer '" + layer + "' não encontrada.");
        }
    }
    private void SelectObjectsByTag(string tag)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(); // Encontra todos os objetos na cena
        List<GameObject> objectsWithTag = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                objectsWithTag.Add(obj);
            }
        }

        if (objectsWithTag.Count > 0)
        {
            Selection.objects = objectsWithTag.ToArray(); // Seleciona os objetos encontrados
            Debug.Log(objectsWithTag.Count + " objetos com a tag '" + tag + "' selecionados.");
        }
        else
        {
            Debug.LogWarning("Nenhum objeto com a tag '" + tag + "' encontrado.");
        }
    }

    private void SelectObjectsByLayer(string layer)
    {
        int layerIndex = LayerMask.NameToLayer(layer);
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(); // Encontra todos os objetos na cena
        List<GameObject> objectsWithLayer = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layerIndex)
            {
                objectsWithLayer.Add(obj);
            }
        }

        if (objectsWithLayer.Count > 0)
        {
            Selection.objects = objectsWithLayer.ToArray(); // Seleciona os objetos encontrados
            Debug.Log(objectsWithLayer.Count + " objetos na layer '" + layer + "' selecionados.");
        }
        else
        {
            Debug.LogWarning("Nenhum objeto na layer '" + layer + "' encontrado.");
        }
    }
}
