// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;

namespace EFramework.SystemUi.Markup.UnityEditors {
    static class EditorIntegration {
        [MenuItem ("Assets/LeopotamGroup/SystemUi/Markup/Create new theme", false, 150)]
        static void CreateTheme () {
            var path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (!string.IsNullOrEmpty (path) && AssetDatabase.Contains (Selection.activeObject)) {
                if (!AssetDatabase.IsValidFolder (path)) {
                    path = Path.GetDirectoryName (path);
                }
            } else {
                path = "Assets";
            }

            var asset = ScriptableObject.CreateInstance<MarkupTheme> ();
            AssetDatabase.CreateAsset (
                asset, AssetDatabase.GenerateUniqueAssetPath (string.Format ("{0}/{1}.asset", path, "MarkupTheme")));
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh ();
        }
    }
}