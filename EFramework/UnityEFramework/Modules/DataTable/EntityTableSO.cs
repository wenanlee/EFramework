using EFramework.Unity.Entity;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode.Core;

namespace EFramework.Unity.DataTable
{
    public class EntityTableSO : TableSOBase
    {
        [TableList(NumberOfItemsPerPage = 20, IsReadOnly = true, ShowPaging = true), Searchable]
        public List<EntityTableItemInfo> entityTableItemInfoLst = new List<EntityTableItemInfo>();
        public Dictionary<string, EntityTableItemInfo> entityTableDict=>entityTableItemInfoLst.ToDictionary(i=>i.uuid);
#if UNITY_EDITOR
        [Button("Refresh")]
        public override void Refresh()
        {
            entityTableItemInfoLst.Clear();
            var entityObjDict = new Dictionary<string, EntityObject>();
            var processGraphDict = new Dictionary<string, ProcessGraphBase>();
            Debug.Log(ProjectMgr.ProjectConfig.prefabPath);
            foreach (var entity in AssetDataUtility.GetAllPrefabs<EntityObject>(ProjectMgr.ProjectConfig.prefabPath))
            {
                if (entity.ComponentsVolume != null)
                {
                    if (entityObjDict.ContainsKey(entity.ComponentsVolume.Uuid))
                    {
                        Debug.LogWarning("存在重复的实体预制体UUID:" + entity.ComponentsVolume.Uuid + " 预制体名称：" + entity.name);
                        continue;
                    }
                    entityObjDict.Add(entity.ComponentsVolume.Uuid, entity);
                }
            }
            foreach (var entity in ScriptableObjectUtility.FindAllScriptableObjects<ProcessGraphBase>())
            {
                if (processGraphDict.ContainsKey(entity.name))
                {
                    Debug.LogWarning("存在重复的流程图名称:" + entity.name);
                    continue;
                }
                var uuid = entity.name.GetUUID();
                processGraphDict.Add(string.IsNullOrEmpty(uuid) ? entity.name : uuid, entity);
            }
            Debug.Log("找到预制体:" + entityObjDict.Count);
            foreach (var item in entityObjDict)
            {

                EntityTableItemInfo entityInfo = new EntityTableItemInfo(item.Value, processGraphDict.ContainsKey(item.Key) ? processGraphDict[item.Key] : null);
                entityTableItemInfoLst.Add(entityInfo);
            }
        }
#endif
        /// <summary>
        /// 获取实体表项信息通过UUID
        /// </summary>
        /// <param name="uuid">实体uuid</param>
        /// <returns>实体表项信息</returns>
        public EntityTableItemInfo GetEntityTableItemInfoByUUID(string uuid)
        {
            return entityTableItemInfoLst.FirstOrDefault(i => i.uuid == uuid);
        }
    }
    [Serializable]
    public class EntityTableItemInfo
    {
        [ReadOnly, TableColumnWidth(8)]
        public string uuid;
        [ReadOnly, TableColumnWidth(120)]
        public string desc;
        [ReadOnly, TableColumnWidth(20)]
        public EntityVolume entityVolume;
        [ReadOnly, TableColumnWidth(20)]
        public EntityObject entityObject;
        [ReadOnly, TableColumnWidth(20)]
        public ProcessGraphBase processGraph;
        public EntityTableItemInfo(EntityObject entityObject, ProcessGraphBase processGraph)
        {
            this.uuid = entityObject.ComponentsVolume.Uuid;
            this.desc = entityObject.ComponentsVolume.Desc;
            this.entityVolume = entityObject.ComponentsVolume;
            this.entityObject = entityObject;
            this.processGraph = processGraph;
        }
    }
}
