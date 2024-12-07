using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LocalizationTool : EditorWindow
{
    private string newKey = "";
    private string newValue = "";
    private string language = "en";
    private Dictionary<string, string> localizedTexts = new Dictionary<string, string>();

    [MenuItem("Tools/Localization Tool")]
    public static void ShowWindow()
    {
        GetWindow<LocalizationTool>("Localization Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gerenciamento de Localização", EditorStyles.boldLabel);

        newKey = EditorGUILayout.TextField("Chave", newKey);
        newValue = EditorGUILayout.TextField("Valor", newValue);
        language = EditorGUILayout.TextField("Idioma", language);

        if (GUILayout.Button("Adicionar Tradução"))
        {
            AddLocalizationEntry(newKey, newValue, language);
        }

        if (GUILayout.Button("Mostrar Todas as Traduções"))
        {
            ShowAllTranslations();
        }
    }

    private void AddLocalizationEntry(string key, string value, string language)
    {
        string localizedKey = key + "_" + language;
        if (!localizedTexts.ContainsKey(localizedKey))
        {
            localizedTexts.Add(localizedKey, value);
            Debug.Log("Tradução adicionada: " + localizedKey + " -> " + value);
        }
        else
        {
            Debug.LogWarning("A chave já existe para este idioma.");
        }
    }

    private void ShowAllTranslations()
    {
        foreach (var entry in localizedTexts)
        {
            Debug.Log(entry.Key + ": " + entry.Value);
        }
    }
}
