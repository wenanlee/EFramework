using System;
using System.Reflection;

namespace CommandTerminal
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterCommandAttribute : Attribute
    {
        int min_arg_count = 0;
        int max_arg_count = -1;
        /// <summary>
        /// 最小参数数量
        /// </summary>
        public int MinArgCount {
            get { return min_arg_count; }
            set { min_arg_count = value; }
        }
        /// <summary>
        /// 最大参数数量
        /// </summary>
        public int MaxArgCount {
            get { return max_arg_count; }
            set { max_arg_count = value; }
        }

        public string Name { get; set; }
        /// <summary>
        /// 帮助
        /// </summary>
        public string Help { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string Hint { get; set; }

        public RegisterCommandAttribute(string command_name = null) {
            Name = command_name;
        }
    }
}
