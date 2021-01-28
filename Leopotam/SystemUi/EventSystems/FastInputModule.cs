// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace EFramework.SystemUi.EventSystems {
#if UNITY_EDITOR
    [AddComponentMenu ("Event/LeopotamGroup/Fast Input Module")]
#endif
    [RequireComponent (typeof (EventSystem))]
    public class FastInputModule : PointerInputModule {
        public float inputActionsPerSecond {
            get { return _inputActionsPerSecond; }
            set { _inputActionsPerSecond = value; }
        }

        public float repeatDelay {
            get { return _repeatDelay; }
            set { _repeatDelay = value; }
        }

        public string horizontalAxis {
            get { return _horizontalAxis; }
            set { _horizontalAxis = value; }
        }

        public string verticalAxis {
            get { return _verticalAxis; }
            set { _verticalAxis = value; }
        }

        public string submitButton {
            get { return _submitButton; }
            set { _submitButton = value; }
        }

        public string cancelButton {
            get { return _cancelButton; }
            set { _cancelButton = value; }
        }

        [SerializeField]
        string _horizontalAxis = "Horizontal";

        [SerializeField]
        string _verticalAxis = "Vertical";

        [SerializeField]
        string _submitButton = "Submit";

        [SerializeField]
        string _cancelButton = "Cancel";

        [SerializeField]
        float _inputActionsPerSecond = 10;

        [SerializeField]
        float _repeatDelay = 0.5f;

        [SerializeField]
        bool _useMiddleMouseButton = false;

        [SerializeField]
        bool _useRightMouseButton = false;

        readonly MouseState _mouseState = new MouseState ();

        float _prevActionTime;

        Vector2 _lastMoveVector;

        int _consecutiveMoveCount;

        bool _shouldActivate;

        public override bool IsModuleSupported () {
            return Input.mousePresent || Input.touchSupported;
        }

        protected override void OnEnable () {
            base.OnEnable ();
            _shouldActivate = true;
        }

        protected override void OnDisable () {
            _shouldActivate = false;
            base.OnDisable ();
        }

        public override bool ShouldActivateModule () {
            return _shouldActivate;
        }

        public override void ActivateModule () {
            base.ActivateModule ();

            var toSelect = eventSystem.currentSelectedGameObject;
            if ((object) toSelect == null) {
                toSelect = eventSystem.firstSelectedGameObject;
            }

            eventSystem.SetSelectedGameObject (toSelect, GetBaseEventData ());
        }

        public override void DeactivateModule () {
            base.DeactivateModule ();
            ClearSelection ();
        }

        public override void Process () {
            var usedEvent = SendUpdateEventToSelectedObject ();

            if (eventSystem.sendNavigationEvents) {
                if (!usedEvent) {
                    usedEvent |= SendMoveEventToSelectedObject ();
                }

                if (!usedEvent) {
                    SendSubmitEventToSelectedObject ();
                }
            }

            ProcessMouseEvent ();
        }

        protected bool SendSubmitEventToSelectedObject () {
            if ((object) eventSystem.currentSelectedGameObject == null) {
                return false;
            }

            var data = GetBaseEventData ();
            if (Input.GetButtonDown (_submitButton)) {
                ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);
            } else {
                if (Input.GetButtonDown (_cancelButton)) {
                    ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
                }
            }
            return data.used;
        }

        void GetRawMoveVector (out Vector2 move, out bool isAllowed) {
            move.x = Input.GetAxisRaw (_horizontalAxis);
            move.y = Input.GetAxisRaw (_verticalAxis);
            isAllowed = false;

            if (Input.GetButtonDown (_horizontalAxis)) {
                isAllowed = true;
                if (move.x < 0f) {
                    move.x = -1f;
                } else {
                    if (move.x > 0f) {
                        move.x = 1f;
                    }
                }
            }
            if (Input.GetButtonDown (_verticalAxis)) {
                isAllowed = true;
                if (move.y < 0f) {
                    move.y = -1f;
                } else {
                    if (move.y > 0f) {
                        move.y = 1f;
                    }
                }
            }
        }

        protected bool SendMoveEventToSelectedObject () {
            Vector2 move;
            bool allow;
            GetRawMoveVector (out move, out allow);
            if (move.x > -float.Epsilon && move.x < float.Epsilon && move.y > -float.Epsilon && move.y < float.Epsilon) {
                _consecutiveMoveCount = 0;
                return false;
            }

            var time = Time.unscaledTime;
            var similarDir = move.x * _lastMoveVector.x + move.y * _lastMoveVector.y > 0;
            if (!allow) {
                allow = similarDir && _consecutiveMoveCount == 1 ?
                    time > (_prevActionTime + _repeatDelay) :
                    time > (_prevActionTime + 1f / _inputActionsPerSecond);
            }
            if (allow) {
                var axisEventData = GetAxisEventData (move.x, move.y, 0.6f);
                ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
                if (!similarDir) {
                    _consecutiveMoveCount = 0;
                }
                _consecutiveMoveCount++;
                _prevActionTime = time;
                _lastMoveVector = move;
                return axisEventData.used;
            }
            return false;
        }

        protected override MouseState GetMousePointerEventData (int id) {
            PointerEventData pointerEventData;
            var pointerData = GetPointerData (-1, out pointerEventData, true);
            pointerEventData.Reset ();
            if (pointerData) {
                pointerEventData.position = Input.mousePosition;
            }
            Vector2 vector = Input.mousePosition;
            if (Cursor.lockState == CursorLockMode.Locked) {
                pointerEventData.position = -Vector2.one;
                pointerEventData.delta = Vector2.zero;
            } else {
                pointerEventData.delta = vector - pointerEventData.position;
                pointerEventData.position = vector;
            }
            pointerEventData.scrollDelta = Input.mouseScrollDelta;
            pointerEventData.button = PointerEventData.InputButton.Left;
            eventSystem.RaycastAll (pointerEventData, m_RaycastResultCache);
            var pointerCurrentRaycast = FindFirstRaycast (m_RaycastResultCache);
            pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
            m_RaycastResultCache.Clear ();

            _mouseState.SetButtonState (PointerEventData.InputButton.Left, StateForMouseButton (0), pointerEventData);

            if (_useRightMouseButton) {
                PointerEventData pointerEventData2;
                GetPointerData (-2, out pointerEventData2, true);
                CopyFromTo (pointerEventData, pointerEventData2);
                pointerEventData2.button = PointerEventData.InputButton.Right;
                _mouseState.SetButtonState (PointerEventData.InputButton.Right, StateForMouseButton (1), pointerEventData2);
            }
            if (_useMiddleMouseButton) {
                PointerEventData pointerEventData3;
                GetPointerData (-3, out pointerEventData3, true);
                CopyFromTo (pointerEventData, pointerEventData3);
                pointerEventData3.button = PointerEventData.InputButton.Middle;
                _mouseState.SetButtonState (PointerEventData.InputButton.Middle, StateForMouseButton (2), pointerEventData3);
            }
            return _mouseState;
        }

        protected void ProcessMouseEvent () {
            var mouseData = GetMousePointerEventData (0);
            var leftButtonData = mouseData.GetButtonState (PointerEventData.InputButton.Left).eventData;

            // Process the first mouse button fully
            ProcessMousePress (leftButtonData);
            ProcessMove (leftButtonData.buttonData);
            ProcessDrag (leftButtonData.buttonData);

            // Now process right / middle clicks
#if !(UNITY_ANDROID || UNITY_IOS)
            if (_useRightMouseButton) {
                ProcessMousePress (mouseData.GetButtonState (PointerEventData.InputButton.Right).eventData);
                ProcessDrag (mouseData.GetButtonState (PointerEventData.InputButton.Right).eventData.buttonData);
            }
            if (_useMiddleMouseButton) {
                ProcessMousePress (mouseData.GetButtonState (PointerEventData.InputButton.Middle).eventData);
                ProcessDrag (mouseData.GetButtonState (PointerEventData.InputButton.Middle).eventData.buttonData);
            }
#endif

            if (leftButtonData.buttonData.scrollDelta.sqrMagnitude > float.Epsilon) {
                var scrollHandler =
                    ExecuteEvents.GetEventHandler<IScrollHandler> (leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.ExecuteHierarchy (scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
            }
        }

        protected bool SendUpdateEventToSelectedObject () {
            if ((object) (eventSystem.currentSelectedGameObject) != null) {
                var data = GetBaseEventData ();
                ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
                return data.used;
            }
            return false;
        }

        protected void ProcessMousePress (MouseButtonEventData data) {
            var pointerEvent = data.buttonData;
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // PointerDown notification
            if (data.PressedThisFrame ()) {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged (currentOverGo, pointerEvent);

                var newPressed = ExecuteEvents.ExecuteHierarchy (currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if ((object) newPressed == null) {
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler> (currentOverGo);
                }

                // Debug.Log("Pressed: " + newPressed);

                var time = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress) {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f) {
                        pointerEvent.clickCount++;
                    } else {
                        pointerEvent.clickCount = 1;
                    }

                    //                    pointerEvent.clickTime = time;
                } else {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;

                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler> (currentOverGo);

                if (pointerEvent.pointerDrag != null) {
                    ExecuteEvents.Execute (pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
                }
            }

            // PointerUp notification
            if (data.ReleasedThisFrame ()) {
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute (pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler> (currentOverGo);
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick) {
                    ExecuteEvents.Execute (pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                } else {
                    if (pointerEvent.pointerDrag != null && pointerEvent.dragging) {
                        ExecuteEvents.ExecuteHierarchy (currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
                    }
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging) {
                    ExecuteEvents.Execute (pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
                }

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                if (currentOverGo != pointerEvent.pointerEnter) {
                    HandlePointerExitAndEnter (pointerEvent, null);
                    HandlePointerExitAndEnter (pointerEvent, currentOverGo);
                }
            }
        }
    }
}