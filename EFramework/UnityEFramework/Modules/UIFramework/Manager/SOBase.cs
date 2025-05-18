using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    public class SOBase : ScriptableObject
    {
        public string jsonPath;
        public string jsonName;
        [Button]
        public virtual void LoadFromJson()
        {
            
        }
        [Button]
        public virtual void SaveJson()
        {

        }
    }
}
