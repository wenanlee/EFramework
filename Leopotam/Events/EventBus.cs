// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Events {
    /// <summary>
    /// EventBus implementation.
    /// </summary>
    public sealed class EventBus {
        /// <summary>
        /// Prototype for subscribers action.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public delegate void EventHandler<T> (T eventData);

        const int MaxCallDepth = 5;

        readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate> (32);

        int _eventsInCall;

        /// <summary>
        /// Subscribe callback to be raised on specific event.
        /// </summary>
        /// <param name="eventAction">Callback.</param>
        public void Subscribe<T> (EventHandler<T> eventAction) {
            if (eventAction != null) {
                var eventType = typeof (T);
                Delegate rawList;
                _events.TryGetValue (eventType, out rawList);
                _events[eventType] = (rawList as EventHandler<T>) + eventAction;
            }
        }

        /// <summary>
        /// Unsubscribe callback.
        /// </summary>
        /// <param name="eventAction">Event action.</param>
        /// <param name="keepEvent">GC optimization - clear only callback list and keep event for future use.</param>
        public void Unsubscribe<T> (EventHandler<T> eventAction, bool keepEvent = false) {
            if (eventAction != null) {
                var eventType = typeof (T);
                Delegate rawList;
                if (_events.TryGetValue (eventType, out rawList)) {
                    var list = (rawList as EventHandler<T>) - eventAction;
                    if (list == null && !keepEvent) {
                        _events.Remove (eventType);
                    } else {
                        _events[eventType] = list;
                    }
                }
            }
        }

        /// <summary>
        /// Unsubscribe all callbacks from event.
        /// </summary>
        /// <param name="keepEvent">GC optimization - clear only callback list and keep event for future use.</param>
        public void UnsubscribeAll<T> (bool keepEvent = false) {
            var eventType = typeof (T);
            Delegate rawList;
            if (_events.TryGetValue (eventType, out rawList)) {
                if (keepEvent) {
                    _events[eventType] = null;
                } else {
                    _events.Remove (eventType);
                }
            }
        }

        /// <summary>
        /// Unsubscribe all listeneres and clear all events.
        /// </summary>
        public void UnsubscribeAndClearAllEvents () {
            _events.Clear ();
        }

        /// <summary>
        /// Publish event.
        /// </summary>
        /// <param name="eventMessage">Event message.</param>
        public void Publish<T> (T eventMessage) {
            if (_eventsInCall >= MaxCallDepth) {
#if UNITY_EDITOR
                Debug.LogError ("Max call depth reached");
#endif
                return;
            }
            var eventType = typeof (T);
            Delegate rawList;
            _events.TryGetValue (eventType, out rawList);
            var list = rawList as EventHandler<T>;
            if (list != null) {
                _eventsInCall++;
                try {
                    list (eventMessage);
                } catch (Exception ex) {
                    Debug.LogError (ex);
                }
                _eventsInCall--;
            }
        }
    }
}