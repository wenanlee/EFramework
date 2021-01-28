// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.DataBinding.Binders {
    /// <summary>
    /// Data binding of enable / disable toggle state for any Toggle component.
    /// </summary>
    [RequireComponent (typeof (Toggle))]
    public sealed class DataBindToggle : AbstractBinderBase {
        Toggle _target;

        protected override void ProcessBindedData (object data) {
            if ((object) _target == null) {
                _target = GetComponent<Toggle> ();
            }
            _target.isOn = GetValueAsBool (data);
        }
    }
}