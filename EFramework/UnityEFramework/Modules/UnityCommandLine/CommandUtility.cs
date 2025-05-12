using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity.Command
{
    public static class CommandUtility 
    {
        public static List<ScriptableObject> FindSOFiles()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            List<ScriptableObject> soFiles = new List<ScriptableObject>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (so != null)
                {
                    soFiles.Add(so);
                }
            }
            return soFiles;
        }
    }
}
