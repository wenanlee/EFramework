// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Common;
using EFramework.Events;
using UnityEngine;

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Base class for ui action.
    /// </summary>
    public abstract class UiActionBase : MonoBehaviour {
        [SerializeField]
        string _group;

        protected int GroupId { get; private set; }

        protected virtual void Awake () {
            SetGroup (_group);
        }

        protected virtual void Start () {
            // Force create eventbus object.
            Service<UnityEventBus>.Get ();
        }

        protected void SendActionData<T> (T data) {
            if (Service<UnityEventBus>.IsRegistered) {
                Service<UnityEventBus>.Get ().Publish<T> (data);
            }
        }

        public void SetGroup (string group) {
            _group = group;
            GroupId = _group.GetUiActionGroupId ();
        }
    }
}