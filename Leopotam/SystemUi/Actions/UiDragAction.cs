// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiBeginDragAction.
    /// </summary>
    public struct UiBeginDragActionData {
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
    /// Event data of UiDragAction.
    /// </summary>
    public struct UiDragActionData {
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
    /// Event data of UiEndDragAction.
    /// </summary>
    public struct UiEndDragActionData {
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
    /// Ui action for processing OnBeginDrag / OnDrag / OnEndDrag events.
    /// </summary>
    public sealed class UiDragAction : UiActionBase, IBeginDragHandler, IDragHandler, IEndDragHandler {
        void IBeginDragHandler.OnBeginDrag (PointerEventData eventData) {
            var action = new UiBeginDragActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }

        void IDragHandler.OnDrag (PointerEventData eventData) {
            var action = new UiDragActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }

        void IEndDragHandler.OnEndDrag (PointerEventData eventData) {
            var action = new UiEndDragActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }
    }
}