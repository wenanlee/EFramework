// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace EFramework.Math {
    /// <summary>
    /// Axis-aligned 2d bounding box with integer coords.
    /// </summary>
    [Serializable]
    public struct Bounds2i {
        /// <summary>
        /// Min X/Y values of bounds.
        /// </summary>
        public Vector2Int Min;

        /// <summary>
        /// Max X/Y values of bounds.
        /// </summary>
        public Vector2Int Max;

        public Bounds2i (Vector2Int min, Vector2Int max) {
            Min = min;
            Max = max;
            Validate ();
        }

        public Bounds2i (int x, int y, int width, int height) {
            Min = new Vector2Int (x, y);
            Max = new Vector2Int (x + width, y + height);
            Validate ();
        }

        /// <summary>
        /// Is specified point contains in bounds.
        /// </summary>
        /// <param name="point">Point</param>
        public bool Contains (Vector2Int point) {
            return point.x >= Min.x && point.x <= Max.x && point.y >= Min.y && point.y <= Max.y;
        }

        /// <summary>
        /// Grow bounds to include specified bounds.
        /// </summary>
        /// <param name="bounds">Bounds</param>
        public void Encapsulate (Bounds2i bounds) {
            if (Min.x > bounds.Min.x) {
                Min.x = bounds.Min.x;
            }
            if (Min.y > bounds.Min.y) {
                Min.y = bounds.Min.y;
            }
            if (Max.x < bounds.Max.x) {
                Max.x = bounds.Max.x;
            }
            if (Max.y < bounds.Max.y) {
                Max.y = bounds.Max.y;
            }
        }

        /// <summary>
        /// Grow / shrink bounds to include specified point.
        /// </summary>
        /// <param name="point">Point</param>
        public void Encapsulate (Vector2Int point) {
            Encapsulate (point.x, point.y);
        }

        /// <summary>
        /// Grow / shrink bounds to include specified point.
        /// </summary>
        /// <param name="x">X coord of point</param>
        /// <param name="y">Y coord of point</param>
        public void Encapsulate (int x, int y) {
            if (Min.x > x) {
                Min.x = x;
            }
            if (Min.y > y) {
                Min.y = y;
            }
            if (Max.x < x) {
                Max.x = x;
            }
            if (Max.y < y) {
                Max.y = y;
            }
        }

        /// <summary>
        /// Grow / shrink bounds to all size with specified offset.
        /// </summary>
        /// <param name="sizeOffset">X/Y size offset</param>
        public void Expand (Vector2Int sizeOffset) {
            Expand (sizeOffset.x, sizeOffset.y);
        }

        /// <summary>
        /// Grow / shrink bounds to all size with specified offset.
        /// </summary>
        /// <param name="width">Width offset</param>
        /// <param name="height">Height offset</param>
        public void Expand (int width, int height) {
            Min.x -= width;
            Min.y -= height;
            Max.x += width;
            Max.y += height;
        }

        /// <summary>
        /// Is instance equals with specified one.
        /// </summary>
        /// <param name="rhs">Specified instance for comparation.</param>
        public override bool Equals (object rhs) {
            if (!(rhs is Bounds2i)) {
                return false;
            }
            return this == (Bounds2i) rhs;
        }

        /// <summary>
        /// Get hash code.
        /// </summary>
        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        /// <summary>
        /// Is specified bounds intersects with current one.
        /// </summary>
        /// <param name="bounds">Bounds</param>
        public bool Intersects (Bounds2i bounds) {
            return Min.x <= bounds.Max.x && Max.x >= bounds.Min.x && Min.y <= bounds.Max.y && Max.y >= bounds.Min.y;
        }

        /// <summary>
        /// Return formatted Min/Max values.
        /// </summary>
        public override string ToString () {
            return string.Format ("[{0},{1}]", Min, Max);
        }

        /// <summary>
        /// Translate bounds with relative offset.
        /// </summary>
        /// <param name="offset">X/Y offset</param>
        public void Translate (Vector2Int offset) {
            Min.x += offset.x;
            Min.y += offset.y;
            Max.x += offset.x;
            Max.y += offset.y;
        }

        /// <summary>
        /// Translate bounds with relative offset.
        /// </summary>
        /// <param name="x">X offset</param>
        /// <param name="y">Y offset</param>
        public void Translate (int x, int y) {
            Min.x += x;
            Min.y += y;
            Max.x += x;
            Max.y += y;
        }

        /// <summary>
        /// Validate bounds for proper min / max orientation.
        /// </summary>
        public void Validate () {
            int t;
            if (Min.x > Max.x) {
                t = Max.x;
                Max.x = Min.x;
                Min.x = t;
            }
            if (Min.y > Max.y) {
                t = Max.y;
                Max.y = Min.y;
                Min.y = t;
            }
        }

        /// <summary>
        /// Are specified bounds instersects. Optimized version.
        /// </summary>
        /// <param name="lhs">First bounds</param>
        /// <param name="rhs">Second bounds</param>
        public static bool Intersects (ref Bounds2i lhs, ref Bounds2i rhs) {
            return lhs.Min.x <= rhs.Max.x && lhs.Max.x >= rhs.Min.x && lhs.Min.y <= rhs.Max.y && lhs.Max.y >= rhs.Min.y;
        }

        public static bool operator == (Bounds2i lhs, Bounds2i rhs) {
            return lhs.Min.x == rhs.Min.x && lhs.Min.y == rhs.Min.y && lhs.Max.x == rhs.Max.x && lhs.Max.y == rhs.Max.y;
        }

        public static bool operator != (Bounds2i lhs, Bounds2i rhs) {
            return !(lhs == rhs);
        }
    }
}