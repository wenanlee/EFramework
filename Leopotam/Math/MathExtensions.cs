// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;

namespace EFramework.Math {
    /// <summary>
    /// Math extensions.
    /// </summary>
    public static class MathExtensions {
        static readonly StringBuilder _floatToStrBuf = new StringBuilder (64);

        static readonly string[] _shortNumberOrders = { "", "k", "M", "G", "T", "P", "E" };

        static readonly float _invLog1K = 1 / (float) System.Math.Log (1000);

        /// <summary>
        /// Convert number to string with "kilo-million-billion" suffix with rounding.
        /// </summary>
        /// <param name="data">Source number.</param>
        /// <param name="digitsAfterPoint">Digits after floating point.</param>
        public static string ToStringWithSuffix (this int data, int digitsAfterPoint = 2) {
            return ToStringWithSuffix ((long) data, digitsAfterPoint);
        }

        /// <summary>
        /// Convert number to string with "kilo-million-billion" suffix with rounding.
        /// </summary>
        /// <param name="data">Source number.</param>
        /// <param name="digitsAfterPoint">Digits after floating point.</param>
        public static string ToStringWithSuffix (this long data, int digitsAfterPoint = 2) {
            int sign;
            if (data < 0) {
                data = -data;
                sign = -1;
            } else {
                sign = 1;
            }

            var i = data > 0 ? (int) (System.Math.Floor (System.Math.Log (data) * _invLog1K)) : 0;
            if (i >= _shortNumberOrders.Length) {
                i = _shortNumberOrders.Length - 1;
            }
            var mask = digitsAfterPoint == 2 ? "0.##" : "0." + new string ('#', digitsAfterPoint);
            return (sign * data / System.Math.Pow (1000, i)).ToString (mask, NumberFormatInfo.InvariantInfo) + _shortNumberOrders[i];
        }

        /// <summary>
        /// Convert string to float with invariant culture.
        /// </summary>
        /// <returns>Float number.</returns>
        /// <param name="text">Source string.</param>
        public static float ToFloat (this string text) {
            return float.Parse (text, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Fast convert string to float. Fast, no GC allocation, no support for scientific format.
        /// </summary>
        /// <returns>Float number.</returns>
        /// <param name="text">Raw string.</param>
        public static float ToFloatUnchecked (this string text) {
            var retVal1 = 0f;
            var retVal2 = 0f;
            var sign = 1f;
            if (text != null) {
                var dir = 10f;
                int i;
                var iMax = text.Length;
                char c;
                for (i = 0; i < iMax; i++) {
                    c = text[i];
                    if (c >= '0' && c <= '9') {
                        retVal1 *= dir;
                        retVal1 += (c - '0');
                    } else {
                        if (c == '.') {
                            break;
                        } else {
                            if (c == '-') {
                                sign = -1f;
                            }
                        }
                    }
                }
                i++;
                dir = 0.1f;
                for (; i < iMax; i++) {
                    c = text[i];
                    if (c >= '0' && c <= '9') {
                        retVal2 += (c - '0') * dir;
                        dir *= 0.1f;
                    }
                }
            }
            return sign * (retVal1 + retVal2);
        }

        /// <summary>
        /// Convert float number to string. Fast, no support for scientific format.
        /// </summary>
        /// <returns>Normalized string.</returns>
        /// <param name="data">Data.</param>
        public static string ToStringFast (this float data) {
            lock (_floatToStrBuf) {
                const int precMul = 100000;
                _floatToStrBuf.Length = 0;
                var isNeg = data < 0f;
                if (isNeg) {
                    data = -data;
                }
                var v0 = (uint) data;
                var diff = (data - v0) * precMul;
                var v1 = (uint) diff;
                diff -= v1;
                if (diff > 0.5f) {
                    v1++;
                    if (v1 >= precMul) {
                        v1 = 0;
                        v0++;
                    }
                } else {
                    if (diff == 0.5f && (v1 == 0 || (v1 & 1) != 0)) {
                        v1++;
                    }
                }
                if (v1 > 0) {
                    var count = 5;
                    while ((v1 % 10) == 0) {
                        count--;
                        v1 /= 10;
                    }

                    do {
                        count--;
                        _floatToStrBuf.Append ((char) ((v1 % 10) + '0'));
                        v1 /= 10;
                    } while (v1 > 0);
                    while (count > 0) {
                        count--;
                        _floatToStrBuf.Append ('0');
                    }
                    _floatToStrBuf.Append ('.');
                }
                do {
                    _floatToStrBuf.Append ((char) ((v0 % 10) + '0'));
                    v0 /= 10;
                } while (v0 > 0);
                if (isNeg) {
                    _floatToStrBuf.Append ('-');
                }
                var i0 = 0;
                var i1 = _floatToStrBuf.Length - 1;
                char c;
                while (i1 > i0) {
                    c = _floatToStrBuf[i0];
                    _floatToStrBuf[i0] = _floatToStrBuf[i1];
                    _floatToStrBuf[i1] = c;
                    i0++;
                    i1--;
                }

                return _floatToStrBuf.ToString ();
            }
        }

        /// <summary>
        /// Get universal hash code for short string.
        /// </summary>
        /// <param name="str">String for hashing.</param>
        public static int GetStableHashCode (this string str) {
            if (str == null) {
                throw new ArgumentException ();
            }
            return GetStableHashCode (str, 0, str.Length);
        }

        /// <summary>
        /// Get universal hash code for part of short string.
        /// </summary>
        /// <param name="str">String for hashing.</param>
        /// <param name="offset">Start hashing from this offset.</param>
        /// <param name="len">Length of part for hashing.</param>
        public static int GetStableHashCode (this string str, int offset, int len) {
            if (str == null || offset < 0 || offset >= str.Length || len < 0 || offset + len > str.Length) {
                throw new ArgumentException ();
            }
            if (len == 0) {
                return 0;
            }
            var seed = 173;
            for (int i = offset, iMax = offset + len; i < iMax; i++) {
                seed = 37 * seed + str[i];
            }
            return seed;
        }

        /// <summary>
        /// Fill array with specified value.
        /// </summary>
        /// <param name="array">Target array.</param>
        /// <param name="value">Value.</param>
        /// <param name="length">Amount.</param>
        public static void Fill<T> (this T[] array, T value, int length) {
            if (array == null || array.Length < length) {
                throw new ArgumentException ();
            }
            if (length > 0) {
                array[0] = value;
                var lenHalf = length >> 1;
                int i;
                for (i = 1; i < lenHalf; i <<= 1) {
                    Array.Copy (array, 0, array, i, i);
                }
                Array.Copy (array, 0, array, i, length - i);
            }
        }
    }
}