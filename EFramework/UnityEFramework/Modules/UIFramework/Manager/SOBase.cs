using EFramework.Unity.Utility;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity
{
    public class SOBase : ScriptableObject
    {
        public string Uuid; // 唯一标识符
        public string Name; // 名称
        public string Desc; // 描述
    }
}
