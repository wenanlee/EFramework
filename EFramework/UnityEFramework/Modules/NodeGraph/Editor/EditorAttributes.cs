using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EFramework.Unity.Node
{
	[AttributeUsage(AttributeTargets.Class)]
	public class NodeCustomEditor : Attribute
	{
		public Type nodeType;

		public NodeCustomEditor(Type nodeType)
		{
			this.nodeType = nodeType;
		}
	}
}