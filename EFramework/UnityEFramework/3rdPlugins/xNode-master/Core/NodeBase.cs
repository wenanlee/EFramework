using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.XNode.Core
{
    /// <summary>
    /// 节点基类，提供节点图执行功能
    /// </summary>
    public abstract class NodeBase : Node
    {
        #region 节点连接管理

        /// <summary>
        /// 所有前置节点及其完成状态
        /// </summary>
        [NonSerialized]
        protected Dictionary<NodeBase, bool> predecessorStatus = new Dictionary<NodeBase, bool>();

        /// <summary>
        /// 所有后置节点
        /// </summary>
        [NonSerialized]
        protected List<NodeBase> successors = new List<NodeBase>();

        #endregion

        #region 生命周期方法
        
        /// <summary>
        /// 初始化节点
        /// </summary>
        public virtual void Initialize()
        {
            predecessorStatus.Clear();
            successors.Clear();

            // 收集前置节点
            CollectPredecessors();
            // 收集后置节点
            CollectSuccessors();
        }

        /// <summary>
        /// 重置节点状态
        /// </summary>
        public virtual void ResetNode()
        {
            // 重置所有前置节点状态
            var keys = new List<NodeBase>(predecessorStatus.Keys);
            foreach (var node in keys)
            {
                predecessorStatus[node] = false;
            }
        }

        /// <summary>
        /// 执行节点协程
        /// </summary>
        public virtual IEnumerator Execute()
        {
            // 检查前置节点是否全部完成
            if (!CheckPredecessorsCompleted())
            {
                yield break;
            }
            try
            {
                // 执行节点逻辑
                yield return OnExecute();
            }
            finally
            {
                // 无论成功与否，触发后续节点
                TriggerSuccessors();
            }
        }

        #endregion

        #region 抽象方法

        /// <summary>
        /// 节点执行逻辑（子类实现）
        /// </summary>
        protected abstract IEnumerator OnExecute();

        #endregion

        #region 节点连接管理

        /// <summary>
        /// 收集所有前置节点
        /// </summary>
        protected virtual void CollectPredecessors()
        {
            NodePort inputPort = GetInputPort("enter");
            if (inputPort == null || !inputPort.IsConnected) return;

            foreach (NodePort connection in inputPort.GetConnections())
            {
                if (connection.node is NodeBase prevNode && !predecessorStatus.ContainsKey(prevNode))
                {
                    predecessorStatus.Add(prevNode, false);
                }
            }
        }

        /// <summary>
        /// 收集所有后置节点
        /// </summary>
        protected virtual void CollectSuccessors()
        {
            NodePort outputPort = GetOutputPort("exit");
            if (outputPort == null || !outputPort.IsConnected) return;

            foreach (NodePort connection in outputPort.GetConnections())
            {
                if (connection.node is NodeBase nextNode && !successors.Contains(nextNode))
                {
                    successors.Add(nextNode);
                }
            }
        }

        /// <summary>
        /// 检查所有前置节点是否完成
        /// </summary>
        protected virtual bool CheckPredecessorsCompleted()
        {
            // 如果没有前置节点，直接返回true
            if (predecessorStatus.Count == 0)
                return true;

            // 检查所有前置节点是否完成
            foreach (var status in predecessorStatus.Values)
            {
                if (!status)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 通知前置节点已完成
        /// </summary>
        public virtual void NotifyPredecessorCompleted(NodeBase predecessor)
        {
            Initialize(); // 确保节点已初始化
            if (predecessorStatus.ContainsKey(predecessor))
            {
                predecessorStatus[predecessor] = true;
            }
        }

        /// <summary>
        /// 触发后续节点执行
        /// </summary>
        protected virtual void TriggerSuccessors()
        {
            if (successors.Count == 0) return;

            foreach (var nextNode in successors)
            {
                // 通知后续节点本节点已完成
                nextNode.NotifyPredecessorCompleted(this);
                // 启动后续节点
                NodeTempMonoBehaviour.Instance.StartCoroutine(nextNode.Execute());
            }
        }

        #endregion
    }

    /// <summary>
    /// 序列化占位类
    /// </summary>
    [Serializable]
    public class Empty { }
}