#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using System;
using UnityEngine;
#endif

namespace EFramework.Unity.Entity
{
    
    [Serializable]
#if ODIN_INSPECTOR
    [LabelText("UUID组件")]
#endif
    public class UUIDComponent : EntityComponent
    {
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public string Uuid;
        public string Desc;
#if ODIN_INSPECTOR
        [Button("初始化实体"), ShowIf("CanInit")]
#endif

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
        public override void EditorInit(EntityObject entity)
        {
            base.EditorInit(entity);
            if (entity.name.Contains("UUID") == false)
            {
                Uuid = UUID.New();
                entity.name += "_UUID" + Uuid;
            }
            else
            {
                Uuid = GetUUID(entity.name);
            }
        }
        private bool CanInit()
        {
            if (string.IsNullOrEmpty(Uuid))
            {
                return false;
            }
            if(Uuid.Contains("UUID")==false)
            {
                return false;
            }
            return true;
        }
    }
}
