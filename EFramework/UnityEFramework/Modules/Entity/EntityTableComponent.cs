using EFramework.Unity.DataTable;
using EFramework.Unity.Entity;
using EFramework.Unity.Event;
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
    /// <summary>
    /// 实体表组件
    /// </summary>
    [LabelText("实体表组件")]
    [Serializable]
    public class EntityTableComponent : DataTableBaseComponent
    {

        #region Properties
        [ShowInInspector, TableList(NumberOfItemsPerPage = 20, IsReadOnly = true, ShowPaging = true), Searchable]
        public List<EntityTableItemInfo> SOLst = new();
        /// <summary>
        /// 实体表字典，通过UUID索引
        /// </summary>
        public Dictionary<string, EntityTableItemInfo> SODict => SOLst.ToDictionary(i => i.uuid);

        /// <summary>
        /// 表名称
        /// </summary>
        public override string tableName => "实体表";

        #endregion

        #region Public Methods

        /// <summary>
        /// 通过UUID获取实体表项信息
        /// </summary>
        /// <param name="uuid">实体UUID</param>
        /// <returns>实体表项信息，如果不存在则返回null</returns>
        public EntityTableItemInfo GetEntityTableItemInfoByUUID(string uuid)
        {
            if (!SODict.ContainsKey(uuid))
                Refresh();

            return SODict.ContainsKey(uuid) ? SODict[uuid] : null;
        }

        #endregion

        #region Editor Methods

#if UNITY_EDITOR

        [Button("Refresh")]
        public override void Refresh()
        {
            SOLst.Clear();

            var entityObjDict = CollectEntityObjects();
            var processGraphDict = CollectProcessGraphs();

            Debug.Log($"找到预制体: {entityObjDict.Count}");

            foreach (var item in entityObjDict)
            {
                ProcessGraphBase processGraph = processGraphDict.ContainsKey(item.Key) ?
                    processGraphDict[item.Key] : null;

                EntityTableItemInfo entityInfo = new EntityTableItemInfo(item.Value, processGraph);
                SOLst.Add(entityInfo);
            }
        }

        /// <summary>
        /// 收集所有实体对象
        /// </summary>
        private Dictionary<string, GameEntity> CollectEntityObjects()
        {
            var entityObjDict = new Dictionary<string, GameEntity>();
            string prefabPath = ProjectConfig.Instance.prefabPath;

            Debug.Log(prefabPath);

            foreach (var entity in AssetDataUtility.GetAllPrefabs<GameEntity>(prefabPath))
            {
                if (entity.ComponentsVolume == null) continue;

                string uuid = entity.ComponentsVolume.Uuid;
                if (entityObjDict.ContainsKey(uuid))
                {
                    Debug.LogWarning($"存在重复的实体预制体UUID: {uuid} 预制体名称：{entity.name}");
                    continue;
                }

                entityObjDict.Add(uuid, entity);
            }

            return entityObjDict;
        }

        /// <summary>
        /// 收集所有流程图对象
        /// </summary>
        private Dictionary<string, ProcessGraphBase> CollectProcessGraphs()
        {
            var processGraphDict = new Dictionary<string, ProcessGraphBase>();

            foreach (var entity in ScriptableObjectUtility.FindAllScriptableObjects<ProcessGraphBase>())
            {
                if (processGraphDict.ContainsKey(entity.name))
                {
                    Debug.LogWarning($"存在重复的流程图名称: {entity.name}");
                    continue;
                }

                string uuid = entity.name.GetUUID();
                string key = string.IsNullOrEmpty(uuid) ? entity.name : uuid;
                processGraphDict.Add(key, entity);
            }

            return processGraphDict;
        }

#endif

        #endregion
    }

    /// <summary>
    /// 实体表项信息
    /// </summary>
    [Serializable]
    public class EntityTableItemInfo: DataTableItemInfoBase
    {
        #region Serialized Fields

        [ReadOnly, TableColumnWidth(120)]
        public string desc;

        [ReadOnly, TableColumnWidth(20)]
        public EntityVolume entityVolume;

        [ReadOnly, TableColumnWidth(20)]
        public GameEntity entityObject;

        [ReadOnly, TableColumnWidth(20)]
        public ProcessGraphBase processGraph;

        #endregion

        #region Constructor

        public EntityTableItemInfo(GameEntity entityObject, ProcessGraphBase processGraph)
        {
            this.uuid = entityObject.ComponentsVolume.Uuid;
            this.desc = entityObject.ComponentsVolume.Desc;
            this.entityVolume = entityObject.ComponentsVolume;
            this.entityObject = entityObject;
            this.processGraph = processGraph;
        }

        #endregion
    }
}