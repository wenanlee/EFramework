using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.UnityCommandLine
{
    public class BuiltinCommands
    {
        [RegisterCommandLine(Name = "Help", Help = "print all command")]
        public static void HelpCommand()
        {
            foreach (var item in UnityCommandLineManager.Instance.commandDict)
            {
                string space = string.Empty;
                for (int i = 0; i < 4 - (item.Key.Length / 8); i++)
                {
                    space += "\t";
                }

                //Debuger.Log($"> {item.Key}{space}:   {item.Value._Attribute.Help}");
            }
        }
    }
}

