// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiSliderAction.
    /// </summary>
    public struct UiSliderActionData {
        /// <summary>
        /// Logical group for filtering events.
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Event sender.
        /// </summary>
        public Slider Sender;

        /// <summary>
        /// New value.
        /// </summary>
        public float Value;
    }

    /// <summary>
    /// Ui action for processing Slider events.
    /// </summary>
    [RequireComponent (typeof (Slider))]
    public sealed class UiSliderAction : UiActionBase {
        Slider _slider;

        protected override void Awake () {
            base.Awake ();
            _slider = GetComponent<Slider> ();
            _slider.onValueChanged.AddListener (OnSliderValueChanged);
        }
        void OnSliderValueChanged (float value) {
            var action = new UiSliderActionData ();
            action.GroupId = GroupId;
            action.Sender = _slider;
            action.Value = value;
            SendActionData (action);
        }
    }
}