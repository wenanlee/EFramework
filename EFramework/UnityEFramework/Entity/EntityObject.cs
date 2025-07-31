#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class EntityObject : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public string Uuid;
        public string Name => name.Replace("(Clone)", "").TrimEnd().Substring(0, name.Length - 11);
        public string FullName => name.Replace("(Clone)", "").TrimEnd();
        public string Desc;

        //public List<EntityComponent> components;
        //[InlineEditor]
        //public EntityComponentVolume volume;
#if ODIN_INSPECTOR
        [Button("初始化实体"),ShowIf("@string.IsNullOrEmpty(Uuid)&&name.Contains(\"UUID\")==false")]
#endif
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
                Uuid  = GetUUID(name);
            }
            //if(volume !=null)
            //{
            //    components = volume.Components;
            //}
        }

        private string GetUUID(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogError("获取UUID失败，字符串为空");
                return string.Empty;
            }
            if (str.Length < 10)
            {
                Debug.LogError("获取UUID失败，字符串长度小于10");
                return string.Empty;
            }
            if (str.Contains("UUID") == false)
            {
                Debug.LogError($"{str} 中找不到UUID");
                return string.Empty;
            }
            return str.Substring(str.IndexOf("UUID") + 4, 6);
        }
    }
}
