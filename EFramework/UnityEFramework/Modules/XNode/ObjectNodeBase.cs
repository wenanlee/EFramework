using EFramework;
using EFramework.Unity.DataTable;
using EFramework.Unity.Entity;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode.Core;
namespace EFramework.Unity.XNode
{


    public class ObjectNodeBase<T> : ProcessNodeBase where T : UnityEngine.Object
    {
        [ValueDropdown("@OdinDataBinding.GetEntityIDs(graph.name)", DropdownWidth = 600)]
        [LabelText("绑定实体ID")]
        public string entityUuid; // 目标实体UUID
        [ValueDropdown("@GetAllComponents()", DropdownWidth = 600)]
        [LabelText("实例名称")]
        public string objName;
   #if UNITY_EDITOR     
        //public T obj => entityObject?.gameObject.GetComponentInChildrenByName<T>(objName);
        public T obj
        {
            get
            {
                if(entityObject==null)
                    return null;
                return entityObject.gameObject.GetComponentInChildrenByName<T>(objName);
            }
        }

        [ShowInInspector, ReadOnly, LabelText("绑定实体"), HideIf("@string.IsNullOrEmpty(objUuid)||objUuid==\"None\"")]
        public GameEntity entityObject
        {
            get
            {
                if (string.IsNullOrEmpty(entityUuid))
                    return null;
                var itemInfo = ProjectConfig
                    .Instance.GetEntityTableItemInfoByUUID(entityUuid);

                return itemInfo?.entityObject;
            }
        }
        private List<string> GetAllComponents()
        {
            return entityObject?.GetComponentsInChildren<T>(true).Select(x => x.name).ToList();
        }
#endif
        protected override IEnumerator OnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}