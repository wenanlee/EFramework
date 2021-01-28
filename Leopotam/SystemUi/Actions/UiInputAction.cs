// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiInputAction onChange callback.
    /// </summary>
    public struct UiInputChangeActionData {
        /// <summary>
        /// Logical group for filtering events.
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Event sender.
        /// </summary>
        public InputField Sender;

        /// <summary>
        /// New value.
        /// </summary>
        public string Value;
    }

    /// <summary>
    /// Event data of UiInputAction onEnd callback.
    /// </summary>
    public struct UiInputEndActionData {
        /// <summary>
        /// Logical group for filtering events.
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Event sender.
        /// </summary>
        public InputField Sender;

        /// <summary>
        /// New value.
        /// </summary>
        public string Value;
    }

    /// <summary>
    /// Ui action for processing InputField events.
    /// </summary>
    [RequireComponent (typeof (InputField))]
    public sealed class UiInputAction : UiActionBase {
        InputField _input;

        protected override void Awake () {
            base.Awake ();
            _input = GetComponent<InputField> ();
            _input.onValueChanged.AddListener (OnInputValueChanged);
            _input.onEndEdit.AddListener (OnInputEnded);
        }

        void OnInputValueChanged (string value) {
            var action = new UiInputChangeActionData ();
            action.GroupId = GroupId;
            action.Sender = _input;
            action.Value = value;
            SendActionData (action);
        }

        void OnInputEnded (string value) {
            var action = new UiInputEndActionData ();
            action.GroupId = GroupId;
            action.Sender = _input;
            action.Value = value;
            SendActionData (action);
        }
    }
}