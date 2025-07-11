using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[HideMonoScript]
public class ToggleGroupExamplesComponent : SerializedMonoBehaviour
{
    [TypeFilter(nameof(GetFilteredTypeList))]
    [LabelText("模块数组")]
    [ListDrawerSettings(
        DraggableItems = true,
        NumberOfItemsPerPage = 5,
        ShowFoldout = true,
        ShowPaging = true
    )]
    public ModuleBase[] ToggleList = new ModuleBase[3];

    // 使用缓存的类型列表
    private static readonly IEnumerable<Type> cachedModuleTypes = CreateModuleTypeList();
    private IEnumerable<Type> GetFilteredTypeList() => cachedModuleTypes;

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
public abstract class ModuleBase
{
    [LabelText("启用")] public bool Enabled;
    [LabelText("名称")] public string Name;
    [LabelText("测试数值")] public float Test;
}

[Serializable]
[LabelText("日志模块")]
public class LogModule : ModuleBase
{
    [LabelText("唯一标识")] public string logStr;
}

[Serializable]
[LabelText("子模块1模块")]
public class SubModule1 : ModuleBase
{
    [LabelText("子模块1参数")] public int value1;
}

[Serializable]
[LabelText("子模块2模块")]
public class SubModule2 : ModuleBase
{
    [LabelText("子模块2参数")] public float value2;
}

[Serializable]
[LabelText("子模块3模块")]
public class SubModule3 : SubModule2
{
    [LabelText("子模块3参数")] public string value3;
}

// 泛型模块示例
[Serializable]
[LabelText("泛型字段模块")]
public class GenericModule<T> : ModuleBase
{
    [LabelText("泛型字段")] public T genericField;
}