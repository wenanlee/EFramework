// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.SystemUi.DataBinding.Binders {
    /// <summary>
    /// Data binding of enable / disable state for any Component or GameObject.
    /// </summary>
    public sealed class DataBindEnable : AbstractBinderBase {
        /// <summary>
        /// Target component for enable / disable. If null - current gameObject will be used.
        /// </summary>
        [SerializeField]
        Behaviour _target = null;

        protected override bool ProcessEventsOnlyWhenEnabled { get { return false; } }

        protected override void ProcessBindedData (object data) {
            var state = GetValueAsBool (data);
            if (_target != null) {
                _target.enabled = state;
            } else {
                gameObject.SetActive (state);
            }
        }
    }
}