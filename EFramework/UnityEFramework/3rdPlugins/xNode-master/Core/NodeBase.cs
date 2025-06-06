using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.XNode.Core
{
    public abstract class NodeBase : Node
    {
        public virtual void Execute()
        {
             GetNextNodes();
        }
        public virtual void GetNextNodes()
        {
            // 获取当前节点的输出端口
            NodePort outputPort = GetOutputPort("exit");

            // 检查是否有连接
            if (outputPort == null || !outputPort.IsConnected) return;

            // 遍历所有连接的节点
            foreach (NodePort connection in outputPort.GetConnections())
            {
                if (connection.node is NodeBase nextNode)
                {
                    nextNode.Execute();
                }
            }
        }
    }
    [Serializable]
    public class Empty
    {
        
    }
}
