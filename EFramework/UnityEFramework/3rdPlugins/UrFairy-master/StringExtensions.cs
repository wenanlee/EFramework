using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using UnityEngine;

public static class StringExtensions
{
    public static string ToColor(this string str, Color color, bool bold = false, bool italic = false)
    {
        var content = string.Format("<color={0}>{1}</color>", color.ToString(), str);
        content = bold ? string.Format("<b>{0}</b>", content) : content;
        content = italic ? string.Format("<i>{0}</i>", content) : content;

        return content;
    }
   
    /// <summary>
    /// 获取后缀(Int)
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="lenght">长度</param>
    /// <returns></returns>
    public static int StringEndToInt(this string str, int lenght)
    {
        return int.Parse(str.Substring(str.Length - lenght - 1, lenght));
    }

    /// <summary>
    /// 获取前缀(int)
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="lenght">长度</param>
    /// <returns></returns>
    public static int StringStratToInt(this string str, int lenght)
    {
        return int.Parse(str.Substring(0, lenght));
    }


    /// <summary>
    /// 获取后缀使用分隔符(Int)
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="separator">分隔符</param>
    /// <param name="index">索引</param>
    /// <returns></returns>
    public static int StringEndToIntBySeparator(this string str, string separator, int index = -1)
    {
        string[] args = str.Split(separator);
        return int.Parse(args[args.Length + index]);
    }

    /// <summary>
    /// 获取前缀使用分隔符(Int)
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="separator">分隔符</param>
    /// <param name="index">索引</param>
    /// <returns></returns>
    public static int StringStartToIntBySeparator(this string str, string separator, int index = 0)
    {
        return int.Parse(str.Split(separator)[index]);
    }
    /// <summary>
    /// 压缩
    /// </summary>
    /// <param name="uncompressedString">未压缩的字符串</param>
    /// <returns></returns>
    public static string Compress(this string uncompressedString)
    {
        byte[] compressedBytes;

        using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
        {
            using (var compressedStream = new MemoryStream())
            {
                // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                using (var compressorStream = new DeflateStream(compressedStream, System.IO.Compression.CompressionLevel.Fastest, true))
                {
                    uncompressedStream.CopyTo(compressorStream);
                }

                // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                compressedBytes = compressedStream.ToArray();
            }
        }

        return Convert.ToBase64String(compressedBytes);
    }
    /// <summary>
    /// 解压缩
    /// </summary>
    /// <param name="compressedString">压缩过后的字符串</param>
    /// <returns></returns>
    public static string Decompress(this string compressedString)
    {
        byte[] decompressedBytes;

        var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

        using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
        {
            using (var decompressedStream = new MemoryStream())
            {
                decompressorStream.CopyTo(decompressedStream);

                decompressedBytes = decompressedStream.ToArray();
            }
        }

        return Encoding.UTF8.GetString(decompressedBytes);
    }
}
