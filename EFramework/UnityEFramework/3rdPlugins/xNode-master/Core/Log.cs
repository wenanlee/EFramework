using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;

namespace EFramework.Unity.XNode.Core
{
    [CreateNodeMenu("系统节点/打印节点", nodeName = "打印节点")]
    public class Log : ProcessNodeBase
    {
        public string message = "";
        public override void Execute()
        {
            Debug.Log($">>> {message}");
            base.Execute();
        }
    }
}
