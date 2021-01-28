// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using EFramework.Serialization;
using UnityEditor;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    /// <summary>
    /// Downloader from google docs sheets.
    /// </summary>
    sealed class GoogleDocsCsvDownloader : EditorWindow {
        const string Title = "CSV downloader";

        const string ProjectPrefsKey = "lg.external-csv.update";

        const string UrlDefault = "http://localhost";

        const string ResDefault = "NewCsv.csv";

        static readonly Regex CsvMultilineRegex = new Regex ("\"([^\"]|\"\"|\\n)*\"");

        readonly JsonSerialization _serializer = new JsonSerialization ();

        List<RecordInfo> _items;

        Vector2 _scrollPos;

        string _newUrl;

        string _newRes;

        JsonMode _newJson;

        [MenuItem ("Window/LeopotamGroupLibrary/Download external CSV-data...")]
        static void OpenEditorWindow () {
            var win = GetWindow<GoogleDocsCsvDownloader> (true);
            var pos = win.position;
            pos.width = 600f;
            pos.height = 300f;
            win.position = pos;
            win.titleContent.text = Title;
        }

        void OnEnable () {
            _scrollPos = Vector2.zero;
        }

        void Load () {
            try {
                _items = _serializer.Deserialize<List<RecordInfo>> (ProjectPrefs.GetString (ProjectPrefsKey, "{}"));
                if (_items == null) {
                    throw new Exception ();
                }
            } catch {
                _items = new List<RecordInfo> ();
            }
        }

        void Save () {
            if (_items != null && _items.Count > 0) {
                ProjectPrefs.SetString (ProjectPrefsKey, _serializer.Serialize (_items));
            } else {
                ProjectPrefs.DeleteKey (ProjectPrefsKey);
            }
        }

        void OnGUI () {
            if (_items == null) {
                Load ();
            }

            if (string.IsNullOrEmpty (_newUrl)) {
                _newUrl = UrlDefault;
            }
            if (string.IsNullOrEmpty (_newRes)) {
                _newRes = ResDefault;
            }

            var needSave = false;

            if (_items.Count > 0) {
                EditorGUILayout.LabelField ("List of csv resources", EditorStyles.boldLabel);
                _scrollPos = GUILayout.BeginScrollView (_scrollPos, false, true);
                for (var i = 0; i < _items.Count; i++) {
                    var item = _items[i];
                    GUILayout.BeginHorizontal (GUI.skin.box);
                    GUILayout.BeginVertical ();

                    GUILayout.BeginHorizontal ();
                    EditorGUILayout.LabelField (
                        "External url path:", EditorStyles.label, GUILayout.Width (EditorGUIUtility.labelWidth));
                    EditorGUILayout.SelectableLabel (
                        item.Url, EditorStyles.textField, GUILayout.Height (EditorGUIUtility.singleLineHeight));
                    GUILayout.EndHorizontal ();

                    GUILayout.BeginHorizontal ();
                    EditorGUILayout.LabelField (
                        "Local resource path:", EditorStyles.label, GUILayout.Width (EditorGUIUtility.labelWidth));
                    EditorGUILayout.SelectableLabel (
                        item.Resource, EditorStyles.textField, GUILayout.Height (EditorGUIUtility.singleLineHeight));
                    GUILayout.EndHorizontal ();

                    GUILayout.BeginHorizontal ();
                    EditorGUILayout.LabelField (
                        "Convert to JSON:", EditorStyles.label, GUILayout.Width (EditorGUIUtility.labelWidth));
                    GUI.enabled = false;
                    EditorGUILayout.EnumPopup (item.JsonMode);
                    GUI.enabled = true;
                    GUILayout.EndHorizontal ();

                    GUILayout.EndVertical ();
                    if (GUILayout.Button ("Remove", GUILayout.Width (80f), GUILayout.Height (52f))) {
                        _items.Remove (item);
                        needSave = true;
                    }
                    GUILayout.EndHorizontal ();
                }
                GUILayout.EndScrollView ();
            }

            GUILayout.Space (4f);
            EditorGUILayout.LabelField ("New external csv", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal (GUI.skin.box);
            GUILayout.BeginVertical ();
            _newUrl = EditorGUILayout.TextField ("External Url path:", _newUrl).Trim ();
            _newRes = EditorGUILayout.TextField ("Resource file:", _newRes).Trim ();
            _newJson = (JsonMode) EditorGUILayout.EnumPopup ("Convert to JSON:", _newJson);
            GUILayout.EndVertical ();
            if (GUILayout.Button ("Add", GUILayout.Width (80f), GUILayout.Height (52f))) {
                var newItem = new RecordInfo ();
                newItem.Url = _newUrl;
                newItem.Resource = _newRes;
                newItem.JsonMode = _newJson;
                _items.Add (newItem);
                _newUrl = UrlDefault;
                _newRes = ResDefault;
                _newJson = JsonMode.None;
                needSave = true;
            }
            GUILayout.EndHorizontal ();

            if (needSave) {
                Save ();
            }

            GUILayout.Space (4f);

            GUI.enabled = _items.Count > 0;
            if (GUILayout.Button ("Update data from external urls", GUILayout.Height (30f))) {
                var res = Process (_items);
                EditorUtility.DisplayDialog (titleContent.text, res ?? "Success", "Close");
            }
            GUI.enabled = true;
        }

        static string ConvertToDictJson (string data) {
            var sb = new StringBuilder (data.Length * 2);
            var list = new CsvSerialization ().Deserialize (data);
            if (list.Count < 2) {
                throw new Exception ("Invalid header data: first line should contains field names, second line - pair of wrapping chars.");
            }

            sb.Append ("{");
            var it = list.GetEnumerator ();

            // header.
            it.MoveNext ();
            var headerKey = it.Current.Key;
            var headerValue = it.Current.Value;

            // wrappers.
            it.MoveNext ();
            var wrapperKey = it.Current.Key;
            var wrapperValue = it.Current.Value;

            if (wrapperKey != "\"\"") {
                throw new Exception (string.Format ("Invalid wrapper data for \"{0}\" field: it should be wrapped with \"\".", headerKey));
            }

            for (var i = 0; i < wrapperValue.Length; i++) {
                if (!(
                        wrapperValue[i] == string.Empty ||
                        wrapperValue[i] == "[]" ||
                        wrapperValue[i] == "{}" ||
                        string.Compare (wrapperValue[i], "IGNORE", true) == 0 ||
                        wrapperValue[i] == "\"\"")) {
                    throw new Exception (string.Format ("Invalid wrapper data for \"{0}\" field.", headerValue[i]));
                }
            }

            var needObjectsComma = false;
            string itemValue;
            string wrapChars;
            while (it.MoveNext ()) {
                sb.AppendFormat ("{0}\"{1}\":{{", needObjectsComma ? "," : string.Empty, it.Current.Key);
                var needFieldsComma = false;
                for (var i = 0; i < headerValue.Length; i++) {
                    wrapChars = wrapperValue[i];
                    if (string.Compare (wrapChars, "IGNORE", true) == 0) {
                        continue;
                    }
                    itemValue = wrapChars.Length > 0 ?
                        string.Format ("{0}{1}{2}", wrapChars[0], it.Current.Value[i], wrapChars[1]) : it.Current.Value[i];
                    sb.AppendFormat ("{0}\"{1}\":{2}", needFieldsComma ? "," : string.Empty, headerValue[i], itemValue);
                    needFieldsComma = true;
                }
                sb.Append ("}");
                needObjectsComma = true;
            }

            sb.Append ("}");
            return sb.ToString ();
        }

        static string ConvertToArrayJson (string data) {
            var sb = new StringBuilder (data.Length * 2);
            var list = new CsvSerialization ().DeserializeAsArray (data);
            if (list.Count < 2) {
                throw new Exception ("Invalid header data: first line should contains field names, second line - pair of wrapping chars.");
            }

            sb.Append ("[");
            var it = list.GetEnumerator ();

            // header.
            it.MoveNext ();
            var headerValue = it.Current;

            // wrappers.
            it.MoveNext ();
            var wrapperValue = it.Current;
            for (var i = 0; i < wrapperValue.Length; i++) {
                if (!(
                        wrapperValue[i] == string.Empty ||
                        wrapperValue[i] == "[]" ||
                        wrapperValue[i] == "{}" ||
                        string.Compare (wrapperValue[i], "IGNORE", true) == 0 ||
                        wrapperValue[i] == "\"\"")) {
                    throw new Exception (string.Format ("Invalid wrapper data for \"{0}\" field.", headerValue[i]));
                }
            }

            var needObjectsComma = false;
            string itemValue;
            string wrapChars;
            while (it.MoveNext ()) {
                sb.AppendFormat ("{0}{{", needObjectsComma ? "," : string.Empty);
                var needFieldsComma = false;
                for (var i = 0; i < headerValue.Length; i++) {
                    wrapChars = wrapperValue[i];
                    if (string.Compare (wrapChars, "IGNORE", true) == 0) {
                        continue;
                    }
                    itemValue = wrapChars.Length > 0 ?
                        string.Format ("{0}{1}{2}", wrapChars[0], it.Current[i], wrapChars[1]) : it.Current[i];
                    sb.AppendFormat ("{0}\"{1}\":{2}", needFieldsComma ? "," : string.Empty, headerValue[i], itemValue);
                    needFieldsComma = true;
                }
                sb.Append ("}");
                needObjectsComma = true;
            }

            sb.Append ("]");
            return sb.ToString ();
        }

        public static string Process (List<RecordInfo> items) {
            if (items == null || items.Count == 0) {
                return "No data";
            }
            try {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                using (var www = new WebClient ()) {
                    www.Encoding = Encoding.UTF8;
                    string data;
                    foreach (var item in items) {
                        if (!string.IsNullOrEmpty (item.Url) && !string.IsNullOrEmpty (item.Resource)) {
                            // Dirty hack for url, because standard "publish to web" has huge lag up to 30 minutes.
                            try {
                                data = www.DownloadString (item.Url.Replace ("?", string.Empty).Replace ("/edit", "/export?format=csv&"));
                            } catch (Exception urlEx) {
                                throw new Exception (string.Format ("\"{0}\": {1}", item.Url, urlEx.Message));
                            }
                            var path = string.Format ("{0}/{1}", Application.dataPath, item.Resource);
                            var folder = Path.GetDirectoryName (path);
                            if (!Directory.Exists (folder)) {
                                Directory.CreateDirectory (folder);
                            }

                            // Fix for multiline string.
                            data = CsvMultilineRegex.Replace (data, m => m.Value.Replace ("\n", "\\n"));

                            // json generation.
                            switch (item.JsonMode) {
                                case JsonMode.Array:
                                    data = ConvertToArrayJson (data);
                                    break;
                                case JsonMode.Dictionary:
                                    data = ConvertToDictJson (data);
                                    break;
                            }

                            File.WriteAllText (path, data, Encoding.UTF8);
                        }
                    }
                }
                AssetDatabase.Refresh ();
                return null;
            } catch (Exception ex) {
                AssetDatabase.Refresh ();
                return ex.Message;
            } finally {
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }

        public enum JsonMode {
            None = 0,
            Array = 1,
            Dictionary = 2
        }

        public sealed class RecordInfo {
            [JsonName ("u")]
            public string Url = string.Empty;

            [JsonName ("r")]
            public string Resource = string.Empty;

            [JsonName ("j")]
            public JsonMode JsonMode = JsonMode.None;
        }
    }
}