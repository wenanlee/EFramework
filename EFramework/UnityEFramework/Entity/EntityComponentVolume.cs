using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace EFramework.Unity.Entity
{
    [CreateAssetMenu(fileName = "EntityComponentVolume", menuName = "EntityComponentVolume")]
    public class EntityComponentVolume : ScriptableObject
    {
        [SerializeReference]
        public List<EntityComponent> Components;
    }

    [Serializable]
    public abstract class EntityComponent
    {
        public abstract string name { get; }
        public bool enabled = true;
        public virtual void Init()
        {
            if (enabled == false)
                return;
            Debug.Log(name);
        }
        public abstract void Execute(object obj);
    }
    [Serializable]
    public class TestComponet : EntityComponent
    {
        public string testStr;

        public override string name => "≤‚ ‘";

        public override void Execute(object obj)
        {
            if (enabled == false)
                return;
            Debug.Log(obj + " " + testStr);
        }
    }
}