// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiDropAction.
    /// </summary>
    public struct UiDropActionData {
        /// <summary>
        /// Logical group for filtering events.
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Event sender.
        /// </summary>
        public GameObject Sender;

        /// <summary>
        /// Event data from uGui.
        /// </summary>
        public PointerEventData EventData;
    }

    /// <summary>
    /// Ui action for processing OnDrop events.
    /// </summary>
    public sealed class UiDropAction : UiActionBase, IDropHandler {
        void IDropHandler.OnDrop (PointerEventData eventData) {
            var action = new UiDropActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }
    }
}