// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Globalization;
using EFramework.Common;
using UnityEngine;

namespace EFramework.SystemUi.DataBinding.Binders {
    /// <summary>
    /// Base class for all binders, provide helpers, automatic subscription in 2 ways.
    /// </summary>
    public abstract class AbstractBinderBase : MonoBehaviour, IDataBinder {
        [SerializeField]
        string _source = null;

        [SerializeField]
        string _property = null;

        object _dirtyData;

        /// <summary>
        /// Receive events only in enabled state or always.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ProcessEventsOnlyWhenEnabled { get { return true; } }

        public string BindedSource { get { return _source; } }

        public string BindedProperty { get { return _property; } }

        static bool IsTypeNumeric (Type type) {
            switch (Type.GetTypeCode (type)) {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.Boolean:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Convert object to float value. If cant - zero will be returned.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        protected float GetValueAsNumber (object obj) {
            if (obj != null && IsTypeNumeric (obj.GetType ())) {
                return Convert.ToSingle (obj);
            }
            return 0f;
        }

        /// <summary>
        /// Convert object to bool value. If cant - false will be returned.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        protected bool GetValueAsBool (object obj) {
            return System.Math.Abs (GetValueAsNumber (obj)) > 0f;
        }

        /// <summary>
        /// Convert object to string value.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        protected string GetValueAsString (object obj, IFormatProvider customFormatProvider = null) {
            if (obj == null) {
                return null;
            }
            if (obj is IConvertible) {
                (obj as IConvertible).ToString (customFormatProvider ?? NumberFormatInfo.InvariantInfo);
            }
            return obj.ToString ();
        }

        void Awake () {
            Subscribe ();
        }

        void OnDestroy () {
            Unsubscribe ();
        }

        void LateUpdate () {
            if (ProcessEventsOnlyWhenEnabled) {
                ProcessBindedData (_dirtyData);
            }
            enabled = false;
        }

        void Subscribe () {
            if (!string.IsNullOrEmpty (_source) && !string.IsNullOrEmpty (_property)) {
                var storage = Service<DataStorage>.Get ();
                storage.Subscribe (this);
                enabled = ProcessEventsOnlyWhenEnabled;
                if (!ProcessEventsOnlyWhenEnabled) {
                    ProcessBindedData (storage.GetData (_source, _property));
                }
            }
        }

        void Unsubscribe () {
            if (!string.IsNullOrEmpty (_source) && !string.IsNullOrEmpty (_property)) {
                if (Service<DataStorage>.IsRegistered) {
                    Service<DataStorage>.Get ().Unsubscribe (this);
                }
            }
        }

        void IDataBinder.OnBindedDataChanged (object data) {
            _dirtyData = data;
            var isDelayedUpdate = ProcessEventsOnlyWhenEnabled;
            enabled = isDelayedUpdate;
            if (!isDelayedUpdate) {
                ProcessBindedData (_dirtyData);
            }
        }

        /// <summary>
        /// Raise when binded data should be validate / visualize.
        /// </summary>
        /// <param name="data">New value.</param>
        protected abstract void ProcessBindedData (object data);
    }
}