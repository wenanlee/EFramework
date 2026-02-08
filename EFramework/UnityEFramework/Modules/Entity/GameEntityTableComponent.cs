using EFramework.Unity.Entity;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System;
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
    public class GameEntityTableComponent : DataTableBaseComponent
    {

        #region Properties
        [ShowInInspector, TableList(NumberOfItemsPerPage = 20, IsReadOnly = true, ShowPaging = true), Searchable]
        public List<GameEntityTableItemInfo> SOLst = new();
        /// <summary>
        /// 实体表字典，通过UUID索引
        /// </summary>
        public Dictionary<string, GameEntityTableItemInfo> SODict => SOLst.ToDictionary(i => i.uuid);

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
        public GameEntityTableItemInfo GetEntityTableItemInfoByUUID(string uuid)
        {
            if (!SODict.ContainsKey(uuid))
                Refresh();

            return SODict.ContainsKey(uuid) ? SODict[uuid] : null;
        }

        #endregion

        #region Editor Methods

#if UNITY_EDITOR

        [Button("刷新")]
        public override void Refresh()
        {
            SOLst.Clear();

            var entityObjDict = CollectEntityObjects();
            var entityVolumeSODict = CollectEntityVolumeSO();

            Debug.Log($"找到预制体: {entityObjDict.Count}");

            foreach (var item in entityObjDict)
            {
                var entityVolumeSO = entityVolumeSODict.ContainsKey(item.Key) ?
                    entityVolumeSODict[item.Key] : null;

                GameEntityTableItemInfo entityInfo = new GameEntityTableItemInfo(item.Value, entityVolumeSO);
                SOLst.Add(entityInfo);
            }
        }
        [Button("添加")]
        public override void Add()
        {
            base.Add();

        }

        /// <summary>
        /// 收集所有实体对象
        /// </summary>
        private Dictionary<string, GameEntity> CollectEntityObjects()
        {
            var entityObjDict = new Dictionary<string, GameEntity>();
            string prefabPath = ProjectConfig.Instance.projectParentPath;

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
        private Dictionary<string, GameEntityVolumeSO> CollectEntityVolumeSO()
        {
            var gameEntityVolumeSODict = new Dictionary<string, GameEntityVolumeSO>();
            foreach (var entity in ScriptableObjectUtility.FindAllScriptableObjects<GameEntityVolumeSO>())
            {
                if (gameEntityVolumeSODict.ContainsKey(entity.name))
                {
                    Debug.LogWarning($"存在重复的名称: {entity.name}");
                    continue;
                }

                gameEntityVolumeSODict.Add(entity.name, entity);
            }

            return gameEntityVolumeSODict;
        }



#endif
        public override void ExportToJson()
        {
        }

        public override void GenerateToEnumFile()
        {
            FileUtility.GenerateConstantsFile(ProjectConfig.Instance.projectParentPath, "EntityItems", SOLst.ToDictionary(x => $"{x.entityObject.name.Substring(0, 2)}_{x.desc}_{x.uuid}", v => v.uuid));
        }
        #endregion
    }

    /// <summary>
    /// 实体表项信息
    /// </summary>
    [Serializable]
    public class GameEntityTableItemInfo : DataTableItemInfoBase
    {
        #region Serialized Fields

        [ReadOnly, TableColumnWidth(120)]
        public string desc;

        [HideInInspector]
        public GameEntityVolume entityVolume;
        [ReadOnly, TableColumnWidth(20)]
        public GameEntityVolumeSO entityVolumeSO;

        [ReadOnly, TableColumnWidth(20)]
        public GameEntity entityObject;

        #endregion

        #region Constructor

        public GameEntityTableItemInfo(GameEntity entityObject, GameEntityVolumeSO entityVolumeSO)
        {
            this.uuid = entityObject.ComponentsVolume.Uuid;
            this.desc = entityObject.ComponentsVolume.Desc;
            this.entityVolume = entityObject.ComponentsVolume;
            this.entityObject = entityObject;
            this.entityVolumeSO = entityVolumeSO;
        }

        #endregion
    }
}