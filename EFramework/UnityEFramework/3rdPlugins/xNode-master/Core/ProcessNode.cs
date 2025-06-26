using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
using EFramework.Core;
namespace EFramework.Unity.XNode.Core
{
    [CreateNodeMenu("流程节点/基础流程节点(占位用没有实际功能)", order = 1)]
    public class ProcessNode : ProcessNodeBase
    {
        [TextArea]
        public string text = "";

        protected override IEnumerator OnExecute()
        {
            yield return null;
        }
    }
}
