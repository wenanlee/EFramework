using EditorAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Pool
{
    [CreateAssetMenu(fileName = "PoolInfo", menuName = "EFramework/Pool/PoolInfo", order = 0)]
    public class PoolInfoSO :ScriptableObject
    {
        [Rename("池ID")]
        public string poolId;
        [Rename("池名称")]
        public string poolName;
        [Rename("池容器")]
        public string hpContainerId;
        [Rename("池预制体")]
        public string hpTemplateId;
        [Rename("池类型")]
        public string poolType;
    }
}
