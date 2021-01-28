// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.DataBinding.Binders {
    /// <summary>
    /// Data binding of progress value for any Slider component.
    /// </summary>
    [RequireComponent (typeof (Slider))]
    public sealed class DataBindSlider : AbstractBinderBase {
        Slider _target;

        protected override void ProcessBindedData (object data) {
            if ((object) _target == null) {
                _target = GetComponent<Slider> ();
            }
            _target.value = GetValueAsNumber (data);
        }
    }
}