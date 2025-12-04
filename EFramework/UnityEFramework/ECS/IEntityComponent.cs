using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class EntityComponentBase<T> where T : class
    {
        [ButtonGroup("基础功能")]
        public virtual void EditorInit(T t) { }
        [ButtonGroup("基础功能")]
        public virtual void Init(T t) { }
        [ButtonGroup("基础功能")]
        public virtual void Add() { }
        [ButtonGroup("基础功能")]
        public virtual void Remove() { }
        [ButtonGroup("基础功能")]
        public virtual void Refresh() { }
    }
}
