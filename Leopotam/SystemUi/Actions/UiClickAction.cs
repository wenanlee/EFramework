// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiClickAction.
    /// </summary>
    public struct UiClickActionData {
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
    /// Ui action for processing OnClick events.
    /// </summary>
    public sealed class UiClickAction : UiActionBase, IPointerClickHandler {
        [Range (1f, 2048f)]
        public float DragTreshold = 5f;

        void IPointerClickHandler.OnPointerClick (PointerEventData eventData) {
            if ((eventData.pressPosition - eventData.position).sqrMagnitude < DragTreshold * DragTreshold) {
                var action = new UiClickActionData ();
                action.GroupId = GroupId;
                action.Sender = gameObject;
                action.EventData = eventData;
                SendActionData (action);
            }
        }
    }
}