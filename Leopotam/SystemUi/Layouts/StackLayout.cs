// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFramework.SystemUi.Layouts {
    /// <summary>
    /// One axis alignment.
    /// </summary>
    public enum OneAxisAlignment {
        Start,
        Center,
        End
    }

    /// <summary>
    /// Stack layout along one axis with respect of items sizes.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (RectTransform))]
    public sealed class StackLayout : UIBehaviour, ILayoutGroup {
        /// <summary>
        /// Items alignment relative to stack transform.
        /// </summary>
        public OneAxisAlignment ChildAlignment {
            get { return _childAlignment; }
            set {
                if (_childAlignment != value) {
                    _childAlignment = value;
                    SetDirty ();
                }
            }
        }

        /// <summary>
        /// Padding between items.
        /// </summary>
        public float Padding {
            get { return _padding; }
            set {
                if (_padding != value) {
                    _padding = value;
                    SetDirty ();
                }
            }
        }

        /// <summary>
        /// Vertical layout.
        /// </summary>
        public bool IsVertical {
            get { return _isVertical; }
            set {
                if (_isVertical != value) {
                    _isVertical = value;
                    SetDirty ();
                }
            }
        }

        /// <summary>
        /// Inverse items order.
        /// </summary>
        public bool IsReverse {
            get { return _isReverse; }
            set {
                if (_isReverse != value) {
                    _isReverse = value;
                    SetDirty ();
                }
            }
        }

        private static readonly List<RectTransform> _children = new List<RectTransform> (128);

        [SerializeField]
        private OneAxisAlignment _childAlignment = OneAxisAlignment.Start;

        [SerializeField]
        private float _padding;

        [SerializeField]
        private bool _isVertical = false;

        [SerializeField]
        private bool _isReverse = false;

        void SetDirty () {
            if (!IsActive ()) {
                return;
            }
            LayoutRebuilder.MarkLayoutForRebuild (transform as RectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate () {
            base.OnValidate ();
            SetDirty ();
        }
#endif

        protected override void OnEnable () {
            base.OnEnable ();
            SetDirty ();
        }

        protected override void OnDisable () {
            LayoutRebuilder.MarkLayoutForRebuild (transform as RectTransform);
            base.OnDisable ();
        }

        protected override void OnDidApplyAnimationProperties () {
            SetDirty ();
        }

        public void SetLayoutHorizontal () { }

        public void SetLayoutVertical () {
            var root = transform;
            var childCount = root.childCount;
            if (childCount == 0) {
                return;
            }

            RectTransform tr;
            var size = Vector2.zero;

            // collect children.
            _children.Clear ();
            for (var i = 0; i < childCount; i++) {
                tr = root.GetChild (i) as RectTransform;
                if ((object) tr != null && tr.gameObject.activeInHierarchy) {
                    _children.Add (tr);
                    size += tr.sizeDelta;
                }
            }

            var count = _children.Count;
            if (count == 0) {
                return;
            }

            size.x += _padding * (count - 1);
            size.y += _padding * (count - 1);

            switch (_childAlignment) {
                case OneAxisAlignment.Center:
                    size.x *= -0.5f;
                    size.y *= 0.5f;
                    break;
                case OneAxisAlignment.End:
                    size.x = -size.x;
                    break;
                default:
                    size = Vector2.zero;
                    break;
            }

            var offset = new Vector2 (_isVertical ? 0f : size.x, _isVertical ? size.y : 0f);
            var dir = _isVertical ? Vector2.down : Vector2.right;

            var idx = _isReverse ? count - 1 : 0;
            var idxDir = _isReverse ? -1 : 1;
            for (var i = 0; i < count; i++) {
                tr = _children[idx];
                size = Vector2.Scale (dir, tr.sizeDelta);
                tr.localPosition = offset + size * 0.5f;
                offset += size + dir * _padding;
                idx += idxDir;
            }
            _children.Clear ();
        }
    }
}