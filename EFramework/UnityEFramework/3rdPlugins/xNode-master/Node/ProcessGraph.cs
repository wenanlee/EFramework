using EFramework.Unity.XNode.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "流程图", menuName = "MJ/流程图", order = 1)]
public class ProcessGraph : ProcessGraphBase
{
    public List<StartNode> startNodes = new List<StartNode>();
    public void GetAllStartNode()
    {
        // 清空当前节点列表
        startNodes.Clear();
        // 遍历所有节点
        foreach (var node in nodes)
        {
            if (node == null) continue;
            // 检查节点类型是否为任务节点
            if (node is StartNode startNode)
            {
                // 添加到列表中
                startNodes.Add(startNode);
            }
        }
    }
    public override void Trigger(string eventName)
    {
        // 遍历所有节点
        foreach (var node in startNodes)
        {
            if(node.eventName == eventName)
            {
                node.Execute();
            }
        }

    }
}
