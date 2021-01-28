// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.Common {
    /// <summary>
    /// Unity class extensions.
    /// </summary>
    public static class UnityExtensions {
        /// <summary>
        /// Broadcast method with data to all active GameObjects.
        /// </summary>
        /// <param name="method">Method name.</param>
        /// <param name="data">Optional data.</param>
        public static void BroadcastToAll (string method, object data = null) {
            var list = Object.FindObjectsOfType<MonoBehaviour> ();
            for (var i = list.Length - 1; i >= 0; i--) {
                list[i].SendMessage (method, data, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// Ensure that GameObject have component.
        /// </summary>
        /// <returns>Wanted component.</returns>
        /// <param name="go">Target GameObject.</param>
        /// <typeparam name="T">Any unity-based component.</typeparam>
        public static T EnsureGetComponent<T> (this GameObject go) where T : Component {
            if ((object) go == null) {
                return null;
            }
            var c = go.GetComponent<T> ();
            if ((object) c == null) {
                c = go.AddComponent<T> ();
            }
            return c;
        }

        /// <summary>
        /// Find GameObject with name in recursive hierarchy.
        /// </summary>
        /// <returns>Transform of found GameObject.</returns>
        /// <param name="target">Root of search.</param>
        /// <param name="name">Name to search.</param>
        public static Transform FindRecursive (this Transform target, string name) {
            if ((object) target == null || string.CompareOrdinal (target.name, name) == 0) {
                return target;
            }
            Transform retVal = null;
            for (var i = target.childCount - 1; i >= 0; i--) {
                retVal = target.GetChild (i).FindRecursive (name);
                if ((object) retVal != null) {
                    break;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Find GameObject with tag in recursive hierarchy.
        /// </summary>
        /// <returns>Transform of found GameObject.</returns>
        /// <param name="target">Root of search.</param>
        /// <param name="tag">Tag to search.</param>
        public static Transform FindRecursiveByTag (this Transform target, string tag) {
            if ((object) target == null || target.CompareTag (tag)) {
                return target;
            }
            Transform retVal = null;
            for (var i = target.childCount - 1; i >= 0; i--) {
                retVal = target.GetChild (i).FindRecursiveByTag (tag);
                if ((object) retVal != null) {
                    break;
                }
            }
            return retVal;
        }
    }
}