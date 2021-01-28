// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.SystemUi.Layouts {
    /// <summary>
    /// Resizer of BoxCollider2D size for follow to RectTransform size.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (RectTransform))]
    [RequireComponent (typeof (BoxCollider2D))]
    public sealed class ResizeBoxCollider2D : MonoBehaviour {
        [SerializeField]
        bool _once = true;

        RectTransform _transform;

        BoxCollider2D _collider;

        static Vector3[] _corners = new Vector3[4];

        void LateUpdate () {
            Reposition ();
            if (_once && Application.isPlaying) {
                enabled = false;
            }
        }

        void Reposition () {
            if ((object) _transform == null) {
                _transform = (RectTransform) transform;
            }
            if ((object) _collider == null) {
                _collider = GetComponent<BoxCollider2D> ();
            }
            _transform.GetLocalCorners (_corners);
            var xMin = float.MaxValue;
            var xMax = float.MinValue;
            var yMin = float.MaxValue;
            var yMax = float.MinValue;
            for (var i = 3; i >= 0; i--) {
                if (xMin > _corners[i].x) {
                    xMin = _corners[i].x;
                }
                if (xMax < _corners[i].x) {
                    xMax = _corners[i].x;
                }
                if (yMin > _corners[i].y) {
                    yMin = _corners[i].y;
                }
                if (yMax < _corners[i].y) {
                    yMax = _corners[i].y;
                }
            }
            _collider.size = new Vector2 (xMax - xMin, yMax - yMin);
        }
    }
}