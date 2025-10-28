using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace EFramework.Unity.Entity
{
    [Serializable]
    public class EntityVolume : ScriptableObject
    {
        [ReadOnly]
        public string Uuid;
        public string Desc;
        [SerializeReference]
        public List<EntityComponent> components = new();
        private readonly Dictionary<Type, EntityComponent> _componentCache = new();
        private bool _isCacheDirty = true;

        public bool ContainsComponent<T>() where T : EntityComponent
        {
            // 使用类型直接比较而不是 is 操作符
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

        public T GetComponentVolume<T>() where T : EntityComponent
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
            return null;
        }

        public void InitAllComponent(EntityObject entityObject)
        {
            // 使用 for 循环避免 foreach 的枚举器分配
            for (int i = 0; i < components.Count; i++)
            {
                components[i]?.Init(entityObject);
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
        public EntityVolume Clone()
        {
            // 创建新的 EntityVolume 实例
            EntityVolume clone = CreateInstance<EntityVolume>();

            // 复制基础字段
            clone.Uuid = this.Uuid;
            clone.Desc = this.Desc;

            // 深度复制组件列表 - 使用 EntityComponent 的 Clone<T> 方法
            clone.components = new List<EntityComponent>();
            foreach (var component in this.components)
            {
                if (component != null)
                {
                    var json = JsonUtility.ToJson(component);
                    var clonedComponent = JsonUtility.FromJson(json, component.GetType()) as EntityComponent;
                    clone.components.Add(clonedComponent);
                }
                else
                {
                    clone.components.Add(null);
                }
            }

            return clone;
        }
    }
}
