using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.DataTable
{
    public class TableSOBase : ScriptableObject
    {
        public virtual void Refresh() { }
        public virtual void Add() { }
    }
}
