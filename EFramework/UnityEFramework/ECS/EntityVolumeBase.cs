using Sirenix.OdinInspector;  // Odin Inspector插件，用于Unity编辑器增强
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    /// <summary>
    /// 实体数据基类，使用泛型约束组件类型
    /// </summary>
    /// <typeparam name="TComponent">组件基类型约束</typeparam>
    [Serializable]
    public class EntityVolumeBase<TComponent> where TComponent : class
    {
        [ReadOnly]  // Odin特性：在编辑器中只读显示
        public string Uuid;  // 唯一标识符

        public string Desc;  // 描述信息

        [SerializeReference]  // Unity序列化引用，支持多态序列化
        public List<TComponent> components = new();  // 组件列表

        private readonly Dictionary<Type, TComponent> _componentCache = new();  // 类型到组件的缓存字典
        private bool _isCacheDirty = true;  // 缓存脏标记，指示缓存是否需要重建

        /// <summary>
        /// 检查是否包含指定类型的组件
        /// </summary>
        /// <typeparam name="T">要查找的组件类型</typeparam>
        /// <returns>是否包含该组件</returns>
        public bool ContainsComponent<T>() where T : TComponent
        {
            var targetType = typeof(T);

            // 尝试从缓存中查找（缓存有效且存在时）
            if (!_isCacheDirty && _componentCache.ContainsKey(targetType))
                return true;

            // 线性搜索（当缓存无效或未命中时）
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component != null && component.GetType() == targetType)
                {
                    // 如果缓存有效但未命中，更新缓存
                    if (!_isCacheDirty)
                        _componentCache[targetType] = component;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定类型的组件
        /// </summary>
        /// <typeparam name="T">要获取的组件类型</typeparam>
        /// <returns>找到的组件，未找到返回默认值</returns>
        public T GetComponentVolume<T>() where T : TComponent
        {
            var targetType = typeof(T);

            // 尝试从缓存中获取（缓存有效且存在时）
            if (!_isCacheDirty && _componentCache.TryGetValue(targetType, out var cachedComponent))
                return (T)cachedComponent;

            // 线性搜索（当缓存无效或未命中时）
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component != null && component.GetType() == targetType)
                {
                    var result = (T)component;
                    // 如果缓存有效但未命中，更新缓存
                    if (!_isCacheDirty)
                        _componentCache[targetType] = result;
                    return result;
                }
            }
            return default(T);  // 未找到返回默认值
        }

        /// <summary>
        /// 初始化所有组件到实体对象
        /// </summary>
        /// <typeparam name="TObject">实体对象类型</typeparam>
        /// <param name="entityObject">要初始化的实体对象</param>
        public void InitAllComponent<TObject>(TObject entityObject) where TObject : class
        {
            // 使用for循环避免foreach的枚举器分配（性能优化）
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                // 检查组件是否实现了EntityComponentBase接口
                if (component is EntityComponentBase<TObject> entityComponent)
                {
                    entityComponent.Init(entityObject);  // 初始化组件
                }
            }
            RebuildCache();  // 初始化后重建缓存
        }

        /// <summary>
        /// 重建组件缓存
        /// </summary>
        public void RebuildCache()
        {
            _componentCache.Clear();  // 清空现有缓存
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component != null)
                {
                    var type = component.GetType();
                    // 避免重复类型，只缓存第一个遇到的组件
                    if (!_componentCache.ContainsKey(type))
                        _componentCache[type] = component;
                }
            }
            _isCacheDirty = false;  // 标记缓存已更新
        }

        /// <summary>
        /// 深拷贝当前EntityVolume
        /// </summary>
        /// <returns>拷贝后的新实例</returns>
        public EntityVolumeBase<TComponent> Clone()
        {
            // 创建新的EntityVolume实例
            EntityVolumeBase<TComponent> clone = new();

            // 复制基础字段
            clone.Uuid = this.Uuid;
            clone.Desc = this.Desc;

            // 深度复制组件列表（使用JSON序列化实现深拷贝）
            clone.components = new List<TComponent>();
            foreach (var component in this.components)
            {
                if (component != null)
                {
                    // 通过JSON序列化实现深拷贝
                    var json = JsonUtility.ToJson(component);
                    var clonedComponent = JsonUtility.FromJson(json, component.GetType()) as TComponent;
                    clone.components.Add(clonedComponent);
                }
                else
                {
                    clone.components.Add(null);  // 处理空组件
                }
            }

            return clone;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <param name="component">要添加的组件</param>
        public TComponent AddComponent(TComponent component)
        {
            if (component != null)
            {
                components.Add(component);
                _isCacheDirty = true;  // 标记缓存需要更新
            }
            return component;
        }

        /// <summary>
        /// 移除指定类型的组件
        /// </summary>
        /// <typeparam name="T">要移除的组件类型</typeparam>
        /// <returns>是否成功移除</returns>
        public bool RemoveComponent<T>() where T : TComponent
        {
            // 从后向前遍历避免索引错位
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i] is T)
                {
                    components.RemoveAt(i);
                    _isCacheDirty = true;  // 标记缓存需要更新
                    return true;
                }
            }
            return false;  // 未找到指定类型的组件
        }

        /// <summary>
        /// 获取所有指定类型的组件
        /// </summary>
        /// <typeparam name="T">要获取的组件类型</typeparam>
        /// <returns>组件列表</returns>
        public List<T> GetAllComponents<T>() where T : TComponent
        {
            List<T> result = new List<T>();
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T component)
                {
                    result.Add(component);
                }
            }
            return result;
        }

        /// <summary>
        /// 标记缓存为脏，指示缓存需要重建
        /// </summary>
        public void MarkCacheDirty()
        {
            _isCacheDirty = true;
        }
    }
}