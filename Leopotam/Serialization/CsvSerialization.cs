// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EFramework.Serialization {
    /// <summary>
    /// Csv serialization. Supports deserialization only.
    /// </summary>
    public sealed class CsvSerialization {
        static readonly Regex _csvRegex = new Regex ("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");

        readonly List<string> _tokens = new List<string> (8);

        void ParseLine (string data) {
            _tokens.Clear ();

            foreach (Match m in _csvRegex.Matches (data)) {
                var part = m.Value.Trim ();
                if (part.Length > 0) {
                    if (part[0] == '"' && part[part.Length - 1] == '"') {
                        part = part.Substring (1, part.Length - 2);
                    }
                    part = part.Replace ("\"\"", "\"");
                }
                _tokens.Add (part);
            }
        }

        /// <summary>
        /// Deserialize csv data from raw string source.
        /// </summary>
        /// <returns>Deserialized KeyValue-dictionary as Key from first column and lists of other columns as
        /// Value.</returns>
        /// <param name="data">Raw text data.</param>
        /// <param name="list">Target list if specified (useful for decrease GC allocations).</param>
        public Dictionary<string, string[]> Deserialize (
            string data, Dictionary<string, string[]> list = null) {
            if (list == null) {
                list = new Dictionary<string, string[]> ();
            }
            list.Clear ();

            var headerLen = -1;
            string key;
            using (var reader = new StringReader (data)) {
                while (reader.Peek () != -1) {
                    ParseLine (reader.ReadLine ());
                    if (_tokens.Count == 0 || string.IsNullOrEmpty (_tokens[0])) {
                        continue;
                    }
                    if (headerLen == -1) {
                        headerLen = _tokens.Count;
                        if (headerLen < 1) {
#if UNITY_EDITOR
                            Debug.LogWarning ("Invalid csv header.");
#endif
                            break;
                        }
                    }
                    if (_tokens.Count != headerLen) {
#if UNITY_EDITOR
                        Debug.LogWarning ("Invalid csv line, skipping.");
#endif
                        continue;
                    }
                    key = _tokens[0];
                    _tokens.RemoveAt (0);
                    list[key] = _tokens.ToArray ();
                }
            }
            return list;
        }

        /// <summary>
        /// Deserialize csv data from raw string source as array - all columns will be used as common data.
        /// </summary>
        /// <returns>Deserialized list of columns data.</returns>
        /// <param name="data">Raw text data.</param>
        /// <param name="list">Target list if specified (useful for decrease GC allocations).</param>
        public List<string[]> DeserializeAsArray (
            string data, List<string[]> list = null) {
            if (list == null) {
                list = new List<string[]> ();
            }
            list.Clear ();

            var headerLen = -1;
            using (var reader = new StringReader (data)) {
                while (reader.Peek () != -1) {
                    ParseLine (reader.ReadLine ());
                    if (_tokens.Count == 0) {
                        continue;
                    }
                    if (headerLen == -1) {
                        headerLen = _tokens.Count;
                        if (headerLen < 1) {
#if UNITY_EDITOR
                            Debug.LogWarning ("Invalid csv header.");
#endif
                            break;
                        }
                    }
                    if (_tokens.Count != headerLen) {
#if UNITY_EDITOR
                        Debug.LogWarning ("Invalid csv line, skipping.");
#endif
                        continue;
                    }
                    list.Add (_tokens.ToArray ());
                }
            }
            return list;
        }
    }
}