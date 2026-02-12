// GameEntity.cs
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity.ECS
{
    public class GameEntity : MonoBehaviour
    {
        public string uuid;
        [HideIf("@UnityEngine.Application.isPlaying"), LabelText("Components(编辑器)")]
        [InlineEditor(Expanded = true), SerializeField]
        [InlineButton("@GameEntityHelpers.CreateNewScriptableObject(this)", "新建", ShowIf = "@ComponentsVolumeSO == null")]
        public GameEntityVolumeSO ComponentsVolumeSO;
        [ShowIf("@UnityEngine.Application.isPlaying"), LabelText("Components(运行时)")]
        public GameEntityVolume ComponentsVolume;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = UUID.New();
            }
            if (ComponentsVolumeSO == null || ComponentsVolumeSO.volume == null || ComponentsVolumeSO.volume.uuid != uuid)
            {
                ComponentsVolumeSO = ScriptableObjectUtility.FindScriptableObjectByUUID<GameEntityVolumeSO>(uuid);
            }
        }
    }
}