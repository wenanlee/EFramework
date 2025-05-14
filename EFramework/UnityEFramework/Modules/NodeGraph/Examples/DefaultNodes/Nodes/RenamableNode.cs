using UnityEngine;
using EFramework.Unity.Node;

[System.Serializable, NodeMenuItem("Custom/Renamable")]
public class RenamableNode : BaseNode
{
    [Output("Out")]
	public float		output;
	
    [Input("In")]
	public float		input;

	public override string name => "Renamable";

    public override bool isRenamable => true;

	protected override void Process() => output = input;
}