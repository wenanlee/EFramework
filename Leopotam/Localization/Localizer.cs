// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using EFramework.Common;
using EFramework.Serialization;
using UnityEngine;

namespace EFramework.Localization {
    /// <summary>
    /// Localizer - helper for localization. Supports dynamic overriding of localization tokens with rollback.
    /// </summary>
    public static class Localizer {
        /// <summary>
        /// Current language.
        /// </summary>
        /// <value>The language.</value>
        public static string Language {
            get { return _language; }
            set {
                if (!string.IsNullOrEmpty (value) && _language != value) {
                    SetLanguage (value);
                    RelocalizeUi ();
                }
            }
        }

        const string HeaderToken = "KEY";

        const string DefaultStaticSourcePath = "Localization";

        const string DefaultLanguage = "English";

        const string SettingsKey = "lg.locale";

        const string OnLocalizeMethodName = "OnLocalize";

        static readonly Dictionary<string, string[]> _statics = new Dictionary<string, string[]> (64);

        static readonly Dictionary<string, string[]> _dynamics = new Dictionary<string, string[]> (64);

        static string[] _header;

        static string _language;

        static int _langId;

        static Localizer () {
            _header = null;
            _statics.Clear ();
            UnloadDynamics ();
            AddStaticSource (DefaultStaticSourcePath);
            SetLanguage (PlayerPrefs.GetString (SettingsKey, DefaultLanguage));
        }

        static void SetLanguage (string lang) {
            _language = lang;
            _langId = _header != null ? Array.IndexOf (_header, _language) : -1;
            PlayerPrefs.SetString (SettingsKey, _language);
        }

        static string LoadAsset (string assetPath) {
            var asset = Resources.Load<TextAsset> (assetPath);
            string data = null;
            if (asset != null) {
                data = asset.text;
                Resources.UnloadAsset (asset);
            }
            return data;
        }

        static void LoadData (string data, Dictionary<string, string[]> storage) {
            Service<CsvSerialization>.Get ().Deserialize (data, storage);
            if (!storage.ContainsKey (HeaderToken)) {
                storage.Clear ();
                return;
            }

            foreach (var item in storage) {
                for (int i = 0, iMax = item.Value.Length; i < iMax; i++) {
                    item.Value[i] = item.Value[i].Replace ("\\n", "\n");
                }
            }

            _header = storage[HeaderToken];
            if (!string.IsNullOrEmpty (_language)) {
                var langId = Array.IndexOf (_header, _language);
                if (_langId != -1 && langId != _langId) {
#if UNITY_EDITOR
                    Debug.LogWarning ("Invalid languages order in source, skipping.");
#endif
                    return;
                }
                if (_langId == -1) {
                    _langId = langId;
                }
            }
        }

        /// <summary>
        /// Get localization for token.
        /// </summary>
        /// <param name="token">Localization token.</param>
        /// <param name="returnTokenOnFail">Should token value be returned as result if no data was found or null otherwise.</param>
        public static string Get (string token, bool returnTokenOnFail = true) {
            if (_langId == -1) {
                return token;
            }
            string[] retVals;
            if (_dynamics.TryGetValue (token, out retVals)) {
                return retVals[_langId];
            }
            if (_statics.TryGetValue (token, out retVals)) {
                return retVals[_langId];
            }
            return returnTokenOnFail ? token : null;
        }

        /// <summary>
        /// Get pluralized string of count value.
        /// One / Two / Many pluralized versions are supported through
        /// "{token}-plural-one" / "{token}-plural-two" / "{token}-plural-many" naming schemas.
        /// If no data will be found - fallback to Get(token) call.
        /// </summary>
        /// <param name="count">Value to pluralize.</param>
        /// <param name="token">Base token of pluralization.</param>
        public static string GetPlural (int count, string token) {
            if (count < 0) {
                count = -count;
            }
            string retVal;
            if (count == 1) {
                retVal = Get (string.Format ("{0}-plural-one", token), false);
                if (retVal != null) {
                    return retVal;
                }
            }
            if (count > 1 && count < 5) {
                retVal = Get (string.Format ("{0}-plural-two", token), false);
                if (retVal != null) {
                    return retVal;
                }
            }
            if (count == 0 || count >= 5) {
                retVal = Get (string.Format ("{0}-plural-many", token), false);
                if (retVal != null) {
                    return retVal;
                }
            }
            return Get (token);
        }

        /// <summary>
        /// Add non-unloadable localization source.
        /// </summary>
        /// <returns>The static source.</returns>
        /// <param name="sourcePath">Source path.</param>
        public static void AddStaticSource (string sourcePath) {
            var data = LoadAsset (sourcePath);
            if (!string.IsNullOrEmpty (data)) {
                LoadData (data, _statics);
            }
        }

        /// <summary>
        /// Add unloadable localization source. Can overrides loaded tokens and be removed by UnloadDynamics call.
        /// </summary>
        /// <returns>The dynamic source.</returns>
        /// <param name="sourcePath">Source path.</param>
        public static void AddDynamicSource (string sourcePath) {
            var data = LoadAsset (sourcePath);
            if (!string.IsNullOrEmpty (data)) {
                LoadData (data, _dynamics);
            }
        }

        /// <summary>
        /// Unload all dynamics localization sources.
        /// </summary>
        /// <returns>The dynamics.</returns>
        public static void UnloadDynamics () {
            _dynamics.Clear ();
        }

        /// <summary>
        /// Raise "OnLocalize" message on all active GameObjects.
        /// </summary>
        /// <returns>The user interface.</returns>
        public static void RelocalizeUi () {
            UnityExtensions.BroadcastToAll (OnLocalizeMethodName);
        }
    }
}