using System.IO;
using System.Text;
using UnityEngine;

public class FileHandler
{
    // 读取指定路径的文本文件内容
    public static byte[] ReadBytesFromFile(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(filePath);
        // 检查文件是否存在
        if (File.Exists(filePath))
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (IOException e)
            {
                Debug.LogError($"读取文件时发生错误: {e.Message}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"文件不存在: {filePath}");
            return null;
        }
    }
    public static bool WriteBytesToFile(byte[] bytes, string fileName, bool append = false)
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            try
            {
                File.WriteAllBytes(filePath, bytes);
                return true;
            }
            catch (IOException e)
            {
                Debug.LogError($"写入文件时发生错误: {e.Message}");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"文件不存在: {filePath}");
            return false;
        }
    }
}