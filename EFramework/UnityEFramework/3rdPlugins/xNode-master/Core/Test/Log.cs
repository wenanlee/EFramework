using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
using EFramework.Unity.XNode.Core;
namespace EFramework.Unity.XNode.Test
{
    [CreateNodeMenu("Log", menuName = "打印节点", nodeName = "打印节点")]
    public class Log : TaskNode
    {

        [Input] // Input port for the node
        public string message;
        protected override void Init()
        {
            base.Init();
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }
    }
}
