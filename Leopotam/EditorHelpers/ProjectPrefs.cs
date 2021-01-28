// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using EFramework.Common;
using EFramework.Serialization;
using UnityEngine;

namespace EFramework.EditorHelpers {
    /// <summary>
    /// EditorPrefs replacement with keeping data per project.
    /// </summary>
    public static class ProjectPrefs {
        const string StorePath = "{0}/../ProjectSettings/LeopotamGroupProjectPrefs.txt";

        static string _storeFile;

        static Dictionary<string, string> _data;

        static ProjectPrefs () {
            Service<JsonSerialization>.Get ();
        }

        static void LoadData () {
            if (_storeFile == null) {
                _storeFile = string.Format (StorePath, Application.dataPath);
            }
            if (_data != null) {
                return;
            }
            try {
                var content = File.ReadAllText (_storeFile);
                _data = Service<JsonSerialization>.Get ().Deserialize<Dictionary<string, string>> (content);
                if (_data == null) {
                    throw new UnityException ();
                }
            } catch {
                _data = new Dictionary<string, string> ();
            }
        }

        static void SaveData () {
            try {
                if (_data.Count > 0) {
                    File.WriteAllText (_storeFile, Service<JsonSerialization>.Get ().Serialize (_data));
                } else {
                    if (File.Exists (_storeFile)) {
                        File.Delete (_storeFile);
                    }
                }
            } catch (Exception ex) {
                Debug.LogWarning ("ProjectPrefs.SaveData: " + ex.Message);
            }
        }

        /// <summary>
        /// Force reload data.
        /// </summary>
        public static void Reset () {
            _data = null;
            LoadData ();
        }

        /// <summary>
        /// Is key-store contains specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public static bool HasKey (string key) {
            if (string.IsNullOrEmpty (key)) {
                throw new UnityException ("Invalid key");
            }
            LoadData ();
            return _data.ContainsKey (key);
        }

        /// <summary>
        /// Delete all keys.
        /// </summary>
        public static void DeleteAll () {
            if (_storeFile == null) {
                _storeFile = string.Format (StorePath, Application.dataPath);
            }
            _data.Clear ();
            SaveData ();
        }

        /// <summary>
        /// Delete key.
        /// </summary>
        /// <param name="key">Key.</param>
        public static void DeleteKey (string key) {
            LoadData ();
            if (HasKey (key)) {
                _data.Remove (key);
            }
            SaveData ();
        }

        /// <summary>
        /// Get string value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static string GetString (string key, string defaultValue = null) {
            return HasKey (key) ? _data[key] : defaultValue;
        }

        /// <summary>
        /// Set string value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="data">Data.</param>
        public static void SetString (string key, string data) {
            HasKey (key);
            _data[key] = data;
            SaveData ();
        }

        /// <summary>
        /// Get int value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static int GetInt (string key, int defaultValue = 0) {
            if (HasKey (key)) {
                int val;
                if (int.TryParse (_data[key], out val)) {
                    return val;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Set int value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="data">Data.</param>
        public static void SetInt (string key, int data) {
            SetString (key, data.ToString (NumberFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Get float value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static float GetFloat (string key, float defaultValue = 0f) {
            if (HasKey (key)) {
                float val;
                if (float.TryParse (_data[key], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out val)) {
                    return val;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Set float value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="data">Data.</param>
        public static void SetFloat (string key, float data) {
            SetString (key, data.ToString (NumberFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Get bool value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static bool GetBool (string key, bool defaultValue = false) {
            return GetInt (key, defaultValue ? 1 : 0) != 0;
        }

        /// <summary>
        /// Set bool value by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="data">Data.</param>
        public static void SetBool (string key, bool data) {
            SetInt (key, data ? 1 : 0);
        }
    }
}

#endif