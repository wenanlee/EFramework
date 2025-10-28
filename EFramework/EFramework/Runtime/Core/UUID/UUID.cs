using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace EFramework
{
    public class UUID
    {
        private static long _lastTicks = 0;
        private static uint _sequence = 0;
        private static readonly object _lock = new object();

        // 自定义字符集（排除易混淆字符）
        private const string CharSet = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";
        private static readonly int Base = CharSet.Length; // 58进制（实际使用58个字符）
        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <param name="length">id长度，最小6，最大16</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string New(int length = 6)
        {
            if (length < 6 || length > 16)
                throw new ArgumentException("Length must be between 6 and 16 characters");

            lock (_lock)
            {
                long currentTicks = GetTimestampPart();
                if (currentTicks != _lastTicks)
                {
                    _lastTicks = currentTicks;
                    _sequence = 0;
                }
                else
                {
                    _sequence++;
                }

                // 合并时间戳和序列号
                ulong combined = CombineValues(currentTicks, _sequence);
                return ConvertToBaseX(combined, length);
            }
        }

        private static long GetTimestampPart()
        {
            // 取UTC时间的中间56位（忽略最低8位）
            long ticks = DateTime.UtcNow.Ticks >> 8;
            return ticks & 0x00FFFFFFFFFFFFFF; // 保留56位
        }

        private static ulong CombineValues(long timestamp, uint sequence)
        {
            // 使用位合并（56位时间戳 + 16位序列号）
            return ((ulong)timestamp << 16) | sequence;
        }

        private static string ConvertToBaseX(ulong number, int length)
        {
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                ulong remainder = number % (ulong)Base;
                result.Insert(0, CharSet[(int)remainder]);
                number /= (ulong)Base;
            }

            // 补充前导零（当数字不足指定长度时）
            while (result.Length < length)
            {
                result.Insert(0, CharSet[0]);
            }

            return result.ToString().Substring(0, length);
        }
    }
}
