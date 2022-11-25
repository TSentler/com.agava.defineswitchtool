using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

namespace DefineSwitchTool.Editor.Tests
{
    public class DefineSwitcherTests
    {
        [Test]
        public void Can_Switch_To_Vk_Games()
        {
            Can_Switch(CustomDefines.VkGamesName, DefineSwitcher.ToVkGames);
        }
        
        [Test]
        public void Can_Switch_To_Vk_Games_Without_Duplicate()
        {
            Can_Switch_Without_Duplicate(CustomDefines.VkGamesName, DefineSwitcher.ToVkGames);
        }
        
        [Test]
        public void Can_Switch_To_Yandex_Games()
        {
            Can_Switch(CustomDefines.YandexGamesName, DefineSwitcher.ToYandexGames);
        }
        
        [Test]
        public void Can_Switch_To_Yandex_Games_Without_Duplicate()
        {
            Can_Switch_Without_Duplicate(CustomDefines.YandexGamesName, DefineSwitcher.ToYandexGames);
        }

        [Test]
        public void Switch_To_Vk_Remove_Yandex_Symbol_Pass()
        {
            //Assign
            
            //Act
            DefineSwitcher.ToYandexGames();
            DefineSwitcher.ToVkGames();
            
            //Assert
            List<string> allDefines = GetDefineSymbols();
            Assert.False(allDefines.Contains(CustomDefines.YandexGamesName));
        }

        private void Can_Switch(string symbol, Action action)
        {
            //Assign
            
            //Act
            action.Invoke();
        
            //Assert
            List<string> allDefines = GetDefineSymbols();
            Assert.True(allDefines.Contains(symbol));
        }
        
        private void Can_Switch_Without_Duplicate(string symbol, Action action)
        {
            //Assign
            
            //Act
            action.Invoke();
            action.Invoke();
        
            //Assert
            List<string> allDefines = GetDefineSymbols();
            Assert.AreEqual(allDefines.LastIndexOf(symbol),
                allDefines.IndexOf(symbol));
        }

        private List<string> GetDefineSymbols()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);
            return definesString.Split(';').ToList();
        }
    }
}
