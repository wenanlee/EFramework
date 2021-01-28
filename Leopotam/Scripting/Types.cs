// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using EFramework.Math;

namespace EFramework.Scripting {
    /// <summary>
    /// Type of ScriptVm variable.
    /// </summary>
    enum ScriptVarType {
        Undefined = 0,

        String,

        Number
    }

    /// <summary>
    /// ScriptVm variable.
    /// </summary>
    struct ScriptVar {
        /// <summary>
        /// Type of variable.
        /// </summary>
        public ScriptVarType Type { get; private set; }

        /// <summary>
        /// Initialization from float value as ScriptVarType.Number.
        /// </summary>
        /// <param name="data">Data.</param>
        public ScriptVar (float data) : this () {
            AsNumber = data;
        }

        /// <summary>
        /// Initialization from string value as ScriptVarType.String.
        /// </summary>
        /// <param name="data">Data.</param>
        public ScriptVar (string data) : this () {
            AsString = data;
        }

        /// <summary>
        /// Get state of variable as string.
        /// On getting of number value it will be converted to string without casting error.
        /// </summary>
        public string AsString {
            get { return IsUndefined ? "undefined" : (IsNumber ? _asNumber.ToStringFast () : _asString); }
            set {
                Type = ScriptVarType.String;
                _asString = value;
            }
        }

        /// <summary>
        /// Get value of variable as number, otherwise 0.
        /// </summary>
        public float AsNumber {
            get { return IsNumber ? _asNumber : 0f; }
            set {
                Type = ScriptVarType.Number;
                _asNumber = value;
            }
        }

        /// <summary>
        /// Is variable type is ScriptVarType.Undefined.
        /// </summary>
        public bool IsUndefined { get { return Type == ScriptVarType.Undefined; } }

        /// <summary>
        /// Is variable type is ScriptVarType.Number.
        /// </summary>
        public bool IsNumber { get { return Type == ScriptVarType.Number; } }

        /// <summary>
        /// Is variable type is ScriptVarType.String.
        /// </summary>
        public bool IsString { get { return Type == ScriptVarType.String; } }

        /// <summary>
        /// Set variable as ScriptVarType.Undefined.
        /// </summary>
        public void SetUndefined () {
            Type = ScriptVarType.Undefined;
        }

        string _asString;

        float _asNumber;
    }

    /// <summary>
    /// Function desc. Internal class - holder of function parameter names for calling script function from host.
    /// </summary>
    sealed class FunctionDesc {
        public int Pc;

        public string[] Params;
    }

    /// <summary>
    /// Variables state of VM. For internal useage
    /// </summary>
    sealed class Vars {
        readonly Dictionary<string, FunctionDesc> _functions = new Dictionary<string, FunctionDesc> (16);

        readonly Dictionary<string, ScriptVar> _vars = new Dictionary<string, ScriptVar> (128);

        readonly Dictionary<string, HostFunction> _hostFuncs = new Dictionary<string, HostFunction> (128);

        readonly ScriptVm _vm;

        /// <summary>
        /// Initialization for specified ScriptVm.
        /// </summary>
        public Vars (ScriptVm vm) {
            _vm = vm;
        }

        /// <summary>
        /// Reset internal state (script functions, variables). Published host functions will be kept.
        /// </summary>
        public void Reset () {
            _functions.Clear ();
            ResetVars ();
        }

        /// <summary>
        /// Reset script variables.
        /// </summary>
        public void ResetVars () {
            _vars.Clear ();
        }

        /// <summary>
        /// Is script variable exists.
        /// </summary>
        /// <param name="varName">Variable name.</param>
        public bool IsVarExists (string varName) {
            return _vars.ContainsKey (varName);
        }

        /// <summary>
        /// Create / update script variable with value.
        /// </summary>
        /// <param name="varName">Variable name.</param>
        /// <param name="v">V.</param>
        public void RegisterVar (string varName, ScriptVar v) {
            _vars[varName] = v;
        }

        /// <summary>
        /// Get script variable without checking of existence - be careful on it!
        /// </summary>
        /// <returns>Variable value.</returns>
        /// <param name="varName">Variable name.</param>
        public ScriptVar GetVar (string varName) {
            return _vars[varName];
        }

        /// <summary>
        /// Is script function exists.
        /// </summary>
        /// <param name="funcName">Func name.</param>
        public bool IsFunctionExists (string funcName) {
            return _functions.ContainsKey (funcName);
        }

        /// <summary>
        /// Register script function during parsing. For internal use.
        /// </summary>
        /// <param name="funcName">Function name.</param>
        /// <param name="pc">PC counter at script-s lexem stream.</param>
        /// <param name="paramList">Parameters list.</param>
        public void RegisterFunction (string funcName, int pc, List<string> paramList) {
            var desc = new FunctionDesc {
                Pc = pc - 1,
            };
            if (paramList.Count > 0) {
                desc.Params = paramList.ToArray ();
            }
            _functions[funcName] = desc;
        }

        /// <summary>
        /// Get function description without checking of existence - be careful on it!
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="varName">Variable name.</param>
        public FunctionDesc GetFunction (string varName) {
            return _functions[varName];
        }

        /// <summary>
        /// is host function exists.
        /// </summary>
        /// <param name="funcName">Function name.</param>
        public bool IsHostFunctionExists (string funcName) {
            return _hostFuncs.ContainsKey (funcName);
        }

        /// <summary>
        /// Register host function (publish to script).
        /// </summary>
        /// <param name="funcName">Function name.</param>
        /// <param name="cb">Host callback.</param>
        public void RegisterHostFunction (string funcName, HostFunction cb) {
            _hostFuncs[funcName] = cb;
        }

        /// <summary>
        /// Unregister all host functions.
        /// </summary>
        public void UnregisterHostFunctions () {
            _hostFuncs.Clear ();
        }

        /// <summary>
        /// Call the host function without checking of existence - be careful on it! For internal use only!
        /// </summary>
        /// <returns>The host function.</returns>
        /// <param name="funcName">Func name.</param>
        public ScriptVar CallHostFunction (string funcName) {
            return _hostFuncs[funcName] (_vm);
        }
    }

    delegate ScriptVar HostFunction (ScriptVm vm);
}