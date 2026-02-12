using EFramework.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// GameEntity.cs
namespace EFramework.Unity.ECS
{
    public static class GameEntityHelpers
    {

        public static void CreateNewScriptableObject(GameEntity gameEntity)
        {
            var ComponentsVolumeSO = ScriptableObjectUtility.CreateScriptableObject<GameEntityVolumeSO>("Assets/" + "CompetentsVolume/", gameEntity.uuid);
            ComponentsVolumeSO.volume = new();
            ComponentsVolumeSO.volume.uuid = gameEntity.uuid;
            gameEntity.ComponentsVolumeSO = ComponentsVolumeSO;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(ComponentsVolumeSO);
#endif
            
        }
    }
}