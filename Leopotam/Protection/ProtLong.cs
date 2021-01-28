// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace EFramework.Protection {
    /// <summary>
    /// Wrapper for int64 value protection.
    /// </summary>
    [StructLayout (LayoutKind.Explicit)]
    public struct ProtLong {
        /// <summary>
        /// Get encrypted value (for serialization or something else).
        /// </summary>
        /// <value>The encrypted value.</value>
        public long EncryptedValue {
            get {
                // Workaround for default struct constructor init.
                if (_conv == 0 && _encrypt == 0) {
                    _conv = XorMask;
                }
                return _encrypt;
            }
        }

        const ulong XorMask = 0xaaaaaaaaaaaaaaaa;

        [FieldOffset (0)]
        long _encrypt;

        [FieldOffset (0)]
        ulong _conv;

        public static implicit operator long (ProtLong v) {
            v._conv ^= XorMask;
            var f = v._encrypt;
            v._conv ^= XorMask;
            return f;
        }

        public static implicit operator ProtLong (long v) {
            var p = new ProtLong ();
            p._encrypt = v;
            p._conv ^= XorMask;
            return p;
        }
    }
}