using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DefineSwitchTool.Editor
{
    public class CustomDefines
    {
        public static readonly string DefineSymbolsDataPath =
            "Packages/com.agava.defineswitchtool/Editor/"
            + typeof(DefineSymbolsData).Name + ".asset";
        
        private DefineSymbolsData _defineSymbolsData;

        public static string VkGamesName => "VK_GAMES";
        public static string YandexGamesName => "YANDEX_GAMES";
        public static string CrazyGamesName => "CRAZY_GAMES";

        public bool IsInitialized => _defineSymbolsData != null;
        public string SymbolsName => nameof(_defineSymbolsData.Symbols);
        public DefineSymbolsData DefineSymbolsData => _defineSymbolsData;
        
        public CustomDefines()
        {
            _defineSymbolsData = LoadDefineSymbolsData();
            if (_defineSymbolsData == null)
            {
                _defineSymbolsData = CreateDefineSymbolsData();
            }
        }
        
        public IEnumerable<string> ExcludeFrom(IEnumerable<string> defines)
        {
            return defines.Except(_defineSymbolsData.Symbols);
        }
        
        public bool Contains(string symbol)
        {
            return _defineSymbolsData.Symbols.Contains(symbol);
        }
        
        public bool ContainsAll(string[] symbol)
        {
            for (int i = 0; i < symbol.Length; i++)
            {
                if (Contains(symbol[i]) == false)
                {
                    return false;
                } 
            }

            return true;
        }

        private DefineSymbolsData CreateDefineSymbolsData()
        {
            DefineSymbolsData asset =
                ScriptableObject.CreateInstance<DefineSymbolsData>();
            string uniqAssetPath =
                AssetDatabase.GenerateUniqueAssetPath(DefineSymbolsDataPath);
            AssetDatabase.CreateAsset(asset, uniqAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<DefineSymbolsData>(DefineSymbolsDataPath);
        }

        private DefineSymbolsData LoadDefineSymbolsData()
        {
            return AssetDatabase.LoadAssetAtPath<DefineSymbolsData>(
                DefineSymbolsDataPath);
        }
    }
}