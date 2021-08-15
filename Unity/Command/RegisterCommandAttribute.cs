using System;

namespace EFramework.Core
{
    //[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterCommandAttribute : Attribute
    {

        private string m_command;
        private string m_description;
        private string[] m_parameterNames;

        public string Command { get {return m_command; }set { m_command = value; } }
        public string Description { get { return m_description; } }
        public string[] ParameterNames { get { return m_parameterNames; } }
        /// <summary>
        /// 注册为事件
        /// </summary>
        /// <param name="command">命令或key</param>
        /// <param name="description">描述</param>
        /// <param name="parameterNames">参数</param>
        public RegisterCommandAttribute(string command, string description, params string[] parameterNames)
        {
            m_command = command;
            m_description = description;
            m_parameterNames = parameterNames;
        }

        public RegisterCommandAttribute(string command = null)
        {
            m_command = command;
        }
    }
}