// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiEnterAction.
    /// </summary>
    public struct UiEnterActionData {
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
    /// Event data of UiExitAction.
    /// </summary>
    public struct UiExitActionData {
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
    /// Ui action for processing OnEnter / OnExit events.
    /// </summary>
    public sealed class UiEnterExitAction : UiActionBase, IPointerEnterHandler, IPointerExitHandler {
        void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData) {
            var action = new UiEnterActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }

        void IPointerExitHandler.OnPointerExit (PointerEventData eventData) {
            var action = new UiExitActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }
    }
}