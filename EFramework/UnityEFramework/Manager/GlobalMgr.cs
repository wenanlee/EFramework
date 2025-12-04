using EFramework.Unity.Entity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    public class GlobalMgr : MonoSingleton<GlobalMgr>
    {
        [Title("运行时生成物品字典")]
        [ShowInInspector, Searchable]
        public Dictionary<string, GameEntity> entityObjDict = new();
        public T GetComponentByEntityID<T>(string entityID) where T : Component
        {

            if (entityObjDict.TryGetValue(entityID, out GameEntity entity))
            {
                if (entity.TryGetComponent(out T component))
                {
                    return component;
                }
            }
            Debug.LogError($"找不到实体: {entityID}");
            return null;
        }
        public T GetComponentByEntitySubObject<T>(string entityID, string subObjName)
        {
            if (entityObjDict.TryGetValue(entityID, out GameEntity entity))
            {
               return entity.gameObject.GetComponentInChildrenByName<T>(subObjName);
            }
            Debug.LogError($"找不到实体: {entityID}");
            return default;
        }
    }
}
