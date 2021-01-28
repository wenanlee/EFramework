// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.SystemUi.Layouts {
    /// <summary>
    /// Table layout without children resize.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (RectTransform))]
    public sealed class FixedTableLayout : MonoBehaviour {
        /// <summary>
        /// Items in horizontal axis.
        /// </summary>
        public int ItemsInRow {
            get { return _itemsInRow; }
            set {
                if (_itemsInRow != value && _itemsInRow > 0) {
                    _itemsInRow = value;
                    NeedReposition ();
                }
            }
        }

        /// <summary>
        /// Will be used as cell size if > 0.
        /// </summary>
        public Vector2 CellSize {
            get { return _cellSize; }
            set {
                if (_cellSize != value) {
                    _cellSize = value;
                    NeedReposition ();
                }
            }
        }

        [Range (1, 128)]
        [SerializeField]
        private int _itemsInRow = 1;

        [SerializeField]
        private Vector2 _cellSize = Vector2.zero;

        private void Update () {
            RepositionNow ();
            if (Application.isPlaying) {
                enabled = false;
            }
        }

        /// <summary>
        /// mark as dirty and need to reposition on next Update.
        /// </summary>
        public void NeedReposition () {
            enabled = true;
        }

        /// <summary>
        /// Force reposition children immediately.
        /// </summary>
        public void RepositionNow () {
            var root = GetComponent<RectTransform> ();
            var childCount = root.childCount;
            if (childCount == 0) {
                return;
            }
            var rootSize = root.sizeDelta;
            var xOffset = _cellSize.x <= 0f ? rootSize.x / _itemsInRow : _cellSize.x;
            var rows = Mathf.CeilToInt (childCount / (float) _itemsInRow);
            var yOffset = _cellSize.y <= 0f ? rootSize.y / rows : _cellSize.y;
            var pivotOffset = new Vector2 (-xOffset * (_itemsInRow * 0.5f - 0.5f), yOffset * (rows * 0.5f - 0.5f));
            float itemOffset;
            float rowOffset;
            Vector3 pos;
            Transform tr;
            for (var i = 0; i < childCount; i++) {
                itemOffset = (i % _itemsInRow) * xOffset;
                rowOffset = -(i / _itemsInRow) * yOffset;
                tr = root.GetChild (i);
                pos = new Vector3 (itemOffset + pivotOffset.x, rowOffset + pivotOffset.y, 0f);
                tr.localPosition = pos;
            }
        }
    }
}