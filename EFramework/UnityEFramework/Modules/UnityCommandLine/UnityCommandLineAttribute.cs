using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class RegisterCommandLine : Attribute
{
    /// <summary>
    /// 命令名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 命令的帮助说明
    /// </summary>
    public string Help { get; set; }
    /// <summary>
    /// 命令提示
    /// </summary>
    public string Hint { get; set; }
    public RegisterCommandLine(string command_name = null)
    {
        Name = command_name;
    }
}