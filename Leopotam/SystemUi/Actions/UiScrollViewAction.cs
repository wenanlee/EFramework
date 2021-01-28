// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiScrollViewAction.
    /// </summary>
    public struct UiScrollViewActionData {
        /// <summary>
        /// Logical group for filtering events.
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Event sender.
        /// </summary>
        public ScrollRect Sender;

        /// <summary>
        /// New value.
        /// </summary>
        public Vector2 Value;
    }

    /// <summary>
    /// Ui action for processing ScrollView events.
    /// </summary>
    [RequireComponent (typeof (ScrollRect))]
    public sealed class UiScrollViewAction : UiActionBase {
        ScrollRect _scrollView;

        protected override void Awake () {
            base.Awake ();
            _scrollView = GetComponent<ScrollRect> ();
            _scrollView.onValueChanged.AddListener (OnScrollViewValueChanged);
        }
        void OnScrollViewValueChanged (Vector2 value) {
            var action = new UiScrollViewActionData ();
            action.GroupId = GroupId;
            action.Sender = _scrollView;
            action.Value = value;
            SendActionData (action);
        }
    }
}