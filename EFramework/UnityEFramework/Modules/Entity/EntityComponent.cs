using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace EFramework.Unity.Entity
{
    [Serializable]
    public abstract class EntityComponent:EntityComponentBase<GameEntity>
    {
        [ToggleLeft,GUIColor("@Enabled ? Color.white : Color.Color.yellow")]
        public bool Enabled = true;
        protected GameEntity entityObject;
        /// <summary>
        /// 编辑器中用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        public override void EditorInit(GameEntity entity)
        {
            entityObject = entity;
        }
        /// <summary>
        /// 运行时用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        public override void Init(GameEntity entity) 
        {
            entityObject = entity;
        }

        public T Clone<T>() where T : EntityComponent
        {
            var originalJson = JsonUtility.ToJson(this);
            T copy = JsonUtility.FromJson<T>(originalJson);
            return copy;
        }

    }
}
