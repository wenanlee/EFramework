// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace EFramework.Math {
    /// <summary>
    /// Rng generator, XorShift based.
    /// </summary>
    public sealed class RngFast {
        const double InvMaxIntExOne = 1.0 / (int.MaxValue + 1.0);

        const double InvIntMax = 1.0 / int.MaxValue;

        uint _x;

        uint _y;

        uint _z;

        uint _w;

        /// <summary>
        /// Default initialization.
        /// </summary>
        public RngFast () : this (System.Environment.TickCount) { }

        /// <summary>
        /// Initialization with custom seed.
        /// </summary>
        /// <param name="seed">Seed.</param>
        public RngFast (int seed) {
            SetSeed (seed);
        }

        /// <summary>
        /// Set new seed.
        /// </summary>
        /// <param name="seed">Seed.</param>
        public void SetSeed (int seed) {
            _x = (uint) (seed * 1431655781 + seed * 1183186591 + seed * 622729787 + seed * 338294347);
            _y = 842502087;
            _z = 3579807591;
            _w = 273326509;
        }

        /// <summary>
        /// Get current internal state. Use on your risk!
        /// </summary>
        /// <param name="x">Data vector 1.</param>
        /// <param name="y">Data vector 2.</param>
        /// <param name="z">Data vector 3.</param>
        /// <param name="w">Data vector 4.</param>
        public void GetInternalState (out int x, out int y, out int z, out int w) {
            x = (int) _x;
            y = (int) _y;
            z = (int) _z;
            w = (int) _w;
        }

        /// <summary>
        /// Set current internal state. Use on your risk!
        /// </summary>
        /// <param name="x">Data vector 1.</param>
        /// <param name="y">Data vector 2.</param>
        /// <param name="z">Data vector 3.</param>
        /// <param name="w">Data vector 4.</param>
        public void SetInternalState (int x, int y, int z, int w) {
            _x = (uint) x;
            _y = (uint) y;
            _z = (uint) z;
            _w = (uint) w;
        }

        /// <summary>
        /// /// Get int32 random number from range [0, max).
        /// </summary>
        /// <returns>Random int32 value.</returns>
        public int GetInt (int max) {
            var t = _x ^ (_x << 11);
            _x = _y;
            _y = _z;
            _z = _w;
            return (int) ((InvMaxIntExOne * (int) (0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))))) * max);
        }

        /// <summary>
        /// Get int32 random number from range [min, max).
        /// </summary>
        /// <returns>Random int32 value.</returns>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value (excluded).</param>
        public int GetInt (int min, int max) {
            if (min >= max) {
                return min;
            }
            var t = _x ^ (_x << 11);
            _x = _y;
            _y = _z;
            _z = _w;
            return min + (int) ((InvMaxIntExOne *
                (int) (0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))))) * (max - min));
        }

        /// <summary>
        /// Get float random number from range [0, 1) or [0, 1] for includeOne=true.
        /// </summary>
        /// <param name="includeOne">Include 1 value for searching.</param>
        public float GetFloat (bool includeOne = true) {
            var t = _x ^ (_x << 11);
            _x = _y;
            _y = _z;
            _z = _w;
            return (float) ((includeOne ? InvIntMax : InvMaxIntExOne) *
                (int) (0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)))));
        }

        /// <summary>
        /// Get float random number from range [min, max) or [min, max] for includeMax=true.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="includeMax">Include max value for searching.</param>
        public float GetFloat (float min, float max, bool includeMax = true) {
            if (min >= max) {
                return min;
            }
            var t = _x ^ (_x << 11);
            _x = _y;
            _y = _z;
            _z = _w;
            return min + (float) ((includeMax ? InvIntMax : InvMaxIntExOne) *
                (int) (0x7fffffff & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)))) * (max - min));
        }
    }
}