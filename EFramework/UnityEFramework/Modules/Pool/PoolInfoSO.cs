using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFramework.Unity.Pool
{
    [CreateAssetMenu(fileName = "PoolInfo", menuName = "EFramework/Pool/PoolInfo", order = 0)]
    public class PoolInfoSO :ScriptableObject
    {
        [LabelText("池ID")]
        public string poolId;
        [LabelText("池名称")]
        public string poolName;
        [LabelText("池容器")]
        public string hpContainerId;
        [LabelText("池预制体")]
        public string hpTemplateId;
        [LabelText("池类型")]
        public string poolType;
    }
}
