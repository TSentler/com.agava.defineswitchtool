using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DefineSwitchTool.Editor
{
    public class DefineSwitcher : MonoBehaviour
    {
        private static readonly BuildTargetGroup _buildTargetGroup =
            EditorUserBuildSettings.selectedBuildTargetGroup;
        
        [MenuItem("Tools/DefineSwitcher/ToVkGames")]
        public static void ToVkGames()
        {
            AddDefineSymbols(new []{CustomDefines.VkGamesName},
                new CustomDefines());
        }

        [MenuItem("Tools/DefineSwitcher/ToYandexGames")]
        public static void ToYandexGames()
        {
            AddDefineSymbols(new []{CustomDefines.YandexGamesName},
                new CustomDefines());
        }

        [MenuItem("Tools/DefineSwitcher/ToCrazyGames")]
        public static void ToCrazyGames()
        {
            AddDefineSymbols(new []{CustomDefines.CrazyGamesName},
                new CustomDefines());
        }
        
        public static List<string> GetCurrentDefineSymbols()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(_buildTargetGroup);
            return definesString.Split(';').ToList();
        }

        public static void AddDefineSymbols(string[] symbols, CustomDefines customDefines)
        {
            if (CustomDefinesContainsSymbols(symbols, customDefines) == false)
                return;
            
            var currentDefinesWithoutCustom = 
                customDefines.ExcludeFrom(GetCurrentDefineSymbols()).ToList();
            var newDefines = new List<string>();
            newDefines.AddRange(currentDefinesWithoutCustom);
            newDefines.AddRange(symbols.Except(currentDefinesWithoutCustom));
            SetDefineSymbols(newDefines.ToArray());

            var symbolsString = string.Join(" ", symbols);
            Debug.Log($"Switch to {symbolsString}!");
        }

        private static void SetDefineSymbols(string[] allDefinesWithoutCustom)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                _buildTargetGroup, string.Join(";", allDefinesWithoutCustom));
        }

        private static bool CustomDefinesContainsSymbols(string[] symbols, CustomDefines customDefines)
        {
            if (customDefines.ContainsAll(symbols))
                return true;
        
            Debug.LogError("Define symbol not found in CustomDefines");
            return false;
        }

    }
}
