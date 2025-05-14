using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using EFramework.Unity.Node;

[NodeCustomEditor(typeof(MessageNode2))]
public class MessageNode2View : BaseNodeView
{
	public override void Enable()
	{
		var node = nodeTarget as MessageNode2;

		var icon = EditorGUIUtility.IconContent("UnityLogo").image;
		AddMessageView("Custom message !", icon, Color.green);
	}
}