using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ToggleGroupExamplesComponent : MonoBehaviour
{
    [SerializeField, Tooltip("模块数组")]
    private ModuleBase[] toggleList = new ModuleBase[3];

    public ModuleBase[] ToggleList
    {
        get => toggleList;
        set => toggleList = value;
    }

    // 使用缓存的类型列表
    public static readonly IEnumerable<Type> cachedModuleTypes = CreateModuleTypeList();

    [Serializable]
    public class PagedListSettings
    {
        public int ItemsPerPage = 5;
        public int CurrentPage = 0;
    }

    [SerializeField]
    private PagedListSettings listSettings = new PagedListSettings();

    // 静态方法创建类型列表（首次访问时执行一次）
    private static IEnumerable<Type> CreateModuleTypeList()
    {
        var baseType = typeof(ModuleBase);
        var assembly = baseType.Assembly;

        // 创建类型集合（自动去重）
        var typeSet = new HashSet<Type>();

        // 添加所有符合条件的非泛型类型
        foreach (var type in assembly.GetTypes().Where(IsValidNonGenericType))
        {
            typeSet.Add(type);
        }

        // 添加预定义的泛型实例化类型
        var genericBase = typeof(GenericModule<>);
        foreach (var genericArg in GetPredefinedGenericArguments())
        {
            typeSet.Add(genericBase.MakeGenericType(genericArg));
        }

        return typeSet.OrderBy(t => t.Name); // 按名称排序便于浏览
    }

    // 验证是否为有效的非泛型模块类型
    private static bool IsValidNonGenericType(Type type)
    {
        return !type.IsAbstract &&
               !type.IsGenericTypeDefinition &&
               typeof(ModuleBase).IsAssignableFrom(type);
    }

    // 集中管理泛型参数类型
    private static IEnumerable<Type> GetPredefinedGenericArguments()
    {
        yield return typeof(GameObject);
        yield return typeof(AnimationCurve);
        yield return typeof(List<float>);
        // 可在此处扩展更多类型...
        // yield return typeof(Transform);
    }
}

// ============ 以下是模块基类及其派生类/泛型子类 ============

[Serializable]
public abstract class ModuleBase : ScriptableObject
{
    [Tooltip("启用")] public bool Enabled;
    [Tooltip("名称")] public string Name;
    [Tooltip("测试数值")] public float Test;
}

[Serializable]
[Tooltip("日志模块")]
public class LogModule : ModuleBase
{
    [Tooltip("唯一标识")] public string logStr;
}

[Serializable]
[Tooltip("子模块1模块")]
public class SubModule1 : ModuleBase
{
    [Tooltip("子模块1参数")] public int value1;
}

[Serializable]
[Tooltip("子模块2模块")]
public class SubModule2 : ModuleBase
{
    [Tooltip("子模块2参数")] public float value2;
}

[Serializable]
[Tooltip("子模块3模块")]
public class SubModule3 : SubModule2
{
    [Tooltip("子模块3参数")] public string value3;
}

// 泛型模块示例
[Serializable]
[Tooltip("泛型字段模块")]
public class GenericModule<T> : ModuleBase where T : class, new()
{
    [Tooltip("泛型字段")] public T genericField;
}