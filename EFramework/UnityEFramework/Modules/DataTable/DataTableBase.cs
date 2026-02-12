using EFramework.Unity.ECS;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    [Serializable]
    public abstract class DataTableBase : ComponentBase
    {
        public abstract string TableName { get; }

        public virtual void Refresh()
        {
            
        }
        public virtual void Add()
        {

        }
        public virtual void ExportToJson()
        { 

        }
        public virtual void GenerateToEnumFile() 
        { 

        }
    }
}
