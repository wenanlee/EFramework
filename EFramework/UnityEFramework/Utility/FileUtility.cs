using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EFramework.Unity.Utility
{

    public static class FileUtility
    {
        /// <summary>
        /// 生成枚举文件
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="enumName">枚举类名</param>
        /// <param name="enumEntries">枚举元素数组</param>
        public static void GenerateEnumFile(string savePath, string enumName, string[] enumEntries)
        {
            // 基本参数检查
            if (string.IsNullOrEmpty(enumName))
                throw new ArgumentException("枚举名称不能为空");

            if (enumEntries == null || enumEntries.Length == 0)
                throw new ArgumentException("枚举元素数组不能为空");

            // 清理并验证元素名称
            for (int i = 0; i < enumEntries.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(enumEntries[i]))
                    throw new ArgumentException($"枚举元素不能为空 (索引: {i})");

                enumEntries[i] = enumEntries[i].Trim();
            }

            // 确保目录存在
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            // 构建文件路径
            string filePath = Path.Combine(savePath, $"{enumName}.cs");

            // 生成枚举代码
            //string code = BuildEnumCode(enumName, enumEntries);
            StringBuilder sb = new StringBuilder();

            // 生成枚举代码
            sb.AppendLine($"public enum {enumName}");
            sb.AppendLine("{");

            for (int i = 0; i < enumEntries.Length; i++)
            {
                sb.AppendLine($"    {enumEntries[i]} = {i},");
            }

            sb.AppendLine("}");
            // 写入文件
            File.WriteAllText(filePath, sb.ToString());

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

            Debug.Log($"枚举生成成功: {filePath}");
        }


        /// <summary>
        /// 生成常量字符串类文件（使用字典）
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="className">类名</param>
        /// <param name="constants">常量字典（键：常量名，值：常量值）</param>
        public static void GenerateConstantsFile(string savePath, string className, Dictionary<string, string> constants)
        {
            // 基本参数检查
            if (string.IsNullOrEmpty(className))
                throw new ArgumentException("类名称不能为空");

            if (constants == null || constants.Count == 0)
                throw new ArgumentException("常量字典不能为空");

            // 清理并验证常量名称
            var cleanConstants = new Dictionary<string, string>();
            foreach (var kvp in constants)
            {
                if (string.IsNullOrWhiteSpace(kvp.Key))
                    throw new ArgumentException("常量名不能为空");

                string cleanKey = kvp.Key.Trim();
                string cleanValue = kvp.Value?.Trim() ?? "";

                cleanConstants[cleanKey] = cleanValue;
            }

            // 确保目录存在
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            // 构建文件路径
            string filePath = Path.Combine(savePath, $"{className}.cs");

            // 生成类代码
            StringBuilder sb = new StringBuilder();

            // 生成常量类代码
            sb.AppendLine($"public static class {className}");
            sb.AppendLine("{");

            foreach (var kvp in cleanConstants)
            {
                sb.AppendLine($"    public const string {kvp.Key} = \"{kvp.Value}\";");
            }

            sb.AppendLine("}");

            // 写入文件
            File.WriteAllText(filePath, sb.ToString());

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

            Debug.Log($"常量类生成成功: {filePath}");
        }
    }
}
