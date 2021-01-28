// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using EFramework.Common;
using EFramework.Math;
using EFramework.Serialization;
using EFramework.SystemUi.Atlases;
using EFramework.SystemUi.Markup.Generators;
using UnityEngine;

namespace EFramework.SystemUi.Markup {
    /// <summary>
    /// Ui markup container. Supports spawning of named xml-schema from Resources folder.
    /// </summary>
    public class MarkupContainer : MonoBehaviour {
        /// <summary>
        /// Scale factor relative to "base" resolution.
        /// </summary>
        [NonSerialized]
        public float PixelSize = 1f;

        /// <summary>
        /// Drag threshold for input events.
        /// </summary>
        [NonSerialized]
        public float DragTreshold = 5f;

        /// <summary>
        /// Drag threshold for input events.
        /// </summary>
        [NonSerialized]
        public bool UseNavigation = true;

        public static readonly int HashedName = "name".GetStableHashCode ();

        [SerializeField]
        string _markupPath = "UnknownMarkup";

        [SerializeField]
        List<SpriteAtlas> _atlases = new List<SpriteAtlas> ();

        [SerializeField]
        List<Font> _fonts = new List<Font> ();

        [SerializeField]
        List<MarkupTheme> _themes = new List<MarkupTheme> ();

        XmlNode _xmlTree;

        Canvas _canvas;

        protected readonly Dictionary<int, Func<RectTransform, XmlNode, MarkupContainer, RectTransform>> Generators =
            new Dictionary<int, Func<RectTransform, XmlNode, MarkupContainer, RectTransform>> (64);

        readonly Dictionary<int, RectTransform> _namedNodes = new Dictionary<int, RectTransform> (128);

        Font _defaultFont;

        MarkupTheme _defaultTheme;

        bool _areGeneratorsAttached;

        bool _isVisualized;

        void ValidateGenerators () {
            if (!_areGeneratorsAttached) {
                Generators.Clear ();
                AttachGenerators ();
                _areGeneratorsAttached = true;
            }
        }

        protected virtual void AttachGenerators () {
            Generators.Add ("align".GetStableHashCode (), AlignNode.Create);
            Generators.Add ("box".GetStableHashCode (), BoxNode.Create);
            Generators.Add ("button".GetStableHashCode (), ButtonNode.Create);
            Generators.Add ("input".GetStableHashCode (), InputNode.Create);
            Generators.Add ("grid".GetStableHashCode (), GridNode.Create);
            Generators.Add ("image".GetStableHashCode (), ImageNode.Create);
            Generators.Add ("mask2d".GetStableHashCode (), Mask2dNode.Create);
            Generators.Add ("scrollView".GetStableHashCode (), ScrollViewNode.Create);
            Generators.Add ("slider".GetStableHashCode (), SliderNode.Create);
            Generators.Add ("stack".GetStableHashCode (), StackNode.Create);
            Generators.Add ("fixedTable".GetStableHashCode (), FixedTableNode.Create);
            Generators.Add ("text".GetStableHashCode (), TextNode.Create);
            Generators.Add ("toggle".GetStableHashCode (), ToggleNode.Create);
            Generators.Add ("toggleGroup".GetStableHashCode (), ToggleGroupNode.Create);
            Generators.Add ("ui".GetStableHashCode (), UiNode.Create);
        }

        static XmlNode LoadXml (string markupPath) {
            var asset = Resources.Load<TextAsset> (markupPath);
            if (asset == null) {
                Debug.LogWarning ("Cant load markup " + markupPath);
                return null;
            }
            XmlNode xmlTree = null;
            try {
                xmlTree = Service<XmlSerialization>.Get ().Deserialize (asset.text, true);
            } catch (Exception ex) {
                Debug.LogWarning (ex);
            }
            Resources.UnloadAsset (asset);
            return xmlTree;
        }

        void LateUpdate () {
            if (!_isVisualized) {
                CreateVisuals ();
            }
        }

