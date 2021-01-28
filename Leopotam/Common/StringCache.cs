// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

namespace EFramework.Common {
    /// <summary>
    /// Custom string.Intern realization with formatting support.
    /// </summary>
    public sealed class StringCache<T> {
        readonly Dictionary<T, string> _cache;

        readonly string _format;

        readonly IFormatProvider _formatter;

        /// <summary>
        /// Create new instance of strings cache.
        /// </summary>
        /// <param name="format">Format of final string (same as for string.Format call). If null - "{0}" will be used.</param>
        /// <param name="formatter">Key data formatter. if null - NumberFormatInfo.InvariantInfo will be used.</param>
        /// <param name="capacity">Init capacity for storage. If not specified - 128 will be used.</param>
        public StringCache (string format = null, IFormatProvider formatter = null, int capacity = 128) {
            _format = string.IsNullOrEmpty (format) ? "{0}" : format;
            _formatter = formatter ?? NumberFormatInfo.InvariantInfo;
            _cache = new Dictionary<T, string> (capacity);
        }

        /// <summary>
        /// Get cached string for specified key.
        /// </summary>
        /// <param name="key">Key data.</param>
        public string Get (T key) {
            string retVal;
            if (!_cache.TryGetValue (key, out retVal)) {
                retVal = string.Format (_formatter, _format, key);
                _cache[key] = retVal;
            }
            return retVal;
        }
    }
}