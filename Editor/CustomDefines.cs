using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DefineSwitchTool.Editor
{
    public class CustomDefines
    {
        public CustomDefines()
        {
            _defineSymbolsData = LoadDefineSymbolsData();
            if (_defineSymbolsData == null)
            {
                _defineSymbolsData = CreateDefineSymbolsData();
            }
        }
        
        private const string _assetsFolder = "Assets";
        private const string _editorFolder = "Editor";
        private const string _defineSwitchToolFolder = "DefineSwitchTool";

        public static readonly string AssetName = typeof(DefineSymbolsData).Name; 
        public static readonly string FileName = AssetName + ".asset";
        public static readonly string WorkFolderPath =
            _defineSwitchToolFolder + "/" + _editorFolder;
        public static readonly string WorkFilePath =
            WorkFolderPath + "/" + FileName;
        public static readonly string AssetFolderPath =
            _assetsFolder + "/" + WorkFolderPath;
        public static readonly string AssetFilePath =
            AssetFolderPath + "/" + FileName;
        
        private static readonly string _fullFilePath = Application.dataPath + "/" + WorkFilePath;
                
        private DefineSymbolsData _defineSymbolsData;

        public static string VkGamesName => "VK_GAMES";
        public static string YandexGamesName => "YANDEX_GAMES";
        public static string CrazyGamesName => "CRAZY_GAMES";

        public bool IsInitialized => _defineSymbolsData != null;
        public DefineSymbolsData DefineSymbolsData => _defineSymbolsData;
        
        public void Save()
        {
            if (IsInitialized)
            {
                EditorUtility.SetDirty(DefineSymbolsData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
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
            Debug.Log("Try create " + AssetName);
            var assets = AssetDatabase.FindAssets(AssetName, 
                new []{AssetFolderPath});
            if (assets.Length > 0)
            {
                Debug.Log("File already exists " + assets[0]);
                AssetDatabase.Refresh();
                return LoadDefineSymbolsData();
            }
            
            if (System.IO.File.Exists(_fullFilePath))
            {
                Debug.Log("File already exists " + _fullFilePath);
                return null;
            }
            
            CreateFolder();
            DefineSymbolsData asset =
                ScriptableObject.CreateInstance<DefineSymbolsData>();
            AssetDatabase.CreateAsset(asset, AssetFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<DefineSymbolsData>(AssetFilePath);
        }

        private static void CreateFolder()
        {
            var path = _assetsFolder;
            var folder = _defineSwitchToolFolder;
            CreateFolder(path, folder);

            path += "/" +folder;
            folder = _editorFolder;
            CreateFolder(path, folder);
        }

        private static void CreateFolder(string path, string folder)
        {
            if (AssetDatabase.IsValidFolder(path + "/" + folder) == false)
            {
                AssetDatabase.CreateFolder(path, folder);
            }
        }

        private DefineSymbolsData LoadDefineSymbolsData()
        {
            var defineSymbolsData = AssetDatabase.
                LoadAssetAtPath<DefineSymbolsData>(AssetFilePath);
            if (defineSymbolsData == null)
            {
                Debug.Log("Cant load " + AssetFilePath);
            }
            return defineSymbolsData;
        }
    }
}