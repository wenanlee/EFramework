// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Common {
    /// <summary>
    /// Helper for cache Resources.Load calls with 2x performance boost.
    /// </summary>
    sealed class ResourceCache : MonoBehaviourService<ResourceCache> {
        readonly Dictionary<string, Object> _cache = new Dictionary<string, Object> (512);

        /// <summary>
        /// Return loaded resource from cache or load it. Important: if you request resource with one type,
        /// you cant get it for same path and different type.
        /// </summary>
        /// <param name="path">Path to loadable resource relative to "Resources" folder.</param>
        public T Load<T> (string path) where T : Object {
            Object asset;
            if (!_cache.TryGetValue (path, out asset)) {
                asset = Resources.Load<T> (path);
                if (asset != null) {
                    _cache[path] = asset;
                }
            }
            return asset as T;
        }

        /// <summary>
        /// Force unload resource. Use carefully.
        /// </summary>
        /// <param name="path">Path to loadable resource relative to "Resources" folder.</param>
        public void Unload (string path) {
            Object asset;
            if (_cache.TryGetValue (path, out asset)) {
                _cache.Remove (path);
                Resources.UnloadAsset (asset);
            }
        }

        protected override void OnCreateService () { }

        protected override void OnDestroyService () {
            _cache.Clear ();
        }
    }
}