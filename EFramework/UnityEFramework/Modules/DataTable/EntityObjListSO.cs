using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    //public class EntityTableSO : ScriptableObject
    //{
    //    public List<EntityItemInfo> entityItemInfos = new List<EntityItemInfo>();
    //}
    [Serializable]
    public class EntityItemInfo
    {
        public string uuid;
        public string name;
        public string desc;
        public UnityEngine.Object prefab;
        public UnityEngine.Object info;
        public UnityEngine.Object graph;
        public EntityItemInfo(UnityEngine.Object prefab)
        {
            this.prefab = prefab;
        }
    }
}
