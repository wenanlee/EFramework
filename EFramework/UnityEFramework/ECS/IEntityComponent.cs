using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class EntityComponentBase<T> where T : class
    {
        public virtual void EditorInit(T t) { }
        public virtual void Init(T t) { }
        [ButtonGroup("Tools")]
        [Button("Ìí¼Ó")]
        public virtual void Add() { }
        public virtual void Remove() { }
        [ButtonGroup("Tools")]
        [Button("Ë¢ÐÂ")]
        public virtual void Refresh() { }
    }
}
