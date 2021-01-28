// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace EFramework.Protection {
    /// <summary>
    /// Wrapper for float value protection.
    /// </summary>
    [StructLayout (LayoutKind.Explicit)]
    public struct ProtFloat {
        /// <summary>
        /// Get encrypted value (for serialization or something else).
        /// </summary>
        public float EncryptedValue {
            get {
                // Workaround for default struct constructor init.
                if (_conv == 0 && ((int) _encrypt == 0)) {
                    _conv = XorMask;
                }
                return _encrypt;
            }
        }

        const uint XorMask = 0xaaaaaaaa;

        [FieldOffset (0)]
        float _encrypt;

        [FieldOffset (0)]
        uint _conv;

        public static implicit operator float (ProtFloat d) {
            d._conv ^= XorMask;
            var f = d._encrypt;
            d._conv ^= XorMask;
            return f;
        }

        public static implicit operator ProtFloat (float d) {
            var p = new ProtFloat ();
            p._encrypt = d;
            p._conv ^= XorMask;
            return p;
        }
    }
}