        void CreateVisualNode (XmlNode xmlTree, RectTransform root) {
            if (xmlTree == null) {
                return;
            }

            Func<RectTransform, XmlNode, MarkupContainer, RectTransform> generator;

            var isFound = Generators.TryGetValue (xmlTree.NameHash, out generator);
            if (!isFound) {
                generator = BoxNode.Create;
            }

            var tr = MarkupUtils.CreateUiObject (null, root);
            var contentTr = generator (tr, xmlTree, this);

#if UNITY_EDITOR
            if (!isFound) {
                tr.name = string.Format ("unknown-replaced-with-{0}", tr.name);
                Debug.LogWarningFormat (tr, "Unknown hashed-node \"{0}\" - box-node will be used instead",
                    xmlTree.NameHash);
            }
#endif

            if ((object) _canvas == null) {
                _canvas = tr.GetComponentInChildren<Canvas> ();
            }

            var nodeName = xmlTree.GetAttribute (HashedName);
            if (!string.IsNullOrEmpty (nodeName)) {
#if UNITY_EDITOR
                tr.name = string.Format ("{0}-{1}", tr.name, nodeName);
#endif
                var nodeNameHash = nodeName.GetStableHashCode ();
                if (_namedNodes.ContainsKey (nodeNameHash)) {
#if UNITY_EDITOR
                    Debug.LogWarning ("Duplicate name: " + nodeName);
#endif
                } else {
                    _namedNodes[nodeNameHash] = tr;
                }
            }

            var children = xmlTree.Children;
            if (children.Count > 0) {
                if ((object) contentTr != null) {
                    for (int i = 0, iMax = children.Count; i < iMax; i++) {
                        CreateVisualNode (children[i], contentTr);
                    }
                } else {
                    Debug.LogWarning ("Node not supported children.", tr);
                }
            }
        }

        /// <summary>
        /// Clear visuals and set path to new markup schema.
        /// </summary>
        /// <param name="markupPath">Path to markup schema.</param>
        public void SetMarkupPath (string markupPath) {
            Clear (false);
            _markupPath = markupPath;
        }

        /// <summary>
        /// Remove generated widgets from scene.
        /// </summary>
        public void ClearVisuals () {
            _isVisualized = false;
            _canvas = null;
            PixelSize = 1f;
            DragTreshold = 5f;
            UseNavigation = true;
            _namedNodes.Clear ();
            var tr = transform;
            for (var i = tr.childCount - 1; i >= 0; i--) {
#if UNITY_EDITOR
                if (Application.isPlaying) {
                    Destroy (tr.GetChild (i).gameObject);
                } else {
                    DestroyImmediate (tr.GetChild (i).gameObject);
                }
#else
                Destroy (tr.GetChild (i).gameObject);
#endif
            }
        }

        /// <summary>
        /// Full cleanup of container (destroy widgets, unload xml, unregister generators).
        /// </summary>
        /// <param name="unregisterGenerators">Unregister generators or not.</param>
        public void Clear (bool unregisterGenerators = true) {
            ClearVisuals ();

            if (_xmlTree != null) {
                _xmlTree.Recycle ();
                _xmlTree = null;
            }

            _defaultFont = null;
            _defaultTheme = null;

            if (unregisterGenerators) {
                _areGeneratorsAttached = false;
                Generators.Clear ();
            }
        }

        /// <summary>
        /// Get root canvas of this infrastructure.
        /// </summary>
        public Canvas GetCanvas () {
            return _canvas;
        }

        /// <summary>
        /// Force cleanup / create widgets infrastructure from attached xml-schema.
        /// </summary>
        public void CreateVisuals () {
            if (_isVisualized) {
                return;
            }
            ValidateGenerators ();
            if ((object) _defaultFont == null) {
                _defaultFont = _fonts.Count > 0 ? _fonts[0] : Resources.GetBuiltinResource<Font> ("Arial.ttf");
            }
            if ((object) _defaultTheme == null) {
                _defaultTheme = _themes.Count > 0 ? _themes[0] : ScriptableObject.CreateInstance<MarkupTheme> ();
            }
            if (_xmlTree == null) {
                _xmlTree = LoadXml (_markupPath);
            }
            ClearVisuals ();
            CreateVisualNode (_xmlTree, MarkupUtils.CreateUiObject ("root", transform));
            _isVisualized = true;
        }

