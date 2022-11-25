using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

namespace DefineSwitchTool.Editor.Tests
{
    public class CustomDefinesTests
    {
        [Test]
        public void Can_Access_Scriptable_Object()
        {
            //Assign
            var customDefines = new CustomDefines();
            
            //Act
        
            //Assert
            Assert.True(customDefines.IsInitialized);
        }
    }
}
