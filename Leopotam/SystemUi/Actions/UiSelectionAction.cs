// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Event data of UiSelectAction.
    /// </summary>
    public struct UiSelectActionData {
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
        public BaseEventData EventData;
    }

    /// <summary>
    /// Event data of UiDeselectAction.
    /// </summary>
    public struct UiDeselectActionData {
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
        public BaseEventData EventData;
    }

    /// <summary>
    /// Ui action for processing OnSelect / OnDeselect events.
    /// </summary>
    public sealed class UiSelectionAction : UiActionBase, ISelectHandler, IDeselectHandler {
        void IDeselectHandler.OnDeselect (BaseEventData eventData) {
            var action = new UiDeselectActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }

        void ISelectHandler.OnSelect (BaseEventData eventData) {
            var action = new UiSelectActionData ();
            action.GroupId = GroupId;
            action.Sender = gameObject;
            action.EventData = eventData;
            SendActionData (action);
        }
    }
}