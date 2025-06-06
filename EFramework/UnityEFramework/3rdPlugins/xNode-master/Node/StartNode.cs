using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
using EFramework.Unity.XNode.Core;
using Sirenix.OdinInspector;
using System;
[CreateNodeMenu("流程节点/开始节点"),NodeWidth(500)]
public class StartNode : EFramework.Unity.XNode.Core.StartNode {
    [ValueDropdown("@OdinDataBinding.EventSO.GetItems()"),LabelText("触发事件")]
    public string eventName;
}