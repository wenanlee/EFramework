using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.ECS
{
    [Serializable]
    public class EntityVolumeBase<TComponent> where TComponent : ComponentBase
    {
        [ReadOnly]
        public string uuid;
        [SerializeReference]
        public List<TComponent> components = new();
    }
}