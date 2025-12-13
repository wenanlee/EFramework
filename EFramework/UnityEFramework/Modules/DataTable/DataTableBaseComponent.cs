using EFramework.Unity.Entity;
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
    }
}
