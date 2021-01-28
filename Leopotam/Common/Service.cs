// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace EFramework.Common {
    /// <summary>
    /// Attribute for limit usage of MonoBehaviourService-classes at specified scenes only.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
    [System.Diagnostics.Conditional ("UNITY_EDITOR")]
    sealed class MonoBehaviourServiceFilterAttribute : Attribute {
        public string Name;

        public MonoBehaviourServiceFilterAttribute (string name) {
            Name = name;
        }
    }

    /// <summary>
    /// MonoBehaviour base class for service locator pattern.
    /// Warning: Touching services at any Awake() method will lead to undefined behaviour!
    /// </summary>
    public abstract class MonoBehaviourService<T> : MonoBehaviour where T : class {
        void Awake () {
            if (Service<T>.IsRegistered) {
                DestroyImmediate (this);
                return;
            }
#if UNITY_EDITOR
            var type = GetType ();
            // check for allowed scenes if possible.
            var attrs = type.GetCustomAttributes (typeof (MonoBehaviourServiceFilterAttribute), true);
            if (attrs.Length > 0) {
                var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
                var i = attrs.Length - 1;
                for (; i >= 0; i--) {
                    if (System.Text.RegularExpressions.Regex.IsMatch (
                            sceneName, ((MonoBehaviourServiceFilterAttribute) attrs[i]).Name)) {
                        break;
                    }
                }
                if (i == -1) {
                    throw new UnityException (
                        string.Format ("\"{0}\" service cant be used at scene \"{1}\"", type.Name, sceneName));
                }
            }
#endif
            Service<T>.Register (this as T);
            OnCreateService ();
        }

        void OnDestroy () {
            if (Service<T>.IsRegistered) {
                OnDestroyService ();
                Service<T>.Unregister (this as T);
            }
        }

        /// <summary>
        /// Replacement of Awake method, will be raised only once for service.
        /// Dont use Awake method in inherited classes!
        /// </summary>
        protected abstract void OnCreateService ();

        /// <summary>
        /// Replacement of OnDestroy method, will be raised only once for service.
        /// Dont use OnDestroy method in inherited classes!
        /// </summary>
        protected abstract void OnDestroyService ();
    }

    /// <summary>
    /// Service locator pattern implementation.
    /// </summary>
    public static class Service<T> where T : class {
        static T _instance;

        /// <summary>
        /// Is instance of service created.
        /// </summary>
        public static bool IsRegistered { get { return (object) _instance != null; } }

        /// <summary>
        /// Get service instance of generic type with lazy initialization.
        /// Warning: Touching MonoBehaviourService<T>-services at any Awake() method will lead to undefined behaviour!
        /// </summary>
        public static T Get () {
            // Unwrap IsRegistered for performance.
            if ((object) _instance != null) {
                return _instance;
            }
            var type = typeof (T);
            // Special case for unity scriptable objects.
            if (type.IsSubclassOf (typeof (ScriptableObject))) {
                var list = Resources.FindObjectsOfTypeAll (type);
                if (list == null || list.Length == 0) {
                    throw new UnityException (
                        string.Format ("Service<{0}>.Get() can be used only with exists / loaded asset of this type", type.Name));
                }
                _instance = list[0] as T;
                return _instance;
            }
            // Not allow any unity components except wrapped to special class.
#if UNITY_EDITOR
            if (type.IsSubclassOf (typeof (Component)) && !type.IsSubclassOf (typeof (MonoBehaviourService<T>))) {
                throw new UnityException (string.Format ("\"{0}\" - invalid type, should be inherited from MonoBehaviourService", type.Name));
            }
#endif
            // Special case for MonoBehaviourService<T> components.
            if (type.IsSubclassOf (typeof (MonoBehaviourService<T>))) {
#if UNITY_EDITOR
                if (!Application.isPlaying) {
                    throw new UnityException (string.Format ("Service<{0}>.Get() can be used only at PLAY mode", type.Name));
                }
#endif
                new GameObject (
#if UNITY_EDITOR
                    "_SERVICE_" + type.Name
#endif
                ).AddComponent (type);
            } else {
                Register (Activator.CreateInstance (type) as T);
            }
            return _instance;
        }

        /// <summary>
        /// Register instance as service.
        /// </summary>
        /// <param name="instance">Service instance.</param>
        public static void Register (T instance) {
            if (IsRegistered) {
                throw new UnityException (string.Format (
                    "Cant register \"{0}\" as service - type already registered", typeof (T).Name));
            }
            if (instance == null) {
                throw new UnityException ("Cant register null instance as service");
            }
            _instance = instance;
        }

        /// <summary>
        /// Unregister service instance.
        /// </summary>
        /// <param name="instance">Instance for unregister.</param>
        /// <param name="force">Force unregister instance even on invalid input instance.</param>
        public static void Unregister (T instance, bool force = false) {
            if (instance == _instance || force) {
                _instance = null;
            }
        }
    }
}