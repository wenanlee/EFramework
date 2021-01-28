// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EFramework.Serialization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    /// <summary>
    /// Unity idents generator.
    /// </summary>
    class UnityIdentsGenerator : EditorWindow {
        [Flags]
        public enum Options {
            Layers = 1,
            Tags = 2,
            Scenes = 4,
            Animators = 8,
            Axes = 16,
            Shaders = 32,
            SortingLayers = 64
        }

        const string Title = "Unity idents generator";

        const string SettingsKey = "lg.unity-idents";

        const string DefaultFilename = "Client/Scripts/Common/UnityIdents.cs";

        const string DefaultNamespace = "Client.Common";

        const string DefaultIgnoredPaths = "";

        const Options DefaultOptions = (Options) (-1);

        public class GenerationSettings {
            [JsonName ("o")]
            public Options Options = DefaultOptions;
            [JsonName ("f")]
            public string Filename = DefaultFilename;
            [JsonName ("n")]
            public string Namespace = DefaultNamespace;
            [JsonName ("is")]
            public string IgnoredPaths = DefaultIgnoredPaths;
        }

        const string CodeTemplate =
            "// Auto generated code, dont change it manually!\n\n" +
            "using UnityEngine;\n\nnamespace {0} {{\n\tpublic static partial class {1} {{\n{2}\n\t}}\n}}";

        const string LayerName = "{0}public static readonly int Layer{1} = LayerMask.NameToLayer (\"{2}\");";

        const string LayerMask = "{0}public static readonly int MaskLayer{1} = 1 << Layer{1};";

        const string TagName = "{0}public const string Tag{1} = \"{2}\";";

        const string SceneName = "{0}public const string Scene{1} = \"{2}\";";

        const string AnimatorName = "{0}public static readonly int Animator{1} = Animator.StringToHash (\"{2}\");";

        const string AxisName = "{0}public const string Axis{1} = \"{2}\";";

        const string ShaderName = "{0}public static readonly int Shader{1} = Shader.PropertyToID (\"{2}\");";

        const string SortingLayerName = "{0}public const int SortingLayer{1} = {2};";

        GenerationSettings _settings;

        string[] _optionNames;

        readonly JsonSerialization _serializer = new JsonSerialization ();

        [MenuItem ("Window/LeopotamGroupLibrary/UnityIdents generator...")]
        static void InitGeneration () {
            GetWindow<UnityIdentsGenerator> (true);
        }

        void OnEnable () {
            titleContent.text = Title;
            _settings = null;
            try {
                _settings = _serializer.Deserialize<GenerationSettings> (ProjectPrefs.GetString (SettingsKey));
            } catch { }
            if (_settings == null) {
                _settings = new GenerationSettings ();
            }
        }

        void OnGUI () {
            if (_optionNames == null) {
                _optionNames = Enum.GetNames (typeof (Options));
            }

            _settings.Filename = EditorGUILayout.TextField ("Target file", _settings.Filename).Trim ();
            if (string.IsNullOrEmpty (_settings.Filename)) {
                _settings.Filename = DefaultFilename;
            }
            _settings.Namespace = EditorGUILayout.TextField ("Namespace", _settings.Namespace).Trim ();
            if (string.IsNullOrEmpty (_settings.Namespace)) {
                _settings.Namespace = DefaultNamespace;
            }
            _settings.Options = (Options) EditorGUILayout.MaskField ("Options", (int) _settings.Options, _optionNames);

            _settings.IgnoredPaths = EditorGUILayout.TextField ("Ignore assets at paths", _settings.IgnoredPaths).Trim ();

            if (GUILayout.Button ("Reset settings")) {
                ProjectPrefs.DeleteKey (SettingsKey);
                OnEnable ();
                Repaint ();
            }
            if (GUILayout.Button ("Save settings & generate")) {
                ProjectPrefs.SetString (SettingsKey, _serializer.Serialize (_settings));
                var res = Generate (_settings);
                EditorUtility.DisplayDialog (titleContent.text, res ?? "Success", "Close");
            }
        }

        static bool ShouldBeIgnored (string assetPath, string[] ignoredPaths) {
            if (string.IsNullOrEmpty (assetPath)) {
                return true;
            }
            if (ignoredPaths == null || ignoredPaths.Length == 0) {
                return false;
            }
            assetPath = assetPath.Substring (assetPath.IndexOf ('/') + 1);
            var i = ignoredPaths.Length - 1;
            for (; i >= 0; i--) {
                if (assetPath.StartsWith (ignoredPaths[i])) {
                    break;
                }
            }
            return i != -1;
        }

        static string GenerateFields (string indent, GenerationSettings settings) {
            var lines = new List<string> (128);
            var uniquesList = new HashSet<string> ();
            var options = settings.Options;

            var ignoredPaths = string.IsNullOrEmpty (settings.IgnoredPaths) ?
                new string[0] : settings.IgnoredPaths.Split (new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // layers, layer masks
            if ((int) (options & Options.Layers) != 0) {
                foreach (var layerName in InternalEditorUtility.layers) {
                    lines.Add (string.Format (LayerName, indent, CleanupName (layerName), CleanupValue (layerName)));
                    lines.Add (string.Format (LayerMask, indent, CleanupName (layerName)));
                }
            }

            // tags
            if ((int) (options & Options.Tags) != 0) {
                foreach (var tagName in InternalEditorUtility.tags) {
                    lines.Add (string.Format (TagName, indent, CleanupName (tagName), CleanupValue (tagName)));
                }
            }

            // scenes
            if ((int) (options & Options.Scenes) != 0) {
                foreach (var scene in EditorBuildSettings.scenes) {
                    if (!ShouldBeIgnored (scene.path, ignoredPaths)) {
                        var sceneName = Path.GetFileNameWithoutExtension (scene.path);
                        lines.Add (string.Format (SceneName, indent, CleanupName (sceneName), CleanupValue (sceneName)));
                    }
                }
            }

            // animators
            if ((int) (options & Options.Animators) != 0) {
                foreach (var guid in AssetDatabase.FindAssets ("t:animatorcontroller")) {
                    var assetPath = AssetDatabase.GUIDToAssetPath (guid);
                    if (!ShouldBeIgnored (assetPath, ignoredPaths)) {
                        var ac = AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController> (assetPath);
                        for (int i = 0, iMax = ac.parameters.Length; i < iMax; i++) {
                            var name = ac.parameters[i].name.Trim ();
                            if (!uniquesList.Contains (name)) {
                                lines.Add (string.Format (AnimatorName, indent, CleanupName (name), CleanupValue (name)));
                                uniquesList.Add (name);
                            }
                        }
                    }
                }
                uniquesList.Clear ();
            }

            // axes
            if ((int) (options & Options.Axes) != 0) {
                var inputManager = AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/InputManager.asset") [0];
                var axes = new SerializedObject (inputManager).FindProperty ("m_Axes");
                for (int i = 0, iMax = axes.arraySize; i < iMax; i++) {
                    var axis = axes.GetArrayElementAtIndex (i).FindPropertyRelative ("m_Name").stringValue;
                    if (!uniquesList.Contains (axis)) {
                        lines.Add (string.Format (AxisName, indent, CleanupName (axis), CleanupValue (axis)));
                        uniquesList.Add (axis);
                    }
                }
                uniquesList.Clear ();
            }

            // shaders
            if ((int) (options & Options.Shaders) != 0) {
                foreach (var guid in AssetDatabase.FindAssets ("t:shader")) {
                    var assetPath = AssetDatabase.GUIDToAssetPath (guid);
                    if (!ShouldBeIgnored (assetPath, ignoredPaths)) {
                        var shader = AssetDatabase.LoadAssetAtPath<Shader> (assetPath);
                        if (shader.name.IndexOf ("Hidden", StringComparison.Ordinal) != 0) {
                            for (int i = 0, iMax = ShaderUtil.GetPropertyCount (shader); i < iMax; i++) {
                                if (!ShaderUtil.IsShaderPropertyHidden (shader, i)) {
                                    var name = ShaderUtil.GetPropertyName (shader, i);
                                    if (!uniquesList.Contains (name)) {
                                        lines.Add (string.Format (ShaderName, indent, CleanupName (name), CleanupValue (name)));
                                        uniquesList.Add (name);
                                    }
                                }
                            }
                        }
                    }
                }
                uniquesList.Clear ();
            }

            // sorting layers
            if ((int) (options & Options.SortingLayers) != 0) {
                foreach (var sortLayer in SortingLayer.layers) {
                    lines.Add (string.Format (SortingLayerName, indent, CleanupName (sortLayer.name), sortLayer.id));
                }
            }

            lines.Sort ();
            return string.Join ("\n\n", lines.ToArray ());
        }

        static string CleanupName (string dirtyName) {
            // cant use "CultureInfo.InvariantCulture.TextInfo.ToTitleCase" due it will break already upcased chars.
            var sb = new StringBuilder ();
            var needUp = true;
            foreach (var c in dirtyName) {
                if (char.IsLetterOrDigit (c)) {
                    sb.Append (needUp ? char.ToUpperInvariant (c) : c);
                    needUp = false;
                } else {
                    needUp = true;
                }
            }
            return sb.ToString ();
        }

        static string CleanupValue (string dirtyValue) {
            return dirtyValue.Replace ("\"", "\\\"");
        }

        /// <summary>
        /// Generate class with idents at specified filename and with specified namespace.
        /// </summary>
        /// <returns>Error message or null on success.</returns>
        /// <param name="settings">Generation settins.</param>
        public static string Generate (GenerationSettings settings) {
            if (settings == null || string.IsNullOrEmpty (settings.Filename) || string.IsNullOrEmpty (settings.Namespace)) {
                return "invalid parameters";
            }
            var fullFilename = Path.Combine (Application.dataPath, settings.Filename);
            var className = Path.GetFileNameWithoutExtension (fullFilename);
            try {
                var path = Path.GetDirectoryName (fullFilename);
                if (!Directory.Exists (path)) {
                    Directory.CreateDirectory (path);
                }
                var fields = GenerateFields (new string ('\t', 2), settings);
                var content = string.Format (CodeTemplate, settings.Namespace, className, fields);
                File.WriteAllText (fullFilename, content.Replace ("\t", new string (' ', 4)));
                AssetDatabase.Refresh ();
                return null;
            } catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}