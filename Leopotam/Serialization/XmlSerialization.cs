// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using EFramework.Collections;
using EFramework.Math;
using EFramework.Serialization.XmlInternal;

namespace EFramework.Serialization {
    /// <summary>
    /// Xml serialization. Supports deserialization only. Declaration / doctype will be ignored.
    /// </summary>
    public sealed class XmlSerialization {
        /// <summary>
        /// Deserialize xml data from raw string source to nodes tree.
        /// </summary>
        /// <param name="source">Xml source.</param>
        /// <param name="useHashesForNames">Use int-hashes instead of string for node / attribute names.
        /// When uses - better for GC allocation.</param>
        public XmlNode Deserialize (string source, bool useHashesForNames = false) {
            if (string.IsNullOrEmpty (source)) {
                throw new Exception ("Invalid xml");
            }
            var i = 0;
            while (true) {
                XmlUtils.SkipWhitespace (source, ref i);
                if (source[i] != '<') {
                    throw new Exception ("Invalid token");
                }
                i++;
                // skip declaration / doctype.
                if (source[i] == '?' || source[i] == '!') {
                    while (source[i] != '>') {
                        if (source[i] == '[') {
                            while (source[i] != ']') {
                                i++;
                            }
                        }
                        i++;
                    }
                    i++;
                    continue;
                }
                return XmlNode.Get (source, ref i, useHashesForNames);
            }
        }
    }

    /// <summary>
    /// Xml node. Contains information about node attributes and children nodes.
    /// </summary>
    public sealed class XmlNode {
        /// <summary>
        /// Node name. If "UseHashesForNames" is false - node name, otherwise null.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Node name hash. If "UseHashesForNames" is true - hash of node name, otherwise 0.
        /// </summary>
        public int NameHash { get; private set; }

        /// <summary>
        /// Textual data inside node (not nested node). Can be null.
        /// </summary>
        public string Value { get { return _valueLength > 0 ? _xmlSource.Substring (_valueStart, _valueLength) : null; } }

        public bool UseHashesForNames { get; private set; }

        /// <summary>
        /// Children nodes list.
        /// </summary>
        public readonly List<XmlNode> Children = new List<XmlNode> ();

        string _xmlSource;

        int _valueStart;

        int _valueLength;

        readonly List<XmlAttribute> _attributes = new List<XmlAttribute> (4);

        static readonly FastList<XmlNode> _pool = new FastList<XmlNode> (128);

        /// <summary>
        /// Get xml node instance from pool. For internal use only.
        /// </summary>
        /// <param name="source">Xml source.</param>
        /// <param name="offset">Offset from start of xml source.</param>
        /// <param name="useHashesForNames">Should names of nodes be hashed (faster) or not.</param>
        public static XmlNode Get (string source, ref int offset, bool useHashesForNames) {
            XmlNode item;
            if (_pool.Count > 0) {
                item = _pool[_pool.Count - 1];
                _pool.RemoveLast (false);
            } else {
                item = new XmlNode ();
            }
            item.Init (source, ref offset, useHashesForNames);
            return item;
        }

        /// <summary>
        /// Recycle node instance to pool. Warning - use only on root node!
        /// </summary>
        public void Recycle () {
            var list = Children;
            for (int i = list.Count - 1; i >= 0; i--) {
                list[i].Recycle ();
            }
            Children.Clear ();
            _xmlSource = null;
            _pool.Add (this);
        }

        XmlNode () { }

        void Init (string xmlSource, ref int i, bool useHashesForNames) {
            _xmlSource = xmlSource;
            UseHashesForNames = useHashesForNames;
            XmlUtils.SkipWhitespace (xmlSource, ref i);
            if (useHashesForNames) {
                Name = null;
                NameHash = XmlUtils.GetHashedValue (xmlSource, ref i, '>', '/', true);
            } else {
                Name = XmlUtils.GetValue (xmlSource, ref i, '>', '/', true);
                NameHash = 0;
            }
            XmlUtils.ParseAttributes (xmlSource, ref i, _attributes, '>', '/', useHashesForNames);
            // skip '/>'.
            if (xmlSource[i] == '/') {
                i += 2;
                return;
            }
            i++;
            var tempIt = i;
            XmlUtils.SkipWhitespace (xmlSource, ref tempIt);
            if (xmlSource[tempIt] == '<') {
                i = tempIt;
                // parse children.
                while (xmlSource[i + 1] != '/') {
                    i++;
                    Children.Add (Get (xmlSource, ref i, useHashesForNames));
                    XmlUtils.SkipWhitespace (xmlSource, ref i);
                    if (i >= xmlSource.Length) {
                        return;
                    }
                    if (xmlSource[i] != '<') {
                        throw new Exception ("Invalid token");
                    }
                }
                i++;
            } else {
                _valueStart = i;
                _valueLength = XmlUtils.SkipValue (xmlSource, ref i, '<', '\0', false);
                i++;
                if (xmlSource[i] != '/') {
                    throw new Exception ("Invalid ending tag at " + i);
                }
            }
            i++;
            XmlUtils.SkipWhitespace (xmlSource, ref i);
            var isValid = useHashesForNames ? XmlUtils.GetHashedValue (xmlSource, ref i, '>', '\0', true) == NameHash : XmlUtils.GetValue (xmlSource, ref i, '>', '\0', true) == Name;
            if (!isValid) {
                throw new Exception ("Start/end tag name mismatch at " + i);
            }
            XmlUtils.SkipWhitespace (xmlSource, ref i);
            if (xmlSource[i] != '>') {
                throw new Exception ("Invalid ending tag at " + i);
            }
            i++;
        }

