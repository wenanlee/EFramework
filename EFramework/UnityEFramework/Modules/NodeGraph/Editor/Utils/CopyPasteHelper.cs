using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Node
{
	[System.Serializable]
	public class CopyPasteHelper
	{
		public List< JsonElement >	copiedNodes = new List< JsonElement >();

		public List< JsonElement >	copiedGroups = new List< JsonElement >();
	
		public List< JsonElement >	copiedEdges = new List< JsonElement >();
	}
}