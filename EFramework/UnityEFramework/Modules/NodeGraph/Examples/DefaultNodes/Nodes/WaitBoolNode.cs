using System;
using System.Collections;
using EFramework.Unity.Node;
using UnityEngine;

namespace NodeGraphProcessor.Examples
{
	[Serializable, NodeMenuItem("Functions/WaitBool")]
	public class WaitBoolNode : WaitableNode
	{
		public override string name => "WaitBool";

		[SerializeField, Input(name = "bool")]
		public bool b = false;

		protected override void Process()
		{
			//	We should check where this Process() called from. But i don't know if this is an elegant and performant way to do that.
			//	If this function is called from other than the ConditionalNode, then there will be problems, errors, unforeseen consequences, tears.
			// var isCalledFromConditionalProcessor = new StackTrace().GetFrame(5).GetMethod().ReflectedType == typeof(ConditionalProcessor);
			// if(!isCalledFromConditionalProcessor) return;

			//new ChainBase()
			//	.WaitFor(() => b)
			//	.Call(ProcessFinished)
			//	.Setup(new GameObject("Go").AddComponent<tmp>());
            new GameObject("Go").AddComponent<tmp>()
				.StartChain()
				.WaitFor(() => b)
				.Call(ProcessFinished);
            //StartChain()

        }
	}
    public class tmp:MonoBehaviour
    {
        
    }
}