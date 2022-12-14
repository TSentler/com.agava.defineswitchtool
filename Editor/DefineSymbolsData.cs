using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefineSwitchTool.Editor
{
    public class DefineSymbolsData : ScriptableObject
    {
        public List<string> Symbols = new List<string>
        {
            "VK_GAMES",
            "VK_GAMES_MOBILE",
            "YANDEX_GAMES",
            "CRAZY_GAMES",
        };
    }
}
