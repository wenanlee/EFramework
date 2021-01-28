// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Scripting.Internal;

namespace EFramework.Scripting {
    /// <summary>
    /// Script engine VM. Internal class.
    /// </summary>
    sealed class ScriptVm {
        readonly Scanner _scanner;

        readonly Parser _parser;

        /// <summary>
        /// Is current execution of code stopped at script function calling.
        /// This flag uses for checking / protecting from endless recursive calls.
        /// </summary>
        /// <value><c>true</c> if in function call; otherwise, <c>false</c>.</value>
        public bool InFunctionCall { get; private set; }

        /// <summary>
        /// Default initialization.
        /// </summary>
        public ScriptVm () {
            InFunctionCall = false;
            _scanner = new Scanner ();
            _parser = new Parser (this, _scanner);
        }

        /// <summary>
        /// Show line info in error lines. True by default.
        /// </summary>
        /// <param name="state"></param>
        public void ShowLineInfo (bool state) {
            _parser.ShowLineInfo = state;
        }

        /// <summary>
        /// Load script source code. Old Vm state will be reset.
        /// </summary>
        /// <param name="source">Source.</param>
        public string Load (string source) {
            InFunctionCall = false;
            if (string.IsNullOrEmpty (source)) {
                return "no source code";
            }
            var err = _scanner.Load (source);
            if (err != null) {
                return err;
            }
            _parser.Vars.Reset ();
            err = _parser.Parse ();
            if (err != null) {
                return err;
            }
            return null;
        }

        /// <summary>
        /// Raise runtime error at VM.
        /// </summary>
        /// <param name="msg">Message.</param>
        public void SetRuntimeError (string msg) {
            _parser.SemErr (msg);
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
            ScriptVar? param1 = null, ScriptVar? param2 = null,
            ScriptVar? param3 = null, ScriptVar? param4 = null) {
            var undef = new ScriptVar ();
            if (InFunctionCall) {
                result = undef;
                return "already in function call";
            }
            if (!_parser.Vars.IsFunctionExists (funcName)) {
                result = undef;
                return string.Format ("function '{0}' not found", funcName);
            }
            InFunctionCall = true;
            var func = _parser.Vars.GetFunction (funcName);
            _scanner.PC = func.Pc;
            _parser.Vars.ResetVars ();
            var id = 0;
            var max = func.Params != null ? func.Params.Length : 0;
            if (param1 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param1.Value);
            }
            if (param2 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param2.Value);
            }
            if (param3 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param3.Value);
            }
            if (param4 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param4.Value);
            }
            for (; id < max; id++) {
                _parser.Vars.RegisterVar (func.Params[id], undef);
            }
            var err = _parser.CallFunction ();
            result = _parser.RetVal;
            InFunctionCall = false;
            return err;
        }

        /// <summary>
        /// Register host function to VM (publish to calling from script side).
        /// </summary>
        /// <param name="funcName">Func name.</param>
        /// <param name="cb">Cb.</param>
        public void RegisterHostFunction (string funcName, HostFunction cb) {
            _parser.Vars.RegisterHostFunction (funcName, cb);
        }

        /// <summary>
        /// Unregister all host functions from VM (unpublish from script side).
        /// </summary>
        public void UnregisterAllHostFunctions () {
            _parser.Vars.UnregisterHostFunctions ();
        }

        /// <summary>
        /// Get parameters count passed from script to current host function.
        /// </summary>
        /// <returns>The parameters count.</returns>
        public int GetParamsCount () {
            return _parser.CallParams.Count - _parser.CallParamsOffset;
        }

        /// <summary>
        /// Get specified parameter that was passed from script to current host function.
        /// </summary>
        /// <returns>Requested parameter or ScriptVar.Undefined.</returns>
        /// <param name="id">Number of parameter.</param>
        public ScriptVar GetParamById (int id) {
            return id >= 0 && id < GetParamsCount () ? _parser.CallParams[_parser.CallParamsOffset + id] : new ScriptVar ();
        }

        /// <summary>
        /// Is script function exists.
        /// </summary>
        /// <param name="funcName">Function name.</param>
        public bool IsFunctionExists (string funcName) {
            return funcName != null && _parser.Vars.IsFunctionExists (funcName);
        }
    }
}