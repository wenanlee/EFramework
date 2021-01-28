// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    /// <summary>
    /// Project folders generator.
    /// </summary>
    sealed class ProjectFoldersGenerator : EditorWindow {
        [Flags]
        public enum Options {
            Animations = 1,

            Fonts = 2,

            Images = 4,

            Models = 8,

            Plugins = 16,

            Prefabs = 32,

            Resources = 64,

            Scenes = 128,

            Scripts = 256,

            Shaders = 512,

            Sounds = 1024,

            StreamingAssets = 2048,
        }

        static readonly Dictionary<int, List<string>> DefinedPaths = new Dictionary<int, List<string>> {
            {
            (int) Options.Animations, new List<string> { "Sources", "Controllers" }
            },
            {
            (int) Options.Images,
            new List<string> { "AppIcon", "Ui", "Ui/Sources" }
            }
        };

        const string Title = "Project folders generator";

        const Options DefaultOptions = (Options) (-1);

        const Options RootOnlyOptions = Options.Plugins;

        const string DefaultRootProjectFolder = "Client";

        const string DefaultCvsFileName = "RemoveMe.txt";

        string _projectRootFolder;

        Options _options;

        bool _cvsSupport;

        string _cvsFileName;

        string[] _optionNames;

        [MenuItem ("Window/LeopotamGroupLibrary/Project folders generator...")]
        static void InitGeneration () {
            var win = GetWindow<ProjectFoldersGenerator> (true);
            win.Reset ();
        }

        void Awake () {
            Reset ();
        }

        void OnEnable () {
            titleContent.text = Title;
        }

        void Reset () {
            _projectRootFolder = DefaultRootProjectFolder;
            _options = DefaultOptions;
            _cvsSupport = true;
            _cvsFileName = DefaultCvsFileName;
        }

        void OnGUI () {
            if (_optionNames == null) {
                _optionNames = Enum.GetNames (typeof (Options));
            }

            _projectRootFolder = EditorGUILayout.TextField ("Project root folder", _projectRootFolder).Trim ();

            _options = (Options) EditorGUILayout.MaskField ("Options", (int) _options, _optionNames);

            var cvsSupport = EditorGUILayout.Toggle ("Cvs support", _cvsSupport);
            if (cvsSupport != _cvsSupport) {
                _cvsSupport = cvsSupport;
                _cvsFileName = DefaultCvsFileName;
            }

            if (_cvsSupport) {
                _cvsFileName = EditorGUILayout.TextField ("Cvs filename", _cvsFileName).Trim ();
            }

            if (GUILayout.Button ("Reset settings")) {
                Reset ();
                Repaint ();
            }
            if (GUILayout.Button ("Generate")) {
                if (string.IsNullOrEmpty (_cvsFileName)) {
                    _cvsSupport = false;
                }
                var res = Generate (_projectRootFolder, _options, _cvsSupport ? _cvsFileName : null);
                EditorUtility.DisplayDialog (titleContent.text, res ?? "Success", "Close");
            }
        }

        static void GenerateCvsSupport (string path, string cvsFileName) {
            if (!string.IsNullOrEmpty (cvsFileName)) {
                path = Path.Combine (path, cvsFileName);
                if (!File.Exists (path)) {
                    File.WriteAllText (path, string.Empty);
                }
            }
        }

        static void GenerateItem (string rootFolder, int item, string cvsFileName) {
            var fullPath = (((int) RootOnlyOptions) & item) != 0 ?
                Application.dataPath :
                Path.Combine (Application.dataPath, rootFolder);

            fullPath = Path.Combine (fullPath, ((Options) item).ToString ());
            if (!Directory.Exists (fullPath)) {
                Directory.CreateDirectory (fullPath);
            }

            if (DefinedPaths.ContainsKey (item)) {
                string path;
                foreach (var subFolder in DefinedPaths[item]) {
                    path = Path.Combine (fullPath, subFolder);
                    if (!Directory.Exists (path)) {
                        Directory.CreateDirectory (path);
                    }
                    GenerateCvsSupport (path, cvsFileName);
                }
            } else {
                GenerateCvsSupport (fullPath, cvsFileName);
            }
        }

        /// <summary>
        /// Generate class with idents at specified filename and with specified namespace.
        /// </summary>
        /// <returns>Error message or null on success.</returns>
        /// <param name="rootFolder">Root folder path or empty/null for disable.</param>
        /// <param name="options">Options</param>
        /// <param name="cvsFileName">Cvs filename for keep empty folders or null for disable.</param>
        public static string Generate (string rootFolder, Options options, string cvsFileName) {
            if ((int) options == 0) {
                return string.Empty;
            }
            if (rootFolder == null) {
                rootFolder = string.Empty;
            }
            try {
                foreach (Options item in Enum.GetValues (typeof (Options))) {
                    if ((int) (options & item) != 0) {
                        GenerateItem (rootFolder, (int) item, cvsFileName);
                    }
                }
                AssetDatabase.Refresh ();
                return null;
            } catch (Exception ex) {
                AssetDatabase.Refresh ();
                return ex.Message;
            }
        }
    }
}