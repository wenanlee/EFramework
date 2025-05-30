using EFramework.Unity.Utility;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    public class SOBase : ScriptableObject
    {
        [NaButton]
        public virtual void ReLoadSO()
        {

        }
        [NaButton]
        public virtual void LoadFromJson()
        {
            
        }
        [NaButton]
        public virtual void SaveJson()
        {

        }
    }
}
