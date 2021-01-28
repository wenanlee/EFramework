// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2017 Mopsicus <immops@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.IO;
using EFramework.Common;
using EFramework.EditorHelpers;
using EFramework.Serialization;
using UnityEditor;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {

    /// <summary>
    /// Buildmanager for assets bundle
    /// </summary>
    sealed class CreateAssetsBundle : EditorWindow {

        public class BuildSettings {
            [JsonName ("o")]
            public BuildAssetBundleOptions Options = BuildAssetBundleOptions.UncompressedAssetBundle;
            [JsonName ("p")]
            public string Path = "AssetBundles";
            [JsonName ("t")]
            public BuildTarget Target = BuildTarget.iOS;
        }
        const string Title = "Asset bundles";
        const string SettingsKey = "lg.build-bundle";
        const string DefaultPath = "AssetBundles";
        BuildSettings _settings;

        [MenuItem ("Window/LeopotamGroupLibrary/Build assets bundle...")]
        public static void OpenEditorWindow () {
            var win = EditorWindow.GetWindow (typeof (CreateAssetsBundle));
            var pos = win.position;
            pos.width = 500f;
            pos.height = 250f;
            win.position = pos;
        }

        void OnEnable () {
            titleContent.text = Title;
            _settings = null;
            try {
                _settings = Service<JsonSerialization>.Get ().Deserialize<BuildSettings> (ProjectPrefs.GetString (SettingsKey));
            } catch { }
            if (_settings == null) {
                _settings = new BuildSettings ();
            }
        }

        void OnGUI () {
            GUILayout.Label ("AssetBundles Settings", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal (GUI.skin.box);
            GUILayout.BeginVertical ();

            GUILayout.BeginHorizontal ();
            _settings.Path = EditorGUILayout.TextField ("Output Path:", _settings.Path).Trim ();
            if (string.IsNullOrEmpty (_settings.Path)) {
                _settings.Path = DefaultPath;
            }
            GUILayout.EndHorizontal ();

            GUILayout.BeginHorizontal ();
            EditorGUILayout.LabelField ("Options:", EditorStyles.label, GUILayout.Width (EditorGUIUtility.labelWidth));
            _settings.Options = (BuildAssetBundleOptions) EditorGUILayout.EnumPopup (_settings.Options);
            GUILayout.EndHorizontal ();

            GUILayout.BeginHorizontal ();
            EditorGUILayout.LabelField ("Platform:", EditorStyles.label, GUILayout.Width (EditorGUIUtility.labelWidth));
            _settings.Target = (BuildTarget) EditorGUILayout.EnumPopup (_settings.Target);
            GUILayout.EndHorizontal ();

            GUILayout.EndVertical ();
            GUILayout.EndHorizontal ();

            GUILayout.Space (10f);

            EditorGUILayout.HelpBox ("Note: if you will load bundle directly from disk – choose option \"UncompressedAssetBundle\", to pack bundle and download from remote source – choose \"None\".", MessageType.Info, true);

            GUILayout.Space (10f);

            EditorGUILayout.Separator ();
            if (GUILayout.Button ("Save settings & build", GUILayout.Height (50f))) {
                ProjectPrefs.SetString (SettingsKey, Service<JsonSerialization>.Get ().Serialize (_settings));
                var res = Generate (_settings);
                EditorUtility.DisplayDialog (titleContent.text, res ?? "Success", "Close");
                if (string.IsNullOrEmpty (res)) {
                    EditorUtility.RevealInFinder (_settings.Path);
                }
            }
        }

        /// <summary>
        /// Build assets bundle with options
        /// </summary>
        /// <returns>Error message or null on success.</returns>
        /// <param name="settings">Generation settings.</param>
        public static string Generate (BuildSettings settings) {
            if (settings == null || string.IsNullOrEmpty (settings.Path)) {
                return "invalid parameters";
            }
            try {
                if (!Directory.Exists (settings.Path)) {
                    Directory.CreateDirectory (settings.Path);
                }
                BuildPipeline.BuildAssetBundles (settings.Path, settings.Options, settings.Target);
                return null;
            } catch (Exception ex) {
                return ex.Message;
            }
        }

    }
}
