// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.Common {
    /// <summary>
    /// Replacement for MonoBehaviour class with transform caching.
    /// </summary>
    public abstract class MonoBehaviourBase : MonoBehaviour {
        /// <summary>
        /// Patched transform, gains 2x performance boost compare to standard.
        /// </summary>
        /// <value>The transform.</value>
        public new Transform transform {
            get {
                if ((object) CachedTransform == null) {
                    CachedTransform = base.transform;
                }
                return CachedTransform;
            }
        }

        /// <summary>
        /// Internal cached transform. Dont be fool to overwrite it, no protection for additional 2x performance boost.
        /// </summary>
        protected Transform CachedTransform;

        protected virtual void Awake () {
            if ((object) CachedTransform == null) {
                CachedTransform = base.transform;
            }
        }
    }
}