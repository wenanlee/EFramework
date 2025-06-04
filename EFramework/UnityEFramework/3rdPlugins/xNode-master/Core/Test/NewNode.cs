using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
using EFramework.Unity.XNode.Core;
using Sirenix.OdinInspector;
[CreateNodeMenu("StartNode", menuName = "开始节点")]
public class NewNode : StartNode {

	[ValueDropdown(nameof(GetNodeIds)),LabelText("触发事件")]
    public string eventId;
	
    protected override void Init() {
		base.Init();
        name = "开始节点";
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
	public IEnumerable<ValueDropdownItem<string>> GetNodeIds()
	{
        return new List<ValueDropdownItem<string>>() { 
            new ValueDropdownItem<string>("开始","OnStart"),
            new ValueDropdownItem<string>( "游戏初始化","OnGameInit"),
            new ValueDropdownItem<string>("互动", "OnInteration")
        };
    }
}