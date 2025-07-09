using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.XNode.Core
{
    [CreateAssetMenu(fileName = "ProcessGraph", menuName = "EFramework/ProcessGraph")]
    public class ProcessGraphBase : NodeGraph
    {
        public virtual void Trigger()
        {
            // 遍历所有节点
            foreach (var node in nodes)
            {
                if (node == null) continue;
                // 检查节点类型是否为任务节点
                if (node is StartNode startNode)
                {
                    startNode.Initialize();
                    NodeTempMonoBehaviour.Instance.StartCoroutine(startNode.Execute());
                    //// 检查事件名称是否匹配
                    //if (startNode. == eventName)
                    //{
                    //    // 执行任务节点的逻辑
                    //    startNode.Execute();
                    //}
                }
            }
        }
    }
}

