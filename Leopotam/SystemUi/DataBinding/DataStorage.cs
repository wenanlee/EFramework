// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using EFramework.Collections;
using EFramework.Common;
using UnityEngine;

namespace EFramework.SystemUi.DataBinding {
    /// <summary>
    /// Data binding control center, route events to IDataBinder-s.
    /// </summary>
    public sealed class DataStorage : MonoBehaviourService<DataStorage> {
        readonly FastList<BindedSourceInfo> _sourcesList = new FastList<BindedSourceInfo> (128);

        BindedSourceInfo[] _sourcesData;

        int _sourcesCount;

        protected override void OnCreateService () {
            _sourcesData = _sourcesList.GetData (out _sourcesCount);
        }

        protected override void OnDestroyService () { }

        void OnDataChanged (IDataSource source, string property) {
            if (source == null || string.IsNullOrEmpty (property)) {
                return;
            }

            BindedPropertyInfo propInfo;
            int listenerCount;
            IDataBinder[] listeners;
            for (var i = _sourcesCount - 1; i >= 0; i--) {
                if (_sourcesData[i].Source == source) {
                    if (_sourcesData[i].Properties.TryGetValue (property, out propInfo)) {
                        listeners = propInfo.Listeners.GetData (out listenerCount);
                        if (listenerCount > 0) {
                            var data = GetData (_sourcesData[i].Name, property);
                            for (var j = listenerCount - 1; j >= 0; j--) {
                                listeners[j].OnBindedDataChanged (data);
                            }
                        }
                    }
                }
            }
        }

        BindedSourceInfo GetSource (string sourceName, bool createNew = false) {
            for (var i = _sourcesCount - 1; i >= 0; i--) {
                if (string.CompareOrdinal (_sourcesData[i].Name, sourceName) == 0) {
                    return _sourcesData[i];
                }
            }
            if (!createNew) {
                return null;
            }
            var res = new BindedSourceInfo ();
            res.Name = sourceName;
            _sourcesList.Add (res);
            _sourcesData = _sourcesList.GetData (out _sourcesCount);
            return res;
        }

        /// <summary>
        /// Subscribe binder to events receiving.
        /// </summary>
        /// <param name="binder">Binder instance.</param>
        public void Subscribe (IDataBinder binder) {
            if (binder == null) {
                throw new ArgumentException ("binder");
            }
            if (string.IsNullOrEmpty (binder.BindedSource)) {
                throw new ArgumentException ("binder.Source");
            }
            if (string.IsNullOrEmpty (binder.BindedProperty)) {
                throw new ArgumentException ("binder.Property");
            }

            var holder = GetSource (binder.BindedSource, true);
            BindedPropertyInfo propInfo;
            if (!holder.Properties.TryGetValue (binder.BindedProperty, out propInfo)) {
                propInfo = new BindedPropertyInfo ();
                holder.Properties[binder.BindedProperty] = propInfo;
            }
            if (!propInfo.Listeners.Contains (binder)) {
                propInfo.Listeners.Add (binder);
            }
        }

        /// <summary>
        /// Unsubscribe binder from events receiving.
        /// </summary>
        /// <param name="binder">Binder instance.</param>
        public void Unsubscribe (IDataBinder binder) {
            if (binder == null ||
                string.IsNullOrEmpty (binder.BindedSource) ||
                string.IsNullOrEmpty (binder.BindedProperty)) {
                return;
            }
            var holder = GetSource (binder.BindedSource);
            if (holder != null) {
                BindedPropertyInfo propInfo;
                if (holder.Properties.TryGetValue (binder.BindedProperty, out propInfo)) {
                    var idx = propInfo.Listeners.IndexOf (binder);
                    if (idx != -1) {
                        propInfo.Listeners.RemoveAt (idx);
                    }
                }
            }
        }

        /// <summary>
        /// Get value of specified property of specified logical source.
        /// </summary>
        /// <param name="sourceName">Logical name of source.</param>
        /// <param name="propertyName">Property name.</param>
        public object GetData (string sourceName, string propertyName) {
            var holder = GetSource (sourceName);
            if (holder != null && holder.Source != null) {
                BindedPropertyInfo propInfo;
                if (holder.Properties.TryGetValue (propertyName, out propInfo)) {
                    if (!propInfo.IsTypeChecked) {
                        var type = holder.Source.GetType ();
                        var member = (MemberInfo) type.GetProperty (propertyName);
                        if (member == null || !((PropertyInfo) member).CanRead) {
                            member = type.GetField (propertyName);
                        }
#if UNITY_EDITOR
                        if (member == null) {
                            Debug.LogWarningFormat ("[DataBinding] Cant get readable member \"{0}\" of source \"{1}\"",
                                propertyName, sourceName);
                        }
#endif
                        propInfo.MemberInfo = member;
                        propInfo.IsTypeChecked = true;
                    }
                    if (propInfo.MemberInfo != null) {
                        return propInfo.MemberInfo is PropertyInfo ?
                            ((PropertyInfo) propInfo.MemberInfo).GetValue (holder.Source, null) :
                            ((FieldInfo) propInfo.MemberInfo).GetValue (holder.Source);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Attach IDataSource as source with specified logical name.
        /// </summary>
        /// <param name="sourceName">Logical name of source.</param>
        /// <param name="source">Data source.</param>
        public void SetDataSource (string sourceName, IDataSource source) {
            if (string.IsNullOrEmpty (sourceName)) {
                throw new ArgumentException ("sourceName");
            }

            var holder = GetSource (sourceName, true);
            if (holder.Source != null) {
                holder.Source.OnBindedDataChanged -= OnDataChanged;
                holder.Source = null;

                // clear types cache.
                foreach (var pair in holder.Properties) {
                    pair.Value.IsTypeChecked = false;
                    pair.Value.MemberInfo = null;
                }
            }

            if (source != null) {
                holder.Source = source;
                source.OnBindedDataChanged += OnDataChanged;
                foreach (var pair in holder.Properties) {
                    OnDataChanged (source, pair.Key);
                }
            }
        }

        class BindedSourceInfo {
            public IDataSource Source;

            public string Name;

            public Dictionary<string, BindedPropertyInfo> Properties = new Dictionary<string, BindedPropertyInfo> (32);
        }

        class BindedPropertyInfo {
            public MemberInfo MemberInfo;

            public bool IsTypeChecked;

            public FastList<IDataBinder> Listeners = new FastList<IDataBinder> (32);

            public BindedPropertyInfo () {
                Listeners.UseCastToObjectComparer (true);
            }
        }
    }
}