        /// <summary>
        /// Attach sprite atlas. Should be called before any visuals with content from this atlas will be created.
        /// </summary>
        /// <param name="atlas">Sprite atlas.</param>
        public void AttachAtlas (SpriteAtlas atlas) {
            if ((object) atlas != null && !_atlases.Contains (atlas)) {
                _atlases.Add (atlas);
            }
        }

        /// <summary>
        /// Attach font. Should be called before any visuals with content from this font will be created.
        /// </summary>
        /// <param name="font">Font.</param>
        public void AttachFont (Font font) {
            if ((object) font != null && !_fonts.Contains (font)) {
                _fonts.Add (font);
            }
        }

        /// <summary>
        /// Attach markup theme. Should be called before any visuals with content from this theme will be created.
        /// </summary>
        /// <param name="theme">Markup theme.</param>
        public void AttachTheme (MarkupTheme theme) {
            if ((object) theme != null && !_themes.Contains (theme)) {
                _themes.Add (theme);
            }
        }

        /// <summary>
        /// Get attached atlas or null.
        /// </summary>
        /// <param name="atlasName">Name of atlas.</param>
        public SpriteAtlas GetAtlas (string atlasName) {
            if (!string.IsNullOrEmpty (atlasName)) {
                for (var i = _atlases.Count - 1; i >= 0; i--) {
                    if (string.CompareOrdinal (_atlases[i].GetName (), atlasName) == 0) {
                        return _atlases[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get font. If not found - default (or first) font will be returned.
        /// </summary>
        /// <param name="fontName">Font name</param>
        public Font GetFont (string fontName) {
            if (!string.IsNullOrEmpty (fontName)) {
                for (var i = _fonts.Count - 1; i >= 0; i--) {
                    if (string.CompareOrdinal (_fonts[i].name, fontName) == 0) {
                        return _fonts[i];
                    }
                }
            }
            return _defaultFont;
        }

        /// <summary>
        /// Get markup theme. If not found - default (or first) theme will be returned.
        /// </summary>
        /// <param name="themeName">Theme name.</param>
        public MarkupTheme GetTheme (string themeName) {
            if (!string.IsNullOrEmpty (themeName)) {
                for (var i = _themes.Count - 1; i >= 0; i--) {
                    if (string.CompareOrdinal (_themes[i].GetName (), themeName) == 0) {
                        return _themes[i];
                    }
                }
            }
            return _defaultTheme;
        }

        /// <summary>
        /// Get widget of specific node from markup or null.
        /// </summary>
        /// <param name="nodeName">Unique name of node.</param>
        public RectTransform GetNamedNode (string nodeName) {
            var hash = nodeName.GetStableHashCode ();
            RectTransform rt;
            return _namedNodes.TryGetValue (hash, out rt) ? rt : null;
        }

        /// <summary>
        /// Create new markup infrastructure from code.
        /// </summary>
        /// <param name="markupPath">Path to xml-schema from Resources folder.</param>
        /// <param name="parent">Root transform for infrastructure.</param>
        public static MarkupContainer CreateMarkup (string markupPath, Transform parent = null) {
            if (string.IsNullOrEmpty (markupPath)) {
                return null;
            }
            var container =
                new GameObject (
#if UNITY_EDITOR
                    "_MARKUP_" + markupPath
#endif
                ).AddComponent<MarkupContainer> ();
            container._markupPath = markupPath;
            if ((object) parent != null) {
                container.transform.SetParent (parent, false);
            }
            return container;
        }
    }
}