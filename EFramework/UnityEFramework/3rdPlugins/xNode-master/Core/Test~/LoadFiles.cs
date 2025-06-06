using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;
[CreateNodeMenu("系统节点/加载文件节点")]
public class LoadFiles : ValueNodeBase<string> {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		name = "加载文件节点";
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}