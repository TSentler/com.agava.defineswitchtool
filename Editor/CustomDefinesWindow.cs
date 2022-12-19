using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefineSwitchTool.Editor
{
    public class CustomDefinesWindow : EditorWindow
    {
        private CustomDefines _customDefines;
        private List<string> _checkedSymbols;

        private List<string> DefineSymbols => _customDefines.DefineSymbolsData.Symbols;
        
        private void OnEnable()
        {
            if (_customDefines == null)
                _customDefines = new CustomDefines();

            if (_customDefines.IsInitialized)
            {
                _checkedSymbols = DefineSwitcher.GetCurrentDefineSymbols();
            }
            else
            {
                Debug.LogError("Can`t find " + CustomDefines.AssetFilePath);
                Close();
            }
        }

        private void OnDestroy()
        {
            _customDefines.Save();
        }
        
        [MenuItem("Tools/DefineSwitcher/CustomDefines")]
        public static void ShowWindow()
        {
            GetWindow<CustomDefinesWindow>(nameof(CustomDefinesWindow));
        }

        private void OnGUI()
        {
            GUILayout.Label("DefineSymbols", EditorStyles.boldLabel);
            int newCount = Mathf.Max(0, EditorGUILayout.IntField("size", DefineSymbols.Count));
            while (newCount < DefineSymbols.Count)
                DefineSymbols.RemoveAt( DefineSymbols.Count - 1 );
            while (newCount > DefineSymbols.Count)
                DefineSymbols.Add("");
 
            for(int i = 0; i < DefineSymbols.Count; i++)
            {
                var symbol = DefineSymbols[i];
                var isChecked = _checkedSymbols.Contains(symbol);
                EditorGUILayout.BeginHorizontal();
                var isCheckedNow = EditorGUILayout.Toggle("", isChecked, 
                    GUILayout.MaxWidth(15f));
                DefineSymbols[i] = GUILayout.TextField(DefineSymbols[i]);
                EditorGUILayout.EndHorizontal();
                if (isChecked && isCheckedNow == false)
                {
                    _checkedSymbols.Remove(symbol);
                }
                else if (isChecked == false && isCheckedNow)
                {
                    _checkedSymbols.Add(symbol);
                }
            }

            _checkedSymbols.RemoveAll(
                symbol => DefineSymbols.Contains(symbol) == false);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(10f);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Apply"))
            {
                _customDefines.Save();
                DefineSwitcher.AddDefineSymbols(_checkedSymbols.ToArray(), _customDefines);
            }
        }
    }
}