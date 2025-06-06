using EFramework.Unity.XNode.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateNodeMenu("时间节点/等待n秒")]
public class WaitSecondsNode : ProcessNodeBase
{
    public float waitTime = 1f;
    public override void Execute()
    {
        NodeTempMonoBehaviour.Instance.Delay(waitTime, () => { base.Execute(); });
    }
}
[CreateNodeMenu("时间节点/等待n帧")]
public class WaitFramesNode:ProcessNodeBase
{
    public int waitFrames = 1;
    public override void Execute()
    {
        NodeTempMonoBehaviour.Instance.Delay(waitFrames, () => {base.Execute(); });
    }
}
public class NodeTempMonoBehaviour : MonoSingleton<NodeTempMonoBehaviour>
{

}