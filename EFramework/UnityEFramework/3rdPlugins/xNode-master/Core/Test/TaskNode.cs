using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateNodeMenu("TaskNode", menuName = "任务节点")]
public class TaskNode : EFramework.Unity.XNode.Core.TaskNodeBase
{
    protected override void Init()
    {
        base.Init();
        name = "任务节点";
    }
}
