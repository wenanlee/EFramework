// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using EFramework.Common;
using UnityEngine;

namespace EFramework.Scripting {
    /// <summary>
    /// Script manager base class. Wrapper around ScriptVm instance, provides api from it.
    /// </summary>
    abstract class ScriptManagerBase<T> : MonoBehaviourService<T> where T : class {
        struct TimeoutPair {
            public float Time;

            public string Event;

            public ScriptVar? Param1;

            public ScriptVar? Param2;

            public ScriptVar? Param3;

            public ScriptVar? Param4;
        }

        readonly ScriptVm _vm = new ScriptVm ();

        readonly List<TimeoutPair> _timeoutListeners = new List<TimeoutPair> (16);

        protected override void OnCreateService () {
            OnAttachHostFunctions (_vm);
        }

        protected override void OnDestroyService () {
            OnResetEvents ();
        }

        /// <summary>
        /// Will be raised on attempt of publish host api methods to script engine.
        /// Override it to register custom api.
        /// </summary>
        /// <param name="vm">Script engine instance.</param>
        protected virtual void OnAttachHostFunctions (ScriptVm vm) {
            _vm.RegisterHostFunction ("callWithDelay", ApiCallWithDelay);
            _vm.RegisterHostFunction ("debug", ApiDebug);
        }

        /// <summary>
        /// Will be raised on error at runtime.
        /// Override it for custom behaviour (logging, interrupting, etc).
        /// </summary>
        /// <param name="errMsg">Error message.</param>
        protected virtual void OnRuntimeError (string errMsg) { }

        /// <summary>
        /// Will be raised on each tick in event loop.
        /// Override it to add custom behaviour to event loop.
        /// </summary>
        protected virtual void OnValidateEvents () {
            if (_timeoutListeners.Count > 0) {
                var time = Time.time;
                TimeoutPair pair;
                ScriptVar ret;
                string err;
                for (int i = _timeoutListeners.Count - 1; i >= 0; i--) {
                    if (_timeoutListeners[i].Time <= time) {
                        pair = _timeoutListeners[i];
                        err = _vm.CallFunction (pair.Event, out ret, pair.Param1, pair.Param2, pair.Param3, pair.Param4);
                        if (err != null) {
                            SetRuntimeError (err);
                            return;
                        }
                        _timeoutListeners.RemoveAt (i);
                    }
                }
            }
        }

        /// <summary>
        /// Will be raised on cleanup of delayed events.
        /// Override it to add custom behaviour.
        /// </summary>
        protected virtual void OnResetEvents () {
            _timeoutListeners.Clear ();
        }

        void LateUpdate () {
            OnValidateEvents ();
        }

        /// <summary>
        /// Cancel execution of all delayed events.
        /// </summary>
        public void ResetEvents () {
            OnResetEvents ();
        }

        /// <summary>
        /// Raise runtime error.
        /// </summary>
        /// <param name="errMsg">Error message.</param>
        public void SetRuntimeError (string errMsg) {
            if (errMsg != null) {
                ResetEvents ();
                OnRuntimeError (errMsg);
            }
        }

        /// <summary>
        /// Load and compile source code of script.
        /// </summary>
        /// <returns>Error of loading / compiling operations.</returns>
        /// <param name="sourceText">Source code.</param>
        public string LoadSource (string sourceText) {
            ResetEvents ();
            return _vm.Load (sourceText);
        }

        /// <summary>
        /// Call script function.
        /// </summary>
        /// <returns>Execution error.</returns>
        /// <param name="funcName">Function name.</param>
        /// <param name="result">Result of function execution.</param>
        /// <param name="param1">Optional parameter to function.</param>
        /// <param name="param2">Optional parameter to function.</param>
        /// <param name="param3">Optional parameter to function.</param>
        /// <param name="param4">Optional parameter to function.</param>
        public string CallFunction (string funcName, out ScriptVar result,
            ScriptVar? param1 = null, ScriptVar ? param2 = null,
            ScriptVar? param3 = null, ScriptVar ? param4 = null) {
            return _vm.CallFunction (funcName, out result, param1, param2, param3, param4);
        }

