using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace EFramework.Debugger
{
    public enum ConsoleMode
    {
        Log,Profiler
    }
    [RequireComponent(typeof(ShowFPS))]
    class Console : MonoBehaviour
    {
        public static Version version = new Version(1, 0, 1);
        #region Inspector Settings

        /// <summary>
        /// The hotkey to show and hide the console window.
        /// </summary>
        public KeyCode toggleKey = KeyCode.BackQuote;

        /// <summary>
        /// Whether to open as soon as the game starts.
        /// </summary>
        public bool openOnStart = false;

        /// <summary>
        /// Whether to open the window by shaking the device (mobile-only).
        /// </summary>
        public bool shakeToOpen = true;

        /// <summary>
        /// The (squared) acceleration above which the window should open.
        /// </summary>
        public float shakeAcceleration = 3f;

        /// <summary>
        /// Whether to only keep a certain number of logs, useful if memory usage is a concern.
        /// </summary>
        public bool restrictLogCount = false;

        /// <summary>
        /// Number of logs to keep before removing old ones.
        /// </summary>
        public int maxLogCount = 1000;

        #endregion
        public ShowFPS fps;
        public ConsoleMode mode;
        private const int MBSize = 1024 * 1024;
        static readonly GUIContent clearLabel = new GUIContent("清除", "清除控制台所有日志");
        static readonly GUIContent collapseLabel = new GUIContent("折叠", "同样内容显示成一条");
        const int margin = 20;
        const string windowTitle = "控制台";

        static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        bool isCollapsed;
        bool isVisible;
        readonly List<Log> logs = new List<Log>();
        readonly ConcurrentQueue<Log> queuedLogs = new ConcurrentQueue<Log>();

        Vector2 scrollPosition;
        readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
        Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
        private string commandStr;
        readonly Dictionary<LogType, bool> logTypeFilters = new Dictionary<LogType, bool>
        {
            { LogType.Assert, true },
            { LogType.Error, true },
            { LogType.Exception, true },
            { LogType.Log, true },
            { LogType.Warning, true },
        };

        #region MonoBehaviour Messages

        void OnDisable()
        {
            Application.logMessageReceivedThreaded -= HandleLogThreaded;
        }

        void OnEnable()
        {
            Application.logMessageReceivedThreaded += HandleLogThreaded;
        }

        void OnGUI()
        {
            if (!isVisible)
            {
                return;
            }

            windowRect = GUILayout.Window(123456, windowRect, DrawWindow, windowTitle);
        }

        void Start()
        {
            fps = GetComponent<ShowFPS>();
            if (openOnStart)
            {
                isVisible = true;
            }
        }

        void Update()
        {
            UpdateQueuedLogs();

            if (Input.GetKeyUp(toggleKey))
            {
                isVisible = true;
            }

            if (shakeToOpen && Input.acceleration.sqrMagnitude > shakeAcceleration)
            {
                isVisible = true;
            }
        }

        #endregion

        void DrawCollapsedLog(Log log)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(log.GetTruncatedMessage());
            GUILayout.FlexibleSpace();
            GUILayout.Label(log.count.ToString(), GUI.skin.box);

            GUILayout.EndHorizontal();
        }

        void DrawExpandedLog(Log log)
        {
            for (var i = 0; i < log.count; i += 1)
            {
                GUILayout.Label(log.GetTruncatedMessage());
            }
        }

        void DrawLog(Log log)
        {
            GUI.contentColor = logTypeColors[log.type];

            if (isCollapsed)
            {
                DrawCollapsedLog(log);
            }
            else
            {
                DrawExpandedLog(log);
            }
        }

        void DrawLogList()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // Used to determine height of accumulated log labels.
            GUILayout.BeginVertical();

            var visibleLogs = logs.Where(IsLogVisible);

            foreach (Log log in visibleLogs)
            {
                DrawLog(log);
            }

            GUILayout.EndVertical();
            var innerScrollRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndScrollView();
            var outerScrollRect = GUILayoutUtility.GetLastRect();

            // If we're scrolled to bottom now, guarantee that it continues to be in next cycle.
            if (Event.current.type == EventType.Repaint && IsScrolledToBottom(innerScrollRect, outerScrollRect))
            {
                ScrollToBottom();
            }

            // Ensure GUI colour is reset before drawing other components.
            GUI.contentColor = Color.white;
        }
        void DrawProfiler()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();
            if (fps)
            {
                GUILayout.Label("FPS：" + fps.f_Fps.ToString("f2"));
            }
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSizeLong() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSizeLong() / 1000000 + "MB");
            GUILayout.EndVertical();
            var innerScrollRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndScrollView();
            var outerScrollRect = GUILayoutUtility.GetLastRect();

            // If we're scrolled to bottom now, guarantee that it continues to be in next cycle.
            if (Event.current.type == EventType.Repaint && IsScrolledToBottom(innerScrollRect, outerScrollRect))
            {
                ScrollToBottom();
            }

            // Ensure GUI colour is reset before drawing other components.
            GUI.contentColor = Color.white;
        }
        void DrawToolbar()
        {
            GUILayout.BeginHorizontal();
            //---------------------------------------------------
            GUI.FocusControl("Input");
            GUI.SetNextControlName("Input");
            commandStr = GUILayout.TextField(commandStr);
            Event e = Event.current;
            if (e.isKey && (e.keyCode == toggleKey))
            {
                commandStr = "";
                isVisible = false;
            }
            if (e.isKey && (e.keyCode == KeyCode.Return))
            {
                switch (commandStr.TrimEnd())
                {
                    case "close":
                        isVisible = false;
                        break;
                    case "clear":
                        logs.Clear();
                        break;
                    case "profiler":
                        mode = ConsoleMode.Profiler;
                        break;
                    case "log":
                        mode = ConsoleMode.Log;
                        break;
                    default:
                        Command.RunCommand(commandStr.TrimEnd());
                        break;
                }
                commandStr = "";
                GUI.FocusControl("Input");
            }
            //---------------------------------------------------
            if (GUILayout.Button(clearLabel, GUILayout.Width(80)))
            {
                logs.Clear();
            }

            foreach (LogType logType in Enum.GetValues(typeof(LogType)))
            {
                var currentState = logTypeFilters[logType];
                var label = logType.ToString();
                logTypeFilters[logType] = GUILayout.Toggle(currentState, label, GUILayout.ExpandWidth(false));
                GUILayout.Space(20);
            }

            isCollapsed = GUILayout.Toggle(isCollapsed, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        void DrawWindow(int windowID)
        {
            switch (mode)
            {
                case ConsoleMode.Log:
                    DrawLogList();
                    break;
                case ConsoleMode.Profiler:
                    DrawProfiler();
                    break;
                default:
                    break;
            }

            DrawToolbar();
            GUI.DragWindow(titleBarRect);
        }

        Log? GetLastLog()
        {
            if (logs.Count == 0)
            {
                return null;
            }

            return logs.Last();
        }

        void UpdateQueuedLogs()
        {
            Log log;
            while (queuedLogs.TryDequeue(out log))
            {
                ProcessLogItem(log);
            }
        }

        void HandleLogThreaded(string message, string stackTrace, LogType type)
        {
            var log = new Log
            {
                count = 1,
                message = message,
                stackTrace = stackTrace,
                type = type,
            };

            // Queue the log into a ConcurrentQueue to be processed later in the Unity main thread,
            // so that we don't get GUI-related errors for logs coming from other threads
            queuedLogs.Enqueue(log);
        }

        void ProcessLogItem(Log log)
        {
            var lastLog = GetLastLog();
            var isDuplicateOfLastLog = lastLog.HasValue && log.Equals(lastLog.Value);

            if (isDuplicateOfLastLog)
            {
                // Replace previous log with incremented count instead of adding a new one.
                log.count = lastLog.Value.count + 1;
                logs[logs.Count - 1] = log;
            }
            else
            {
                logs.Add(log);
                TrimExcessLogs();
            }
        }

        bool IsLogVisible(Log log)
        {
            return logTypeFilters[log.type];
        }

        bool IsScrolledToBottom(Rect innerScrollRect, Rect outerScrollRect)
        {
            var innerScrollHeight = innerScrollRect.height;

            // Take into account extra padding added to the scroll container.
            var outerScrollHeight = outerScrollRect.height - GUI.skin.box.padding.vertical;

            // If contents of scroll view haven't exceeded outer container, treat it as scrolled to bottom.
            if (outerScrollHeight > innerScrollHeight)
            {
                return true;
            }

            // Scrolled to bottom (with error margin for float math)
            return Mathf.Approximately(innerScrollHeight, scrollPosition.y + outerScrollHeight);
        }

        void ScrollToBottom()
        {
            scrollPosition = new Vector2(0, Int32.MaxValue);
        }

        void TrimExcessLogs()
        {
            if (!restrictLogCount)
            {
                return;
            }

            var amountToRemove = logs.Count - maxLogCount;

            if (amountToRemove <= 0)
            {
                return;
            }

            logs.RemoveRange(0, amountToRemove);
        }

    }

    struct Log
    {
        public int count;
        public string message;
        public string stackTrace;
        public LogType type;

        private const int MaxMessageLength = 16382;

        public bool Equals(Log log)
        {
            return message == log.message && stackTrace == log.stackTrace && type == log.type;
        }

        public string GetTruncatedMessage()
        {
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }

            return message.Length <= MaxMessageLength ? message : message.Substring(0, MaxMessageLength);
        }
    }

    public class ConcurrentQueue<T>
    {
        private readonly System.Object queueLock = new System.Object();
        private readonly Queue<T> queue = new Queue<T>();

        public void Enqueue(T item)
        {
            lock (queueLock)
            {
                queue.Enqueue(item);
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (queueLock)
            {
                if (queue.Count == 0)
                {
                    result = default(T);
                    return false;
                }

                result = queue.Dequeue();
                return true;
            }
        }
    }
}
