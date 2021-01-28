// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using EFramework.Collections;
using UnityEngine;

namespace EFramework.Common {
    /// <summary>
    /// Helper for use yield waiters without GC.
    /// </summary>
    public static class Yields {
        /// <summary>
        /// Get WaitForEndOfFrame yield instruction. Yes, its null. :)
        /// </summary>
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = null;

        /// <summary>
        /// Get WaitForFixedUpdate yield instruction.
        /// </summary>
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate ();

        static readonly Dictionary<float, FastList<IEnumerator>> _waitForSeconds = new Dictionary<float, FastList<IEnumerator>> (4);

        /// <summary>
        /// Get WaitForSeconds yield instruction.
        /// Important: - you should never cache / reuse received instance, request brand new one for each task instead!
        /// </summary>
        /// <param name="seconds">Delay in seconds.</param>
        public static IEnumerator WaitForSeconds (float seconds) {
            FastList<IEnumerator> list;
            if (!_waitForSeconds.TryGetValue (seconds, out list)) {
                list = new FastList<IEnumerator> (4);
                _waitForSeconds[seconds] = list;
            }
            IEnumerator retVal;
            if (list.Count > 0) {
                retVal = list[list.Count - 1];
                list.RemoveLast ();
            } else {
                retVal = new CustomWaitForSeconds (seconds, list);
            }
            retVal.Reset ();
            return retVal;
        }

        /// <summary>
        /// Get WaitForSeconds instance without auto-pooling on end. Can be cached / reused.
        /// </summary>
        /// <param name="seconds">Delay in seconds.</param>
        public static IEnumerator GetWaitForSecondsInstance (float seconds) {
            return new CustomWaitForSeconds (seconds, null);
        }

        /// <summary>
        /// Custom WaitForSeconds implementation for support pooling / reset.
        /// </summary>
        sealed class CustomWaitForSeconds : IEnumerator {
            float _delay;

            IList<IEnumerator> _poolList;

            float _endTime;

            object IEnumerator.Current { get { return null; } }

            bool IEnumerator.MoveNext () {
                if (Time.time < _endTime) {
                    return true;
                }
                (this as IEnumerator).Reset ();
                if (_poolList != null) {
                    _poolList.Add (this);
                }
                return false;
            }

            void IEnumerator.Reset () {
                _endTime = Time.time + _delay;
            }

            public CustomWaitForSeconds (float seconds, IList<IEnumerator> poolList) {
                _delay = seconds;
                _poolList = poolList;
                (this as IEnumerator).Reset ();
            }
        }
    }
}