        /// <summary>
        /// Get value of attribute. Returns null if no attribute with specified name or "UseHashesForNames" is true.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        public string GetAttribute (string name) {
            if (UseHashesForNames) {
                return null;
            }
            for (var i = _attributes.Count - 1; i >= 0; i--) {
                if (string.CompareOrdinal (_attributes[i].Name, name) == 0) {
                    var attr = _attributes[i];
                    return attr.ValueLength > 0 ? _xmlSource.Substring (attr.ValueStart, attr.ValueLength) : null;
                }
            }
            return null;
        }

        /// <summary>
        /// Get value of hashed attribute. Returns null if no attribute with specified nameHash or "UseHashesForNames" is false.
        /// </summary>
        /// <param name="nameHash">Hash of attribute name.</param>
        public string GetAttribute (int nameHash) {
            if (!UseHashesForNames) {
                return null;
            }
            for (var i = _attributes.Count - 1; i >= 0; i--) {
                if (_attributes[i].NameHash == nameHash) {
                    var attr = _attributes[i];
                    return attr.ValueLength > 0 ? _xmlSource.Substring (attr.ValueStart, attr.ValueLength) : null;
                }
            }
            return null;
        }
    }

    namespace XmlInternal {
        /// <summary>
        /// For internal use.
        /// </summary>
        static class XmlUtils {
            public static void SkipWhitespace (string str, ref int i) {
                while (i < str.Length) {
                    if (!(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r')) {
                        // skip comment.
                        if (str[i] == '<' && i + 4 < str.Length && str[i + 1] == '!' && str[i + 2] == '-' && str[i + 3] == '-') {
                            i += 4;
                            while (i + 2 < str.Length && !(str[i] == '-' && str[i + 1] == '-')) {
                                i++;
                            }
                            i += 2;
                        } else {
                            break;
                        }
                    }
                    i++;
                }
            }

            public static int SkipValue (string str, ref int i, char endChar, char endChar2, bool stopOnSpace) {
                var start = i;
                while (
                    (!stopOnSpace || !(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r')) &&
                    str[i] != endChar && str[i] != endChar2) {
                    i++;
                }
                return i - start;
            }

            public static string GetValue (string str, ref int i, char endChar, char endChar2, bool stopOnSpace) {
                var start = i;
                while (
                    (!stopOnSpace || !(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r')) &&
                    str[i] != endChar && str[i] != endChar2) {
                    i++;
                }
                return str.Substring (start, i - start);
            }

            public static int GetHashedValue (string str, ref int i, char endChar, char endChar2, bool stopOnSpace) {
                var start = i;
                while (
                    (!stopOnSpace || !(str[i] == ' ' || str[i] == '\t' || str[i] == '\n' || str[i] == '\r')) &&
                    str[i] != endChar && str[i] != endChar2) {
                    i++;
                }
                return str.GetStableHashCode (start, i - start);
            }

            public static void ParseAttributes (
                string str, ref int i, List<XmlAttribute> list, char endChar, char endChar2, bool useHashesForNames) {
                list.Clear ();
                SkipWhitespace (str, ref i);
                while (str[i] != endChar && str[i] != endChar2) {
                    var attr = new XmlAttribute ();
                    if (useHashesForNames) {
                        attr.NameHash = GetHashedValue (str, ref i, '=', '\0', true);
                    } else {
                        attr.Name = GetValue (str, ref i, '=', '\0', true);
                    }
                    SkipWhitespace (str, ref i);
                    i++;
                    SkipWhitespace (str, ref i);
                    var quote = str[i];
                    if (quote != '"' && quote != '\'') {
                        throw new Exception ("Unexpected token after " + attr.Name);
                    }
                    i++;
                    attr.ValueStart = i;
                    attr.ValueLength = SkipValue (str, ref i, quote, '\0', false);
                    i++;
                    list.Add (attr);
                    SkipWhitespace (str, ref i);
                }
            }
        }

        struct XmlAttribute {
            public string Name;

            public int NameHash;

            public int ValueStart;

            public int ValueLength;
        }
    }
}