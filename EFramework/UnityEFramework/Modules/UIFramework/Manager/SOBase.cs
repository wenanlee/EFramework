using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    public class SOBase : ScriptableObject
    {
        [Button, ButtonGroup]
        public virtual void ReLoadSO()
        {

        }
        [Button,ButtonGroup]
        public virtual void LoadFromJson()
        {
            
        }
        [Button,ButtonGroup]
        public virtual void SaveJson()
        {

        }
    }
}
