using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    [Serializable]
    public class EntityVolume : ScriptableObject
    {
        [SerializeReference]
        public List<EntityComponent> components =new List<EntityComponent>();
        public bool ContainsComponent<T>() where T : EntityComponent
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T)
                {
                    return true;
                }
            }
            return false;
        }
        public T GetComponentVolume<T>() where T : EntityComponent
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T t)
                {
                    return t;
                }
            }
            return null;
        }
    }
}
