// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.DataBinding.Binders {
    /// <summary>
    /// Data binding of text value for any Text component.
    /// </summary>
    [RequireComponent (typeof (Text))]
    public sealed class DataBindText : AbstractBinderBase {
        Text _target;

        protected override void ProcessBindedData (object data) {
            if ((object) _target == null) {
                _target = GetComponent<Text> ();
            }
            _target.text = GetValueAsString (data);
        }
    }
}