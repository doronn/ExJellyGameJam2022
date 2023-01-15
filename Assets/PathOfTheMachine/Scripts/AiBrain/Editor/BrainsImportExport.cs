using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace AiBrain.Editor
{
    public class BrainsImportExport : EditorWindow
    {
        [MenuItem("Tools/Brains/Import And Export brains from Prefs")]
        private static void ShowWindow()
        {
            var window = GetWindow<BrainsImportExport>();
            window.titleContent = new GUIContent("Import/export brains from prefs");
            window.Show();
        }

        private string _brainName = string.Empty;
        private string _brainFilePath = string.Empty;
        private float _enabled;
        private AnimBool _isImport;
        private FadeAreaHelper _importArea = new FadeAreaHelper(true);
        private FadeAreaHelper _exportArea = new FadeAreaHelper(false);

        private void OnEnable()
        {
            _isImport = new AnimBool(true);
            _isImport.valueChanged.AddListener(Repaint);
        }

        private void OnGUI()
        {
            _isImport.target = EditorGUILayout.ToggleLeft("Import", _isImport.target);
            _isImport.target = !EditorGUILayout.ToggleLeft("Export", !_isImport.target);
            EditorGUILayout.Separator();
            _importArea.FadeArea(ImportAreaGUI, _isImport);
            _exportArea.FadeArea(ExportAreaGUI, _isImport);
        }

        private void ImportAreaGUI()
        {
            EditorGUILayout.LabelField("Import to brain name");
            EditorGUILayout.BeginHorizontal();
            _brainFilePath = EditorGUILayout.TextField(_brainFilePath);
            
            if (GUILayout.Button("Find"))
            {
                _brainFilePath = EditorUtility.OpenFilePanel("Find brain file", "", "brain");
                _brainName = Path.GetFileNameWithoutExtension(_brainFilePath);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();

            _brainName = EditorGUILayout.TextField(_brainName);

            if (GUILayout.Button("Import"))
            {
                if (_brainFilePath?.Length > 0)
                {
                    string fileContent = string.Empty;
                    try
                    { 
                        fileContent = File.ReadAllText(_brainFilePath);
                    }
                    catch (Exception e)
                    { 
                        EditorUtility.DisplayDialog("Error loading file",
                            $"There was an issue loading the content of the file in path\n{_brainFilePath}\nSee logs for more information.\nError massage: {e.Message}", "OK");
                        Debug.Log(e);
                        return;
                    }
                    
                    Debug.Log(fileContent);
                    
                    var testDeserialize = string.IsNullOrEmpty(fileContent)
                        ? null
                        : JsonConvert.DeserializeObject<NeuralNetworkArray>(fileContent);
                    
                    if (testDeserialize == null)    
                    {
                        EditorUtility.DisplayDialog("Cannot load brain network",
                            $"Cannot save {_brainName} because the file at {_brainFilePath} is not a valid network", "OK");
                        return;
                    }

                    var shouldWriteBrainToPrefs = !PlayerPrefs.HasKey(_brainName);
                    if (!shouldWriteBrainToPrefs)
                    {
                        shouldWriteBrainToPrefs = EditorUtility.DisplayDialog("Brain already exists",
                            $"A brain with the name {_brainName} is already saved in prefs. Would you like to override it?", "Yes", "No");
                    }

                    if (shouldWriteBrainToPrefs)
                    {
                        PlayerPrefs.SetString(_brainName, fileContent);
                        PlayerPrefs.Save();
                        EditorUtility.DisplayDialog("Imported successfully",
                            $"Brain with name {_brainName} was saved to prefs", "Thanks!");
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("No file chosen",
                        $"Cannot save {_brainName} there was no file chosen", "OK");
                }
            }

        }
        private void ExportAreaGUI()
        {
            EditorGUILayout.LabelField("Export from brain name");
            _brainName = EditorGUILayout.TextField(_brainName);

            if (GUILayout.Button("Export"))
            {
                var savedBrainNetworks = PlayerPrefs.GetString(_brainName);

                var testDeserialize = string.IsNullOrEmpty(savedBrainNetworks)
                    ? null
                    : JsonConvert.DeserializeObject<NeuralNetworkArray>(savedBrainNetworks);

                if (testDeserialize == null)
                {
                    EditorUtility.DisplayDialog("Brain network not found",
                        $"A network for brain name {_brainName} was not found. Result is printed to logs", "OK");
                    Debug.LogError($"Loaded network result was not saved to file:\n{savedBrainNetworks}");
                    return;
                }
                
                _brainFilePath = EditorUtility.SaveFilePanel("Save brain to file", "", _brainName, "brain");
                File.WriteAllText(_brainFilePath, savedBrainNetworks);
            }
        }
    }

    public class FadeAreaHelper
    {
        private bool _wantedTarget;
        public FadeAreaHelper(bool enabled)
        {
            _wantedTarget = enabled;
        }

        public void FadeArea(Action areaContent, AnimBool isEnabled)
        {
            if (EditorGUILayout.BeginFadeGroup(_wantedTarget ? isEnabled.faded : 1 - isEnabled.faded))
            {
                var wasEnabled = GUI.enabled;
                var enabled = isEnabled.target == _wantedTarget;
                GUI.enabled = enabled;
                areaContent?.Invoke();
                GUI.enabled = wasEnabled;
            }

            EditorGUILayout.EndFadeGroup();
        }
    }
}