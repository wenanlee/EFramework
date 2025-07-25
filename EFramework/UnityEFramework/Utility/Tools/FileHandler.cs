#if UNITY_EDITOR
using EFramework.Unity.UIFramework;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace EFramework.Unity.Utility
{
    public static class FileIOHelper
    {
        public static bool WriteScriptableObject<T>(string path, string fileName, T so) where T : ScriptableObject
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + path);
                AssetDatabase.Refresh();
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                AssetDatabase.CreateAsset(so, $"Assets/{path}/{fileName}");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return true;
        }
        /// <summary>
        /// 从文件读取字节数组
        /// </summary>
        public static byte[] ReadBytesFromFile(string path, string fileName)
        {
            string filePath = Path.Combine(path, fileName);
            Debug.Log($"读取文件路径: {filePath}");

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"文件不存在: {filePath}");
                return null;
            }

            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (IOException e)
            {
                Debug.LogError($"读取文件错误: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"未知错误: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 写入字节数组到文件（支持追加模式）
        /// </summary>
        public static bool WriteBytesToFile(byte[] bytes, string path, string fileName, bool append = false)
        {
            string filePath = Path.Combine(path, fileName);
            Debug.Log($"写入文件路径: {filePath}");

            try
            {
                using (var fs = new FileStream(
                    filePath,
                    append ? FileMode.Append : FileMode.Create,
                    FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                return true;
            }
            catch (IOException e)
            {
                Debug.LogError($"写入文件错误: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"未知错误: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从文件读取字符串内容（默认UTF-8编码）
        /// </summary>
        public static string ReadStringFromFile(string path, string fileName, Encoding encoding = null)
        {
            string filePath = Path.Combine(path, fileName);
            Debug.Log($"读取文件路径: {filePath}");

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"文件不存在: {filePath}");
                return null;
            }

            try
            {
                encoding = encoding ?? Encoding.UTF8;
                return File.ReadAllText(filePath, encoding);
            }
            catch (IOException e)
            {
                Debug.LogError($"读取文件错误: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"未知错误: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 写入字符串到文件（默认UTF-8编码，支持追加模式）
        /// </summary>
        public static bool WriteStringToFile(string content, string path, string fileName, bool append = false, Encoding encoding = null)
        {
            string filePath = Path.Combine(path, fileName);
            Debug.Log($"写入文件路径: {filePath}");

            try
            {
                encoding = encoding ?? Encoding.UTF8;

                if (append)
                {
                    File.AppendAllText(filePath, content, encoding);
                }
                else
                {
                    File.WriteAllText(filePath, content, encoding);
                }
                return true;
            }
            catch (IOException e)
            {
                Debug.LogError($"写入文件错误: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"未知错误: {e.Message}");
                return false;
            }
        }
    }
}
#endif