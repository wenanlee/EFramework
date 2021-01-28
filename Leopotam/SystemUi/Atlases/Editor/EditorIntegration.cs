// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EFramework.SystemUi.Atlases.UnityEditors {
    static class EditorIntegration {
        const string Title = "SpriteAtlas packer";

        [MenuItem ("Assets/LeopotamGroup/SystemUi/Atlases/Create new atlas", false, 1)]
        static void CreateAtlas () {
            var path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (!string.IsNullOrEmpty (path) && AssetDatabase.Contains (Selection.activeObject)) {
                if (!AssetDatabase.IsValidFolder (path)) {
                    path = Path.GetDirectoryName (path);
                }
            } else {
                path = "Assets";
            }

            var asset = new GameObject ();
            asset.AddComponent<SpriteAtlas> ();
            PrefabUtility.CreatePrefab (
                AssetDatabase.GenerateUniqueAssetPath (string.Format ("{0}/{1}.prefab", path, "SpriteAtlas")), asset);
            Object.DestroyImmediate (asset);
            AssetDatabase.Refresh ();
        }

        [MenuItem ("Assets/LeopotamGroup/SystemUi/Atlases/Rebuild all atlases", false, 2)]
        static void RebuildAtlases () {
            var sprites = AssetDatabase.FindAssets ("t:sprite");
            var spriteList = new Dictionary<string, List<Sprite>> ();
            string assetPath;
            string tag;
            TextureImporter importer;
            for (int i = sprites.Length - 1; i >= 0; i--) {
                assetPath = AssetDatabase.GUIDToAssetPath (sprites[i]);
                importer = (TextureImporter) AssetImporter.GetAtPath (AssetDatabase.GUIDToAssetPath (sprites[i]));
                tag = importer.spritePackingTag;
                if (!string.IsNullOrEmpty (tag)) {
                    if (!spriteList.ContainsKey (tag)) {
                        spriteList.Add (tag, new List<Sprite> ());
                    }
                    spriteList[tag].Add (AssetDatabase.LoadAssetAtPath<Sprite> (assetPath));
                }
            }
            if (spriteList.Count == 0) {
                EditorUtility.DisplayDialog ("SpriteAtlas packer", "No sprites with atlas tag", "Close");
                return;
            }

            var prefabs = AssetDatabase.FindAssets ("t:prefab");
            foreach (var item in prefabs) {
                var path = AssetDatabase.GUIDToAssetPath (item);
                var asset = AssetDatabase.LoadAssetAtPath<SpriteAtlas> (path);
                if (asset != null) {
                    tag = asset.GetName ();
                    if (!string.IsNullOrEmpty (tag)) {
                        asset.SetSprites (spriteList.ContainsKey (tag) ? spriteList[tag] : null);
                        EditorUtility.SetDirty (asset);
                    } else {
                        Debug.LogWarningFormat ("SpriteAtlas.Name at \"{0}\" is empty - skipped", path);
                    }
                }
            }
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh ();

            EditorUtility.DisplayDialog (Title, "Success", "Close");
        }
    }
}