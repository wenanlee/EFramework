// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Common;

namespace EFramework.Events {
    /// <summary>
    /// Event bus service, local for current scene.
    /// </summary>
    sealed class UnityEventBus : MonoBehaviourService<UnityEventBus> {
        EventBus _eb;

        protected override void OnCreateService () {
            _eb = new EventBus ();
        }

        protected override void OnDestroyService () {
            UnsubscribeAndClearAllEvents ();
        }

        /// <summary>
        /// Subscribe callback to be raised on specific event.
        /// </summary>
        /// <param name="eventAction">Callback.</param>
        public void Subscribe<T> (EventBus.EventHandler<T> eventAction) {
            _eb.Subscribe (eventAction);
        }

        /// <summary>
        /// Unsubscribe callback.
        /// </summary>
        /// <param name="eventAction">Event action.</param>
        /// <param name="keepEvent">GC optimization - clear only callback list and keep event for future use.</param>
        public void Unsubscribe<T> (EventBus.EventHandler<T> eventAction, bool keepEvent = false) {
            _eb.Unsubscribe (eventAction, keepEvent);
        }

        /// <summary>
        /// Unsubscribe all callbacks from event.
        /// </summary>
        /// <param name="keepEvent">GC optimization - clear only callback list and keep event for future use.</param>
        public void UnsubscribeAll<T> (bool keepEvent = false) {
            _eb.UnsubscribeAll<T> (keepEvent);
        }

        /// <summary>
        /// Unsubscribe all listeneres and clear all events.
        /// </summary>
        public void UnsubscribeAndClearAllEvents () {
            _eb.UnsubscribeAndClearAllEvents ();
        }

        /// <summary>
        /// Publish event.
        /// </summary>
        /// <param name="eventMessage">Event message.</param>
        public void Publish<T> (T eventMessage) {
            _eb.Publish (eventMessage);
        }
    }
}