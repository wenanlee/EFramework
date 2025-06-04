using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
[CreateNodeMenu("MovePlayerPosition", menuName = "移动玩家位置")]
public class MovePlayerPosition : TaskNode {
	[Input]
	public Vector3 Player;
	// Use this for initialization
	protected override void Init() {
		base.Init();
		name = "移动玩家位置";
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}