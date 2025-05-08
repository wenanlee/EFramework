using EFramework.UnityCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFramework.Log
{
    /// <summary>
    /// Unity游戏内调试控制台
    /// 功能：实时显示日志、支持日志分类筛选、支持命令行输入
    /// 版本：1.1.2
    /// </summary>
    class Console : MonoSingleton<Console>
    {
        #region 控制台设置

        [Header("控制台显示设置")]
        [Tooltip("显示/隐藏控制台的快捷键")]
        public KeyCode toggleKey = KeyCode.Escape;

        [Tooltip("启动时自动打开控制台")]
        public bool openOnStart = false;

        [Header("移动设备设置")]
        [Tooltip("是否允许摇动设备打开控制台")]
        public bool shakeToOpen = true;

        [Tooltip("摇动时需要触摸屏幕才触发")]
        public bool shakeRequiresTouch = false;

        [Tooltip("触发控制台打开的加速度阈值")]
        public float shakeAcceleration = 3f;

        [Tooltip("切换显示状态的最小时间间隔(秒)")]
        public float toggleThresholdSeconds = .5f;

        [Header("日志显示设置")]
        [Tooltip("是否限制日志数量")]
        public bool restrictLogCount = false;

        [Tooltip("最大保留日志数量")]
        public int maxLogCount = 1000;

        [Tooltip("日志字体大小")]
        public int logFontSize = 12;

        [Tooltip("UI缩放比例")]
        public float scaleFactor = 1f;

        #endregion

        #region 常量与静态字段

        private const int margin = 20;
        private const string windowTitle = "控制台";
        private const int maxMessageLength = 16382; // Unity GUI Label最大支持长度
        
        private static readonly GUIContent clearLabel = new GUIContent("清空", "清除控制台所有内容");
        private static readonly GUIContent collapseLabel = new GUIContent("折叠", "合并重复的日志消息");
        
        private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        #endregion

        #region 运行时变量

        private bool isCollapsed = true;    // 是否折叠重复日志
        private bool isVisible;             // 控制台是否可见
        private float lastToggleTime;       // 上次切换时间
        private string commandText = string.Empty; // 命令行输入内容
        
        private readonly List<Log> logs = new List<Log>();                  // 日志列表
        private readonly ConcurrentQueue<Log> queuedLogs = new ConcurrentQueue<Log>(); // 线程安全日志队列
        private Vector2 scrollPosition;                                     // 滚动位置
        private readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);    // 标题栏区域
        private float windowX = margin;                                     // 窗口X位置
        private float windowY = margin;                                    // 窗口Y位置

        // 日志类型过滤器
        private readonly Dictionary<LogType, bool> logTypeFilters = new Dictionary<LogType, bool>
        {
            { LogType.Assert, true },
            { LogType.Error, true },
            { LogType.Exception, true },
            { LogType.Log, true },
            { LogType.Warning, false },
        };

        #endregion

        #region Unity生命周期方法

        private void OnEnable() => Application.logMessageReceivedThreaded += HandleLogThreaded;
        private void OnDisable() => Application.logMessageReceivedThreaded -= HandleLogThreaded;

        private void Start()
        {
            if (openOnStart) isVisible = true;
        }

        private void Update()
        {
            UpdateQueuedLogs(); // 处理队列中的日志
            CheckToggleInput(); // 检查切换输入
        }

        private void OnGUI()
        {
            if (!isVisible) return;

            // 应用UI缩放
            GUI.matrix = Matrix4x4.Scale(Vector3.one * scaleFactor);

            // 计算窗口大小和位置
            float width = (Screen.width / scaleFactor) - (margin * 2);
            float height = (Screen.height / scaleFactor) - (margin * 2);
            Rect windowRect = new Rect(windowX, windowY, width, height);

            // 绘制控制台窗口
            Rect newWindowRect = GUILayout.Window(123456, windowRect, DrawConsoleWindow, windowTitle);
            windowX = newWindowRect.x;
            windowY = newWindowRect.y;
        }

        #endregion

        #region 控制台核心功能

        /// <summary>
        /// 绘制控制台主窗口
        /// </summary>
        private void DrawConsoleWindow(int windowID)
        {
            DrawLogList();    // 绘制日志列表
            DrawToolbar();    // 绘制工具栏
            
            // 允许通过标题栏拖动窗口
            GUI.DragWindow(titleBarRect);
        }

        /// <summary>
        /// 绘制日志列表
        /// </summary>
        private void DrawLogList()
        {
            // 设置日志样式
            GUIStyle logStyle = new GUIStyle(GUI.skin.label) 
            { 
                fontSize = logFontSize 
            };
            
            GUIStyle badgeStyle = new GUIStyle(GUI.skin.box) 
            { 
                fontSize = logFontSize 
            };

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();

            // 筛选并显示可见日志
            foreach (var log in logs.Where(IsLogVisible))
            {
                DrawSingleLog(log, logStyle, badgeStyle);
            }

            GUILayout.EndVertical();
            var innerScrollRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndScrollView();
            var outerScrollRect = GUILayoutUtility.GetLastRect();

            // 如果已滚动到底部，保持底部位置
            if (Event.current.type == EventType.Repaint && IsScrolledToBottom(innerScrollRect, outerScrollRect))
            {
                ScrollToBottom();
            }
        }

        /// <summary>
        /// 绘制单个日志条目
        /// </summary>
        private void DrawSingleLog(Log log, GUIStyle logStyle, GUIStyle badgeStyle)
        {
            GUI.contentColor = logTypeColors[log.type];

            if (isCollapsed)
            {
                // 折叠模式：显示合并后的日志和计数
                GUILayout.BeginHorizontal();
                GUILayout.Label(log.GetTruncatedMessage(), logStyle);
                GUILayout.FlexibleSpace();
                GUILayout.Label(log.count.ToString(), badgeStyle);
                GUILayout.EndHorizontal();
            }
            else
            {
                // 展开模式：显示所有日志条目
                for (int i = 0; i < log.count; i++)
                {
                    GUILayout.Label(log.GetTruncatedMessage(), logStyle);
                }
            }

            GUI.contentColor = Color.white;
        }

        /// <summary>
        /// 绘制工具栏
        /// </summary>
        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal();
            
            DrawCommandInputField(); // 命令行输入框
            
            if (GUILayout.Button(clearLabel))
            {
                logs.Clear(); // 清空日志
            }

            // 日志类型筛选开关
            foreach (LogType logType in Enum.GetValues(typeof(LogType)))
            {
                logTypeFilters[logType] = GUILayout.Toggle(
                    logTypeFilters[logType], 
                    logType.ToString(), 
                    GUILayout.ExpandWidth(false));
                GUILayout.Space(20);
            }

            // 折叠开关
            isCollapsed = GUILayout.Toggle(isCollapsed, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制命令行输入框
        /// </summary>
        private void DrawCommandInputField()
        {
            GUI.SetNextControlName("command_text_field");
            commandText = GUILayout.TextField(
                commandText, 
                GUILayout.Width((Screen.width / scaleFactor) * 0.5f));
                
            GUI.FocusControl("command_text_field");

            if (Event.current.type == EventType.KeyUp)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Return:
                        ExecuteCommand();
                        break;
                    case KeyCode.Escape:
                        GUI.FocusWindow(123456);
                        break;
                    case KeyCode.Tab:
                        AutoCompleteCommand();
                        break;
                    case KeyCode.UpArrow:
                        NavigateCommandHistory(-1);
                        break;
                    case KeyCode.DownArrow:
                        NavigateCommandHistory(1);
                        break;
                }
            }
        }

        #endregion

        #region 日志处理

        /// <summary>
        /// 多线程日志处理
        /// </summary>
        internal void HandleLogThreaded(string message, string stackTrace, LogType type)
        {
            queuedLogs.Enqueue(new Log
            {
                count = 1,
                message = message,
                stackTrace = stackTrace,
                type = type,
            });
        }

        /// <summary>
        /// 处理队列中的日志
        /// </summary>
        private void UpdateQueuedLogs()
        {
            while (queuedLogs.TryDequeue(out Log log))
            {
                ProcessLogItem(log);
            }
        }

        /// <summary>
        /// 处理单个日志项
        /// </summary>
        private void ProcessLogItem(Log log)
        {
            var lastLog = GetLastLog();
            bool isDuplicate = lastLog.HasValue && log.Equals(lastLog.Value);

            if (isDuplicate)
            {
                // 合并重复日志
                log.count = lastLog.Value.count + 1;
                logs[logs.Count - 1] = log;
            }
            else
            {
                logs.Add(log);
                TrimExcessLogs();
            }
        }

        /// <summary>
        /// 获取最后一条日志
        /// </summary>
        private Log? GetLastLog() => logs.Count > 0 ? logs.Last() : null;

        /// <summary>
        /// 检查日志是否可见
        /// </summary>
        private bool IsLogVisible(Log log) => logTypeFilters[log.type];

        /// <summary>
        /// 清理超出限制的日志
        /// </summary>
        private void TrimExcessLogs()
        {
            if (!restrictLogCount || logs.Count <= maxLogCount) return;
            logs.RemoveRange(0, logs.Count - maxLogCount);
        }

        #endregion

        #region 辅助功能

        /// <summary>
        /// 检查控制台切换输入
        /// </summary>
        private void CheckToggleInput()
        {
            float curTime = Time.realtimeSinceStartup;

            if (Input.GetKeyDown(toggleKey))
            {
                isVisible = !isVisible;
            }

            // 摇动设备打开控制台
            if (shakeToOpen &&
                Input.acceleration.sqrMagnitude > shakeAcceleration &&
                curTime - lastToggleTime >= toggleThresholdSeconds &&
                (!shakeRequiresTouch || Input.touchCount > 2))
            {
                isVisible = !isVisible;
                lastToggleTime = curTime;
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        private void ExecuteCommand()
        {
            UnityCommandLineManager.Instance.CommandParser(commandText);
            commandText = string.Empty;
        }

        /// <summary>
        /// 命令自动补全
        /// </summary>
        private void AutoCompleteCommand()
        {
            Debug.Log("命令自动补全功能");
            // 实现命令自动补全逻辑
        }

        /// <summary>
        /// 导航命令历史
        /// </summary>
        private void NavigateCommandHistory(int direction)
        {
            Debug.Log(direction > 0 ? "下一条历史命令" : "上一条历史命令");
            // 实现命令历史导航逻辑
        }

        /// <summary>
        /// 检查是否滚动到底部
        /// </summary>
        private bool IsScrolledToBottom(Rect innerScrollRect, Rect outerScrollRect)
        {
            float innerScrollHeight = innerScrollRect.height;
            float outerScrollHeight = outerScrollRect.height - GUI.skin.box.padding.vertical;

            return outerScrollHeight > innerScrollHeight || 
                   Mathf.Approximately(innerScrollHeight, scrollPosition.y + outerScrollHeight);
        }

        /// <summary>
        /// 滚动到底部
        /// </summary>
        private void ScrollToBottom() => scrollPosition = new Vector2(0, Int32.MaxValue);

        #endregion
    }

    #region 辅助类

    /// <summary>
    /// 日志数据结构
    /// </summary>
    struct Log
    {
        public int count;         // 重复次数
        public string message;   // 日志消息
        public string stackTrace;// 堆栈跟踪
        public LogType type;     // 日志类型

        public bool Equals(Log log) => 
            message == log.message && 
            stackTrace == log.stackTrace && 
            type == log.type;

        /// <summary>
        /// 获取截断后的消息(避免Unity GUI长度限制)
        /// </summary>
        public string GetTruncatedMessage() => 
            string.IsNullOrEmpty(message) ? 
                message : 
                message.Length <= 16382 ? 
                    message : 
                    message.Substring(0, 16382);
    }

    /// <summary>
    /// 线程安全队列(简化版)
    /// </summary>
    class ConcurrentQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly object queueLock = new object();

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
                    result = default;
                    return false;
                }

                result = queue.Dequeue();
                return true;
            }
        }
    }

    #endregion
}