        /// <summary>
        /// Call script function or skip with no error if function not exists.
        /// </summary>
        /// <returns>Execution error.</returns>
        /// <param name="funcName">Function name.</param>
        /// <param name="result">Result of function execution.</param>
        /// <param name="param1">Optional parameter to function.</param>
        /// <param name="param2">Optional parameter to function.</param>
        /// <param name="param3">Optional parameter to function.</param>
        /// <param name="param4">Optional parameter to function.</param>
        public string CallFunctionOrSkip (string funcName, out ScriptVar result,
            ScriptVar? param1 = null, ScriptVar ? param2 = null,
            ScriptVar? param3 = null, ScriptVar ? param4 = null) {
            if (!_vm.IsFunctionExists (funcName)) {
                result = new ScriptVar ();
                return null;
            }
            return _vm.CallFunction (funcName, out result, param1, param2, param3, param4);
        }

        /// <summary>
        /// Call script function with delay.
        /// </summary>
        /// <param name="funcName">Function name.</param>
        /// <param name="timeout">Delay in seconds before calling function.</param>
        /// <param name="param1">Optional parameter to function.</param>
        /// <param name="param2">Optional parameter to function.</param>
        /// <param name="param3">Optional parameter to function.</param>
        /// <param name="param4">Optional parameter to function.</param>
        public void CallFunctionWithDelay (string funcName, float timeout,
            ScriptVar? param1 = null, ScriptVar ? param2 = null,
            ScriptVar? param3 = null, ScriptVar ? param4 = null) {
            var pair = new TimeoutPair {
            Event = funcName,
            Time = Time.time + timeout,
            Param1 = param1,
            Param2 = param2,
            Param3 = param3,
            Param4 = param4
            };
            _timeoutListeners.Add (pair);
        }

        /// <summary>
        /// Call script function with delay or skip with no error if function not exists.
        /// </summary>
        /// <param name="funcName">Function name.</param>
        /// <param name="timeout">Delay in seconds before calling function.</param>
        /// <param name="param1">Optional parameter to function.</param>
        /// <param name="param2">Optional parameter to function.</param>
        /// <param name="param3">Optional parameter to function.</param>
        /// <param name="param4">Optional parameter to function.</param>
        public void CallFunctionWithDelayOrSkip (string funcName, float timeout,
            ScriptVar? param1 = null, ScriptVar ? param2 = null,
            ScriptVar? param3 = null, ScriptVar ? param4 = null) {
            if (!_vm.IsFunctionExists (funcName)) {
                return;
            }
            CallFunctionWithDelay (funcName, timeout, param1, param2, param3, param4);
        }

        #region Common api

        ScriptVar ApiCallWithDelay (ScriptVm vm) {
            var count = vm.GetParamsCount ();
            var pTimeout = vm.GetParamById (0);
            var pEvent = vm.GetParamById (1);
            if (count < 2 || !pTimeout.IsNumber || !pEvent.IsString) {
                _vm.SetRuntimeError ("(nTimeout, sFuncName[, param1, param2]) parameters required");
                return new ScriptVar ();
            }
            ScriptVar? param1 = null;
            if (count > 2) {
                param1 = vm.GetParamById (2);
            }
            ScriptVar? param2 = null;
            if (count > 3) {
                param2 = vm.GetParamById (3);
            }
            ScriptVar? param3 = null;
            if (count > 4) {
                param3 = vm.GetParamById (4);
            }
            ScriptVar? param4 = null;
            if (count > 5) {
                param4 = vm.GetParamById (5);
            }

            CallFunctionWithDelay (pEvent.AsString, pTimeout.AsNumber, param1, param2, param3, param4);

            return new ScriptVar ();
        }

        ScriptVar ApiDebug (ScriptVm vm) {
            var str = string.Empty;
            for (int i = 0, iMax = vm.GetParamsCount (); i < iMax; i++) {
                str += " " + vm.GetParamById (i).AsString;
            }
            Debug.LogFormat ("[SCRIPT: {0}]{1}", Time.time, str);
            return new ScriptVar ();
        }

        #endregion
    }
}