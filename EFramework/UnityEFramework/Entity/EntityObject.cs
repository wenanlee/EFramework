using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class EntityObject : MonoBehaviour
    {
        [ReadOnly]
        public string Uuid;
        public string Name => name.Replace("(Clone)", "").TrimEnd().Substring(0, name.Length - 11);
        public string FullName => name.Replace("(Clone)", "").TrimEnd();
        public string Desc;

        public List<EntityComponent> components;
        public EntityComponentVolume volume;

        [Button("初始化实体"),ShowIf("@string.IsNullOrEmpty(Uuid)&&name.Contains(\"UUID\")==false")]
        public virtual void Init()
        {
            if (name.Contains("UUID") == false)
            {
                Debug.Log("重新分配UUID" + name + "   " + name.Contains("UUID"));
                Uuid = UUID.New();
                name += "_UUID" + Uuid;
                Debug.Log(name);
            }
            else
            {
                Uuid  = name.GetUUID();
            }
            if(volume !=null)
            {
                components = volume.Components;
            }
        }

        
    }
}
