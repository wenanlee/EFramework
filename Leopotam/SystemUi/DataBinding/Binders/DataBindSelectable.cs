// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.DataBinding.Binders {
    /// <summary>
    /// Data binding of enable / disable interactable state for any Selectable component.
    /// </summary>
    [RequireComponent (typeof (Selectable))]
    public sealed class DataBindSelectable : AbstractBinderBase {
        Selectable _target;

        protected override void ProcessBindedData (object data) {
            if ((object) _target == null) {
                _target = GetComponent<Selectable> ();
            }
            _target.interactable = GetValueAsBool (data);
        }
    }
}