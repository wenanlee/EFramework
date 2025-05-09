using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace EFramework.Unity.Command
{
    [ExecuteInEditMode] // 让脚本在 Editor 模式下运行
    public class CodeChangeListener : MonoBehaviour
    {
#if UNITY_EDITOR
        private FileSystemWatcher watcher;

        private void OnEnable()
        {
            if (!Application.isPlaying) // 确保只在 Editor 模式下运行
            {
                SetupFileWatcher();
            }
        }

        private void OnDisable()
        {
            if (watcher != null)
            {
                watcher.Dispose();
                watcher = null;
            }
        }

        private void SetupFileWatcher()
        {
            string assetsPath = Application.dataPath;
            watcher = new FileSystemWatcher(assetsPath, "*.cs");
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += OnCodeFileChanged;
            watcher.EnableRaisingEvents = true;

            Debug.Log("Started watching for code changes in: " + assetsPath);
        }

        private void OnCodeFileChanged(object sender, FileSystemEventArgs e)
        {
            // 由于 FileSystemWatcher 可能在非主线程触发，需要用 `EditorApplication.delayCall`
            EditorApplication.delayCall += () =>
            {
                Debug.Log($"Detected code change: {e.Name}");
                // 在这里执行你的扫描逻辑
                PerformScan();
            };
        }

        private void PerformScan()
        {
            Debug.Log("Performing scan due to code change...");
            // 你的扫描逻辑
        }
#endif
    }
}
