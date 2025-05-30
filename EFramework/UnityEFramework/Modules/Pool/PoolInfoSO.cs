using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Pool
{
    [CreateAssetMenu(fileName = "PoolInfo", menuName = "EFramework/Pool/PoolInfo", order = 0)]
    public class PoolInfoSO :ScriptableObject
    {
        [NaLabel("池ID")]
        public string poolId;
        [NaLabel("池名称")]
        public string poolName;
        [NaLabel("池容器")]
        public string hpContainerId;
        [NaLabel("池预制体")]
        public string hpTemplateId;
        [NaLabel("池类型")]
        public string poolType;
    }
}
