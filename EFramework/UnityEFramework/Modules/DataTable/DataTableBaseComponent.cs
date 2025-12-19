using EFramework.Unity.Entity;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    [Serializable]
    public abstract class DataTableBaseComponent : EntityComponentBase<string>
    {
        public abstract string tableName { get; }
        [ButtonGroup("Tools")]
        [Button("导出到Json")]
        public abstract void ExportToJson();
        [ButtonGroup("Tools")]
        [Button("生成枚举文件")]
        public abstract void GenerateToEnumFile();
    }
}
