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
            _customDefines = new CustomDefines();

            if (_customDefines.IsInitialized)
            {
                //Selection.activeObject = _customDefines.DefineSymbolsData;
                _checkedSymbols = DefineSwitcher.GetCurrentDefineSymbols();
            }
            else
            {
                Debug.LogError("Can`t find " 
                               + CustomDefines.DefineSymbolsDataPath);
                Close();
            }
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(_customDefines.DefineSymbolsData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
                DefineSymbols[i] = GUILayout.TextField(DefineSymbols[i]);
            }
            
            foreach (var symbol in _customDefines.DefineSymbolsData.Symbols)
            {
                var isChecked = _checkedSymbols.Contains(symbol);
                var isCheckedNow = EditorGUILayout.Toggle(symbol, isChecked);
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

            if (GUILayout.Button("Apply"))
            {
                DefineSwitcher.AddDefineSymbols(_checkedSymbols.ToArray(), _customDefines);
            }
        }
    }
}