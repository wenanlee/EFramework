using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    [Serializable]
    public class EntityVolumeBase<TComponent> : ScriptableObject where TComponent : class
    {
        [ReadOnly]
        public string Uuid;
        public string Desc;
        [SerializeReference]
        public List<TComponent> components = new();
        private readonly Dictionary<Type, TComponent> _componentCache = new();
        private bool _isCacheDirty = true;

        public bool ContainsComponent<T>() where T : TComponent
        {
            var targetType = typeof(T);

            // 尝试从缓存中查找
            if (!_isCacheDirty && _componentCache.ContainsKey(targetType))
                return true;

            // 线性搜索（当缓存无效时）
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component != null && component.GetType() == targetType)
                {
                    if (!_isCacheDirty)
                        _componentCache[targetType] = component;
                    return true;
                }
            }
            return false;
        }

        public T GetComponentVolume<T>() where T : TComponent
        {
            var targetType = typeof(T);

            // 尝试从缓存中获取
            if (!_isCacheDirty && _componentCache.TryGetValue(targetType, out var cachedComponent))
                return (T)cachedComponent;

            // 线性搜索
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component != null && component.GetType() == targetType)
                {
                    var result = (T)component;
                    if (!_isCacheDirty)
                        _componentCache[targetType] = result;
                    return result;
                }
            }
            return default(T);
        }

        public void InitAllComponent<TObject>(TObject entityObject) where TObject : class
        {
            // 使用 for 循环避免 foreach 的枚举器分配
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component is EntityComponentBase<TObject> entityComponent)
                {
                    entityComponent.Init(entityObject);
                }
            }
            RebuildCache();
        }

        public void RebuildCache()
        {
            _componentCache.Clear();
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component != null)
                {
                    var type = component.GetType();
                    if (!_componentCache.ContainsKey(type))
                        _componentCache[type] = component;
                }
            }
            _isCacheDirty = false;
        }

        public EntityVolumeBase<TComponent> Clone()
        {
            // 创建新的 EntityVolume 实例
            EntityVolumeBase<TComponent> clone = CreateInstance<EntityVolumeBase<TComponent>>();

            // 复制基础字段
            clone.Uuid = this.Uuid;
            clone.Desc = this.Desc;

            // 深度复制组件列表
            clone.components = new List<TComponent>();
            foreach (var component in this.components)
            {
                if (component != null)
                {
                    var json = JsonUtility.ToJson(component);
                    var clonedComponent = JsonUtility.FromJson(json, component.GetType()) as TComponent;
                    clone.components.Add(clonedComponent);
                }
                else
                {
                    clone.components.Add(null);
                }
            }

            return clone;
        }

        // 添加组件的方法
        public void AddComponent(TComponent component)
        {
            if (component != null)
            {
                components.Add(component);
                _isCacheDirty = true;
            }
        }

        // 移除组件的方法
        public bool RemoveComponent<T>() where T : TComponent
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i] is T)
                {
                    components.RemoveAt(i);
                    _isCacheDirty = true;
                    return true;
                }
            }
            return false;
        }

        // 获取所有组件的方法
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

        // 标记缓存为脏
        public void MarkCacheDirty()
        {
            _isCacheDirty = true;
        }
    }
}
