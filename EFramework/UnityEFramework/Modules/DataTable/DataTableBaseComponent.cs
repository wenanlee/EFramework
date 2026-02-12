using EFramework.Unity.ECS;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    [Serializable]
    public abstract class DataTableBaseComponent<TEntityVolumeSO, TTableVolume> : DataTableBase
                where TEntityVolumeSO : ScriptableObject
        where TTableVolume : DataTableItemInfoBase<TEntityVolumeSO>, new()
    {
        public abstract string parentPath { get; }
        [ShowInInspector, TableList(NumberOfItemsPerPage = 20, IsReadOnly = true, ShowPaging = true), Searchable]
        public List<TTableVolume> SOLst = new();
        public Dictionary<string, TTableVolume> SODict => SOLst.ToDictionary(i => i.uuid);
        public override void Add()
        {
            CreateScriptableObjectWindow.OpenWindow<TEntityVolumeSO>(
                ProjectConfig.Instance.soDataPath + parentPath + "/",
                (soName) =>
                {
                    Refresh();
                }
            );
        }
        public override void Refresh()
        {
            SOLst.Clear();
            var eventSOLst = ScriptableObjectUtility.FindAllScriptableObjects<TEntityVolumeSO>();
            foreach (var item in eventSOLst)
            {
                TTableVolume eventTableItem = new();
                eventTableItem.Init(item);
                SOLst.Add(eventTableItem);
            }
        }
    }
}
