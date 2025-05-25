using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EFramework.Unity.Utility
{
    public static class ReflectionHelper
    {
        #region InvokeMethod Overloads

        // 基础重载
        public static object InvokeMethod(
            string assemblyName,
            string typeFullName,
            string methodName,
            object targetInstance,
            object[] args)
        {
            Type[] paramTypes = args?.Select(a => a?.GetType() ?? typeof(object)).ToArray() ?? Type.EmptyTypes;
            return InvokeMethod(assemblyName, typeFullName, methodName, targetInstance, paramTypes, args);
        }

        // 指定参数类型的重载
        public static object InvokeMethod(
            string assemblyName,
            string typeFullName,
            string methodName,
            object targetInstance,
            Type[] parameterTypes,
            object[] args)
        {
            try
            {
                Assembly assembly = GetAssembly(assemblyName);
                Type type = GetTypeFromAssembly(assembly, typeFullName);
                targetInstance = AdjustMonoBehaviourTarget(targetInstance, type);
                MethodInfo method = GetMethodInfo(type, methodName, parameterTypes);
                ValidateMethodInvocation(method, targetInstance);

                return method.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                HandleReflectionError(ex);
                return null;
            }
        }

        // 使用Type的重载
        public static object InvokeMethod(
            Type type,
            string methodName,
            object targetInstance,
            object[] args)
        {
            Type[] paramTypes = args?.Select(a => a?.GetType() ?? typeof(object)).ToArray() ?? Type.EmptyTypes;
            return InvokeMethod(type, methodName, targetInstance, paramTypes, args);
        }

        public static object InvokeMethod(
            Type type,
            string methodName,
            object targetInstance,
            Type[] parameterTypes,
            object[] args)
        {
            try
            {
                targetInstance = AdjustMonoBehaviourTarget(targetInstance, type);
                MethodInfo method = GetMethodInfo(type, methodName, parameterTypes);
                ValidateMethodInvocation(method, targetInstance);

                return method.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                HandleReflectionError(ex);
                return null;
            }
        }

        // 泛型重载
        public static object InvokeMethod<T>(
            string methodName,
            object targetInstance,
            object[] args) where T : class
        {
            return InvokeMethod(typeof(T), methodName, targetInstance, args);
        }

        // 直接使用MethodInfo的重载
        public static object InvokeMethod(
            MethodInfo methodInfo,
            object targetInstance,
            object[] args)
        {
            try
            {
                targetInstance = AdjustMonoBehaviourTarget(targetInstance, methodInfo.DeclaringType);
                ValidateMethodInvocation(methodInfo, targetInstance);

                return methodInfo.Invoke(targetInstance, args);
            }
            catch (Exception ex)
            {
                HandleReflectionError(ex);
                return null;
            }
        }

        #endregion

        #region Helper Methods

        private static Assembly GetAssembly(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
                return Assembly.GetExecutingAssembly();

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));

            if (assembly != null)
                return assembly;

            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                throw new Exception($"Assembly '{assemblyName}' not found.");
            }
        }

        private static Type GetTypeFromAssembly(Assembly assembly, string typeFullName)
        {
            Type type = assembly.GetType(typeFullName);
            return type ?? throw new Exception($"Type '{typeFullName}' not found in assembly '{assembly.GetName()}'.");
        }

        private static object AdjustMonoBehaviourTarget(object target, Type targetType)
        {
            if(target == null)
                return null;
            if (target is GameObject go && typeof(MonoBehaviour).IsAssignableFrom(targetType))
            {
                Component component = go.GetComponent(targetType);
                return component ?? throw new Exception(
                    $"Component of type '{targetType.Name}' not found on GameObject '{go.name}'.");
            }
            return target;
        }

        private static MethodInfo GetMethodInfo(Type type, string methodName, Type[] parameterTypes)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            MethodInfo method = type.GetMethod(methodName, flags, null, parameterTypes, null);

            return method ?? throw new Exception(
                $"Method '{methodName}' with specified parameters not found in type '{type.FullName}'.");
        }

        private static void ValidateMethodInvocation(MethodInfo method, object targetInstance)
        {
            if (method.IsStatic && targetInstance != null)
                throw new ArgumentException(
                    $"Static method '{method.Name}' cannot be called with an instance.");

            if (!method.IsStatic && targetInstance == null)
                throw new ArgumentNullException(
                    nameof(targetInstance), $"Instance required for non-static method '{method.Name}'.");
        }

        private static void HandleReflectionError(Exception ex)
        {
            Debug.LogError($"[ReflectionHelper] Error: {ex.Message}\n{ex.StackTrace}");
        }

        #endregion

        #region Method Info Helpers

        public static void GetMethodInfoDetails(
            MethodInfo methodInfo,
            out string assemblyName,
            out string className,
            out string methodName)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            Type declaringType = methodInfo.DeclaringType ?? throw new InvalidOperationException(
                "Method does not have declaring type.");
            assemblyName = declaringType.Assembly.GetName().Name;
            className = declaringType.FullName;
            methodName = methodInfo.Name;
        }
        public static (string assemblyName, string className, string methodName) GetMethodDetails(
            MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            Type declaringType = methodInfo.DeclaringType ?? throw new InvalidOperationException(
                "Method does not have declaring type.");

            return (
                declaringType.Assembly.GetName().Name,
                declaringType.FullName,
                methodInfo.Name
            );
        }

        public static string[] GetMethodDetailsAsArray(MethodInfo methodInfo)
        {
            var details = GetMethodDetails(methodInfo);
            return new[] { details.assemblyName, details.className, details.methodName };
        }

        #endregion
    }
}