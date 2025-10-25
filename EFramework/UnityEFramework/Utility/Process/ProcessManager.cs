using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFramework.Process
{
    /// <summary>
    /// 流程管理器，负责游戏各阶段的流程控制
    /// 使用单例模式确保全局唯一访问点
    /// </summary>
    public class ProcedureManager : MonoSingleton<ProcedureManager>
    {
        /// <summary>
        /// 当前正在执行的流程类型
        /// </summary>
        public ProcedureType CurrentProcess { get; private set; }

        /// <summary>
        /// 存储各流程对应的处理器方法
        /// Key: 流程类型
        /// Value: 该流程下要执行的方法列表
        /// </summary>
        private readonly Dictionary<ProcedureType, List<Action>> processHandlers = 
            new Dictionary<ProcedureType, List<Action>>();

        private MonoBehaviour[] behaviours; // 场景中所有MonoBehaviour组件缓存

        /// <summary>
        /// 初始化方法，在单例创建时调用
        /// </summary>
        public override void Init()
        {
            base.Init();
            LoadProcessHandlers(); // 加载所有流程处理器
        }

        /// <summary>
        /// 执行指定流程的所有处理器
        /// </summary>
        /// <param name="processType">要执行的流程类型</param>
        /// <returns>等待0.1秒的协程</returns>
        public IEnumerator ExecuteProcess(ProcedureType processType)
        {
            CurrentProcess = processType; // 更新当前流程状态
            
            // 检查该流程是否有注册的处理器
            if (processHandlers.TryGetValue(processType, out var handlers))
            {
                // 执行该流程的所有处理器方法
                foreach (var handler in handlers)
                {
                    handler.Invoke();
                }
            }
            
            // 每个流程执行后等待0.1秒
            yield return new WaitForSeconds(0.1f);
        }

        /// <summary>
        /// 加载并注册所有流程处理器
        /// </summary>
        private void LoadProcessHandlers()
        {
            // 获取场景中所有MonoBehaviour组件
            behaviours = FindObjectsOfType<MonoBehaviour>();
            
            // 初始化流程处理器字典，为每个流程类型创建空列表
            foreach (ProcedureType type in Enum.GetValues(typeof(ProcedureType)))
            {
                // 跳过None类型
                if (type == ProcedureType.None) continue;
                processHandlers[type] = new List<Action>();
            }

            // 注册各流程的处理器方法
            // 游戏启动流程
            RegisterHandlers<I_100_Logo>(ProcedureType._0_Logo, x => x.OnLogo());
            RegisterHandlers<I_101_Loading>(ProcedureType._1_Loading, x => x.OnLoading());
            RegisterHandlers<I_102_GameLoadData>(ProcedureType._2_GameLoadData, x => x.OnGameLoadData());
            RegisterHandlers<I_103_GameLoadDataComplete>(ProcedureType._3_GameLoadDataComplete, x => x.OnGameLoadDataComplete());
            RegisterHandlers<I_104_GameInit>(ProcedureType._4_GameInit, x => x.OnGameInit());
            RegisterHandlers<I_105_GameStart>(ProcedureType._5_GameStart, x => x.OnGameStart());
            
            // 场景加载流程
            RegisterHandlers<I_110_SceneLoad>(ProcedureType._10_SceneLoad, x => x.OnSceneLoad());
            RegisterHandlers<I_111_SceneLoadComplete>(ProcedureType._11_SceneLoadComplete, x => x.OnSceneLoadComplete());
            RegisterHandlers<I_112_SceneLoadData>(ProcedureType._12_SceneLoadData, x => x.OnSceneLoadData());
            RegisterHandlers<I_113_SceneLoadDataComplete>(ProcedureType._13_SceneLoadDataComplete, x => x.OnSceneLoadDataComplete());
            RegisterHandlers<I_114_SceneInit>(ProcedureType._14_SceneInit, x => x.OnSceneInit());
            RegisterHandlers<I_115_SceneStart>(ProcedureType._15_SceneStart, x => x.OnSceneStart());
        }

        /// <summary>
        /// 注册指定类型组件的处理器方法
        /// </summary>
        /// <typeparam name="T">处理器接口类型</typeparam>
        /// <param name="processType">关联的流程类型</param>
        /// <param name="handler">要执行的处理方法</param>
        private void RegisterHandlers<T>(ProcedureType processType, Action<T> handler) where T : class
        {
            // 查找所有实现指定接口的组件
            var components = behaviours.OfType<T>().ToList();
            
            // 为每个找到的组件创建委托并添加到处理器列表
            foreach (var component in components)
            {
                processHandlers[processType].Add(() => handler(component));
            }
        }
    }

    /// <summary>
    /// 游戏流程类型枚举
    /// </summary>
    public enum ProcedureType
    {
        /// <summary>
        /// 未定义流程
        /// </summary>
        None = -1,
        
        // ====== 游戏启动流程 ======
        /// <summary>
        /// Logo展示阶段
        /// </summary>
        _0_Logo = 0,
        /// <summary>
        /// 加载界面阶段
        /// </summary>
        _1_Loading,
        /// <summary>
        /// 游戏数据加载阶段
        /// </summary>
        _2_GameLoadData,
        /// <summary>
        /// 游戏数据加载完成阶段
        /// </summary>
        _3_GameLoadDataComplete,
        /// <summary>
        /// 游戏初始化阶段
        /// </summary>
        _4_GameInit,
        /// <summary>
        /// 游戏开始阶段
        /// </summary>
        _5_GameStart,

        // ====== 场景加载流程 ======
        /// <summary>
        /// 场景加载阶段
        /// </summary>
        _10_SceneLoad = 10,
        /// <summary>
        /// 场景加载完成阶段
        /// </summary>
        _11_SceneLoadComplete,
        /// <summary>
        /// 场景数据加载阶段
        /// </summary>
        _12_SceneLoadData,
        /// <summary>
        /// 场景数据加载完成阶段
        /// </summary>
        _13_SceneLoadDataComplete,
        /// <summary>
        /// 场景初始化阶段
        /// </summary>
        _14_SceneInit,
        /// <summary>
        /// 场景开始阶段
        /// </summary>
        _15_SceneStart,
    }
}