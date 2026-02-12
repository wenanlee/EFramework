using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    public class DataTableItemInfoBase<TEntityVolumeSO> where TEntityVolumeSO : ScriptableObject
    {
        [ReadOnly, TableColumnWidth(8)]
        public string uuid;

        public void Init(TEntityVolumeSO item)
        {

        }
    }
}
