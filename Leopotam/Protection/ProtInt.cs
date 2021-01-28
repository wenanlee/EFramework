// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace EFramework.Protection {
    /// <summary>
    /// Wrapper for int32 value protection.
    /// </summary>
    [StructLayout (LayoutKind.Explicit)]
    public struct ProtInt {
        /// <summary>
        /// Get encrypted value (for serialization or something else).
        /// </summary>
        /// <value>The encrypted value.</value>
        public int EncryptedValue {
            get {
                // Workaround for default struct constructor init.
                if (_conv == 0 && _encrypt == 0) {
                    _conv = XorMask;
                }
                return _encrypt;
            }
        }

        const uint XorMask = 0xaaaaaaaa;

        [FieldOffset (0)]
        int _encrypt;

        [FieldOffset (0)]
        uint _conv;

        public static implicit operator int (ProtInt v) {
            v._conv ^= XorMask;
            var f = v._encrypt;
            v._conv ^= XorMask;
            return f;
        }

        public static implicit operator ProtInt (int v) {
            var p = new ProtInt ();
            p._encrypt = v;
            p._conv ^= XorMask;
            return p;
        }
    }
}