using UnityEngine;
using EFramework.Unity.Node;
using NodeGraphProcessor.Examples;

public class RuntimeConditionalGraph : MonoBehaviour
{
	[Header("Graph to Run on Start")]
	public BaseGraph graph;
	public bool b;
	private ConditionalProcessor processor;
	private void Start()
	{
		if(graph != null)
			processor = new ConditionalProcessor(graph);
		processor.Run();
		this.StartChain().Log("开始").WaitFor(()=>b).Log("按下");
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
			//graph.SetParameterValue("start", true);
			b = true;

        }
    }
}