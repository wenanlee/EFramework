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
    public class ComponentNodeBase<T> : ProcessNodeBase where T : Component
    {
        [ValueDropdown("@OdinDataBinding.GetEntityIDs(graph.name)", DropdownWidth = 600)]
        [LabelText("绑定实体ID")]
        public string objUuid; // 目标实体UUID
        [ValueDropdown("@GetAllComponents()", DropdownWidth = 600)]
        [LabelText("实例名称")]
        public string componentObjName;
        [HideInInspector]
        public T component;
        private GameEntity entityObj;
#if UNITY_EDITOR
        [ShowInInspector, ReadOnly, LabelText("绑定实体"), HideIf("@string.IsNullOrEmpty(objUuid)||objUuid==\"None\"")]
        public GameEntity EntityObject
        {
            get
            {
                if (string.IsNullOrEmpty(objUuid))
                    return null;
                if(ProjectConfig.Instance == null)
                    Debug.LogError("ProjectMgr 示例为空!");
                return ProjectConfig.Instance.volume.GetComponentVolume<GameEntityTableComponent>().GetEntityTableItemInfoByUUID(objUuid)?.entityObject;
            }
        }
        [ShowInInspector,ReadOnly]
        public T Component
        {
            get
            {
                return EntityObject?.gameObject.GetComponentInChildrenByName<T>(componentObjName);
            }
        }
        private List<string> GetAllComponents()
        {
            return EntityObject?.GetComponentsInChildren<T>(true).Select(x => x.name).ToList();
        }
#endif
        protected override IEnumerator OnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
