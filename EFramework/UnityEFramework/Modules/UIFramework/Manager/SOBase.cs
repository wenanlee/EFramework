using EditorAttributes;
using EFramework.Unity.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    public class SOBase : ScriptableObject
    {
        [Button]
        public virtual void ReLoadSO()
        {

        }
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
