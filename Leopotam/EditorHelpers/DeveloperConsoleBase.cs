// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Common;
using EFramework.Events;
using EFramework.Scripting;
using EFramework.SystemUi.Actions;
using EFramework.SystemUi.Markup;
using UnityEngine;
using UnityEngine.UI;

namespace EFramework.EditorHelpers {
    /// <summary>
    /// Base class for developer UI console.
    /// </summary>
    [DefaultExecutionOrder (-32768)]
    abstract class DeveloperConsoleBase<T> : MonoBehaviourService<T> where T : class {
        /// <summary>
        /// Is console was shown.
        /// </summary>
        public bool IsVisible { get; private set; }

        const string MarkupSchema = "LeopotamGroup/EditorHelpers/DevConsole";

        const string MarkupTheme = "LeopotamGroup/EditorHelpers/DevConsoleTheme";

        const string MarkupScrollViewName = "logScroll";

        const string MarkupLogTextName = "logText";

        const string MarkupInputName = "input";

        const float CloseButtonWidth = 128f;

        ScriptVm _vm;

        MarkupContainer _markup;

        InputField _inputField;

        Text _logText;

        RectTransform _logScroll;

        string[] _logLines;

        int _linesCount;

        int _onDevConsoleId;

        protected override void OnCreateService () {
            _logLines = new string[GetMaxLines ()];
            _vm = new ScriptVm ();
            _vm.ShowLineInfo (false);
            OnRegisterFunctions (_vm);
            _onDevConsoleId = "DeveloperConsole".GetUiActionGroupId ();
            _markup = MarkupContainer.CreateMarkup (MarkupSchema);
            _markup.AttachTheme (Resources.Load<MarkupTheme> (MarkupTheme));
            _markup.CreateVisuals ();
            _markup.GetCanvas ().sortingOrder = short.MaxValue;
            _markup.gameObject.SetActive (false);

            var scrollView = _markup.GetNamedNode (MarkupScrollViewName).GetComponent<ScrollRect> ();
            _logScroll = scrollView.content;
            _logScroll.pivot = new Vector2 (0.5f, 0f);
            var pos = _logScroll.localPosition;
            pos.y -= scrollView.GetComponent<RectTransform> ().sizeDelta.y * 0.5f;
            _logScroll.localPosition = pos;
            _logText = _markup.GetNamedNode (MarkupLogTextName).GetComponent<Text> ();
            _logText.horizontalOverflow = HorizontalWrapMode.Overflow;
            _logText.verticalOverflow = VerticalWrapMode.Overflow;

            var inputTr = _markup.GetNamedNode (MarkupInputName);
            var offset = inputTr.offsetMax;
            offset.x = -CloseButtonWidth;
            inputTr.offsetMax = offset;
            _inputField = inputTr.GetComponent<InputField> ();

            var ueb = Service<UnityEventBus>.Get ();
            ueb.Subscribe<UiInputEndActionData> (OnInputEnd);
            ueb.Subscribe<UiClickActionData> (OnClose);
        }

        protected override void OnDestroyService () { }

        /// <summary>
        /// Get max amount of lines in log. Should be constant during all calls!
        /// </summary>
        protected virtual int GetMaxLines () {
            return 30;
        }

        void OnInputEnd (UiInputEndActionData arg) {
            if (arg.GroupId == _onDevConsoleId && Input.GetButton ("Submit")) {
                ExecuteCommand (arg.Value);
                _inputField.text = "";
                _inputField.ActivateInputField ();
            }
        }

        void OnClose (UiClickActionData arg) {
            Show (false);
        }

        /// <summary>
        /// Execute script code line. Code should be expression without function declaration, etc.
        /// </summary>
        /// <param name="value">Script code.</param>
        protected virtual void ExecuteCommand (string value) {
            if (!string.IsNullOrEmpty (value)) {
                AppendLine (LogType.Log, value);
                var err = _vm.Load (string.Format ("function _devConsoleMain(){{return {0};}}", value));
                if (!string.IsNullOrEmpty (err)) {
                    AppendLine (LogType.Warning, err);
                    return;
                }
                ScriptVar result;
                err = _vm.CallFunction ("_devConsoleMain", out result);
                if (!string.IsNullOrEmpty (err)) {
                    AppendLine (LogType.Warning, err);
                    return;
                }
                AppendLine (LogType.Log, result.AsString);
            }
        }

        /// <summary>
        /// Append message to log. Multiline message will be splitted automatically.
        /// </summary>
        /// <param name="type">Type of message (Log, Warning, Error).</param>
        /// <param name="line">Text of message.</param>
        protected virtual void AppendLine (LogType type, string line) {
            if (string.IsNullOrEmpty (line)) {
                return;
            }
            if (line.IndexOf ('\n') != -1) {
                var lines = line.Split ('\n');
                for (int i = 0; i < lines.Length; i++) {
                    AppendLine (type, lines[i]);
                }
                return;
            }
            if (type != LogType.Log) {
                line = string.Format ("> <color=\"{0}\">{1}</color>", type == LogType.Warning ? "yellow" : "red", line);
            } else {
                line = string.Format ("> {0}", line);
            }
            if (_linesCount == GetMaxLines () - 1) {
                _linesCount--;
                System.Array.Copy (_logLines, 1, _logLines, 0, _linesCount);
            }
            _logLines[_linesCount++] = line;
            _logText.text = string.Join ("\n", _logLines, 0, _linesCount);
            var size = _logScroll.sizeDelta;
            size.y = _logText.preferredHeight;
            _logScroll.sizeDelta = size;
        }

        /// <summary>
        /// Should be implemented for custom functions processing at console.
        /// For registering functions - use _vm.RegisterHostFunction() method.
        /// </summary>
        /// <param name="vm">Script engine instance.</param>
        protected abstract void OnRegisterFunctions (ScriptVm vm);

        /// <summary>
        /// Show / hide console.
        /// </summary>
        /// <param name="state">New state of visibility.</param>
        public virtual void Show (bool state) {
            if (state != IsVisible) {
                IsVisible = state;
                _markup.gameObject.SetActive (state);
                if (state) {
                    _inputField.ActivateInputField ();
                } else {
                    _inputField.text = string.Empty;
                }
            }
        }
    }
}