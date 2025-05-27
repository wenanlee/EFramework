using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EFramework.Unity.Utility
{
    /// <summary>
    /// 反射辅助工具类，提供通过反射调用方法的实用功能
    /// </summary>
    public static class ReflectionHelper
    {
        #region InvokeMethod Overloads

        /// <summary>
        /// 通过程序集名称、类型全名和方法名调用方法
        /// </summary>
        /// <param name="assemblyName"> 程序集名称</param>
        /// <param name="typeFullName">类型全名(包括命名空间)</param>
        /// <param name="methodName">方法名</param>
        /// <param name="targetInstance">目标实例(静态方法可为null)</param>
        /// <param name="args">方法参数数组</param>
        /// <returns></returns>
        public static object InvokeMethod(
            string assemblyName,      // 程序集名称
            string typeFullName,      // 类型全名(包括命名空间)
            string methodName,       // 方法名
            object targetInstance,    // 目标实例(静态方法可为null)
            object[] args)           // 方法参数数组
        {
            // 从参数对象数组推断参数类型
            Type[] paramTypes = args?.Select(a => a?.GetType() ?? typeof(object)).ToArray() ?? Type.EmptyTypes;
            return InvokeMethod(assemblyName, typeFullName, methodName, targetInstance, paramTypes, args);
        }
        
        /// <summary>
        /// 通过程序集名称、类型全名和方法名调用方法(使用明确的参数类型数组)
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="typeFullName">类型全名(包括命名空间)</param>
        /// <param name="methodName">方法名</param>
        /// <param name="targetInstance">目标实例(静态方法可为null)</param>
        /// <param name="parameterTypes">// 明确的参数类型数组</param>
        /// <param name="args">方法参数数组</param>
        /// <returns></returns>
        public static object InvokeMethod(
            string assemblyName,
            string typeFullName,
            string methodName,
            object targetInstance,
            Type[] parameterTypes,    // 明确的参数类型数组
            object[] args)
        {
            try
            {
                // 1. 获取程序集
                Assembly assembly = GetAssembly(assemblyName);
                // 2. 从程序集中获取类型
                Type type = GetTypeFromAssembly(assembly, typeFullName);
                // 3. 调整MonoBehaviour目标实例(如果是GameObject则获取对应组件)
                targetInstance = AdjustMonoBehaviourTarget(targetInstance, type);
                // 4. 获取方法信息
                MethodInfo method = GetMethodInfo(type, methodName, parameterTypes);
                // 5. 验证方法调用是否有效(静态/实例方法检查)
                ValidateMethodInvocation(method, targetInstance);

                // 6. 调用方法并返回结果
                return method.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                // 处理反射错误
                HandleReflectionError(ex);
                return null;
            }
        }

        // 使用Type的重载 - 直接使用Type对象调用方法
        public static object InvokeMethod(
            Type type,               // 直接传入Type对象
            string methodName,
            object targetInstance,
            object[] args)
        {
            // 从参数对象数组推断参数类型
            Type[] paramTypes = args?.Select(a => a?.GetType() ?? typeof(object)).ToArray() ?? Type.EmptyTypes;
            return InvokeMethod(type, methodName, targetInstance, paramTypes, args);
        }

        // 使用Type和明确参数类型的重载
        public static object InvokeMethod(
            Type type,
            string methodName,
            object targetInstance,
            Type[] parameterTypes,
            object[] args)
        {
            try
            {
                // 调整MonoBehaviour目标实例
                targetInstance = AdjustMonoBehaviourTarget(targetInstance, type);
                // 获取方法信息
                MethodInfo method = GetMethodInfo(type, methodName, parameterTypes);
                // 验证方法调用
                ValidateMethodInvocation(method, targetInstance);

                // 调用方法并返回结果
                return method.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                // 处理反射错误
                HandleReflectionError(ex);
                return null;
            }
        }

        // 泛型重载 - 使用泛型类型参数调用方法
        public static object InvokeMethod<T>(
            string methodName,
            object targetInstance,
            object[] args) where T : class
        {
            return InvokeMethod(typeof(T), methodName, targetInstance, args);
        }

        // 直接使用MethodInfo的重载 - 使用预获取的MethodInfo调用方法
        public static object InvokeMethod(
            MethodInfo methodInfo,    // 预获取的方法信息
            object targetInstance,
            object[] args)
        {
            try
            {
                // 调整MonoBehaviour目标实例
                targetInstance = AdjustMonoBehaviourTarget(targetInstance, methodInfo.DeclaringType);
                // 验证方法调用
                ValidateMethodInvocation(methodInfo, targetInstance);

                // 调用方法并返回结果
                return methodInfo.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                // 处理反射错误
                HandleReflectionError(ex);
                return null;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// 获取指定名称的程序集
        /// </summary>
        private static Assembly GetAssembly(string assemblyName)
        {
            // 如果程序集名为空，返回当前执行程序集
            if (string.IsNullOrEmpty(assemblyName))
                return Assembly.GetExecutingAssembly();

            // 尝试从已加载程序集中查找
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));

            if (assembly != null)
                return assembly;

            // 尝试加载程序集
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                throw new Exception($"找不到程序集: '{assemblyName}'");
            }
        }

        /// <summary>
        /// 从程序集中获取指定类型
        /// </summary>
        private static Type GetTypeFromAssembly(Assembly assembly, string typeFullName)
        {
            Type type = assembly.GetType(typeFullName);
            return type ?? throw new Exception($"在程序集 '{assembly.GetName()}' 中找不到类型 '{typeFullName}'");
        }

        /// <summary>
        /// 调整MonoBehaviour目标实例 - 如果是GameObject则获取对应组件
        /// </summary>
        private static object AdjustMonoBehaviourTarget(object target, Type targetType)
        {
            if (target == null)
                return null;

            // 如果目标是GameObject且目标类型是MonoBehaviour派生类
            if (target is GameObject go && typeof(MonoBehaviour).IsAssignableFrom(targetType))
            {
                // 尝试获取组件
                Component component = go.GetComponent(targetType);
                return component ?? throw new Exception(
                    $"在游戏对象 '{go.name}' 上找不到类型为 '{targetType.Name}' 的组件");
            }
            return target;
        }

        /// <summary>
        /// 获取方法信息
        /// </summary>
        private static MethodInfo GetMethodInfo(Type type, string methodName, Type[] parameterTypes)
        {
            // 设置绑定标志(包括公共、非公共、静态和实例方法)
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            // 获取方法信息
            MethodInfo method = type.GetMethod(methodName, flags, null, parameterTypes, null);

            return method ?? throw new Exception(
                $"在类型 '{type.FullName}' 中找不到具有指定参数的方法 '{methodName}'");
        }

        /// <summary>
        /// 验证方法调用是否有效
        /// </summary>
        private static void ValidateMethodInvocation(MethodInfo method, object targetInstance)
        {
            // 静态方法不能有实例
            if (method.IsStatic && targetInstance != null)
                throw new ArgumentException(
                    $"静态方法 '{method.Name}' 不能通过实例调用");

            // 实例方法必须有实例
            if (!method.IsStatic && targetInstance == null)
                throw new ArgumentNullException(
                    nameof(targetInstance), $"非静态方法 '{method.Name}' 需要实例对象");
        }

        /// <summary>
        /// 处理反射错误 - 输出错误日志
        /// </summary>
        private static void HandleReflectionError(Exception ex)
        {
            Debug.LogError($"[反射辅助工具] 错误: {ex.Message}\n{ex.StackTrace}");
        }

        #endregion

        #region Method Info Helpers

        /// <summary>
        /// 获取方法信息的详细信息(程序集名、类名、方法名)
        /// </summary>
        public static void GetMethodInfoDetails(
            MethodInfo methodInfo,
            out string assemblyName,
            out string className,
            out string methodName)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            Type declaringType = methodInfo.DeclaringType ?? throw new InvalidOperationException(
                "方法没有声明类型");

            assemblyName = declaringType.Assembly.GetName().Name;
            className = declaringType.FullName;
            methodName = methodInfo.Name;
        }

        /// <summary>
        /// 获取方法详细信息(元组形式返回)
        /// </summary>
        public static (string assemblyName, string className, string methodName) GetMethodDetails(
            MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            Type declaringType = methodInfo.DeclaringType ?? throw new InvalidOperationException(
                "方法没有声明类型");

            return (
                declaringType.Assembly.GetName().Name,
                declaringType.FullName,
                methodInfo.Name
            );
        }

        /// <summary>
        /// 获取方法详细信息(数组形式返回)
        /// </summary>
        public static string[] GetMethodDetailsAsArray(MethodInfo methodInfo)
        {
            var details = GetMethodDetails(methodInfo);
            return new[] { details.assemblyName, details.className, details.methodName };
        }

        #endregion
    }
}