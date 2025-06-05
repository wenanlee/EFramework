using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
using EFramework.Core;
namespace EFramework.Unity.XNode.Core
{
    [CreateNodeMenu("系统/流程节点",nodeName ="流程节点")]
    public class ProcessNode : TaskNodeBase
    {
        public string ProcessName = "";
        public void StartProcess()=> EventManager.SendEvent("ProcessStart", ProcessName);
        public void EndProcess() => EventManager.SendEvent("ProcessEnd", ProcessName);
    }
}
