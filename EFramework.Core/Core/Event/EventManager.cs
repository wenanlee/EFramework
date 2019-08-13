using System.Collections;
using System.Collections.Generic;
using System;
using EFramework.Core;
namespace EFramework.Core
{
    public class EventManager:Singleton<EventManager>
    {
        private List<IEventAgent> gameEventAgentList = new List<IEventAgent>(30);//根据项目中的事件数量调整

        public void AddEventAgent(IEventAgent agent)
        {
            gameEventAgentList.Add(agent);
        }

        //----------------------------------------无参--------------------------------------------
        public static void AddListener(Enum eid, Action action)
        {
            EventAgent.Instance.AddListener(eid, action);
        }

        public static void AddListener(Events.Event gameEvent, Action action)
        {
            EventAgent.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent(Events.Event gameEvent)
        {
            EventAgent.Instance.Invoke(gameEvent.eid);
        }

        public static void SendEvent(Enum eid)
        {
            EventAgent.Instance.Invoke(eid);
        }

        public static void RemoveListener(Enum eid, Action action)
        {
            EventAgent.Instance.RemoveListener(eid, action);
        }

        public static void RemoveListener(Events.Event gameEvent, Action action)
        {
            EventAgent.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener(Events.Event gameEvent)
        {
            return EventAgent.Instance.CheckHaveListener(gameEvent.eid);
        }

        //----------------------------------T参数-----------------------------------------------
        public static void AddListener<T>(Enum eid, Action<T> action)
        {
            EventAgent<T>.Instance.AddListener(eid, action);
        }

        public static void AddListener<T>(Events.Event<T> gameEvent, Action<T> action)
        {
            EventAgent<T>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T>(Events.Event<T> gameEvent, T param)
        {
            EventAgent<T>.Instance.Invoke(gameEvent.eid, param);
        }

        public static void SendEvent<T>(Enum eid, T param)
        {
            EventAgent<T>.Instance.Invoke(eid, param);
        }

        public static void RemoveListener<T>(Enum eid, Action<T> action)
        {
            EventAgent<T>.Instance.RemoveListener(eid, action);
        }

        public static void RemoveListener<T>(Events.Event<T> gameEvent, Action<T> action)
        {
            EventAgent<T>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T>(Events.Event<T> gameEvent)
        {
            return EventAgent<T>.Instance.CheckHaveListener(gameEvent.eid);
        }

        //-----------------------------T1 T2参数-------------------------------------------------
        public static void AddListener<T1, T2>(Enum eid, Action<T1, T2> action)
        {
            EventAgent<T1, T2>.Instance.AddListener(eid, action);
        }

        public static void AddListener<T1, T2>(Events.Event<T1, T2> gameEvent, Action<T1, T2> action)
        {
            EventAgent<T1, T2>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T1, T2>(Events.Event<T1, T2> gameEvent, T1 param1, T2 param2)
        {
            EventAgent<T1, T2>.Instance.Invoke(gameEvent.eid, param1, param2);
        }

        public static void SendEvent<T1, T2>(Enum eid, T1 param1, T2 param2)
        {
            EventAgent<T1, T2>.Instance.Invoke(eid, param1, param2);
        }

        public static void RemoveListener<T1, T2>(Enum eid, Action<T1, T2> action)
        {
            EventAgent<T1, T2>.Instance.RemoveListener(eid, action);
        }

        public static void RemoveListener<T1, T2>(Events.Event<T1, T2> gameEvent, Action<T1, T2> action)
        {
            EventAgent<T1, T2>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T1, T2>(Events.Event<T1, T2> gameEvent)
        {
            return EventAgent<T1, T2>.Instance.CheckHaveListener(gameEvent.eid);
        }

        //--------------------------------------------------------------------------------------
        public static void AddListener<T1, T2, T3>(Enum eid, Action<T1, T2, T3> action)
        {
            EventAgent<T1, T2, T3>.Instance.AddListener(eid, action);
        }

        public static void AddListener<T1, T2, T3>(Events.Event<T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
        {
            EventAgent<T1, T2, T3>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T1, T2, T3>(Events.Event<T1, T2, T3> gameEvent, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<T1, T2, T3>.Instance.Invoke(gameEvent.eid, param1, param2, param3);
        }
        public static void SendEvent<T1, T2, T3>(Enum eid, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<T1, T2, T3>.Instance.Invoke(eid, param1, param2, param3);
        }
        public static void RemoveListener<T1, T2, T3>(Enum eid, Action<T1, T2, T3> action)
        {
            EventAgent<T1, T2, T3>.Instance.RemoveListener(eid, action);
        }

        public static void RemoveListener<T1, T2, T3>(Events.Event<T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
        {
            EventAgent<T1, T2, T3>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T1, T2, T3>(Events.Event<T1, T2, T3> gameEvent)
        {
            return EventAgent<T1, T2, T3>.Instance.CheckHaveListener(gameEvent.eid);
        }

        //-----------------------------------T1T2T3T4参数----------------------------------------
        public static void AddListener<T1, T2, T3, T4>(Enum eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<T1, T2, T3, T4>.Instance.AddListener(eid, action);
        }

        public static void AddListener<T1, T2, T3, T4>(Events.Event<T1, T2, T3, T4> gameEvent, Action<T1, T2, T3, T4> action)
        {
            EventAgent<T1, T2, T3, T4>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T1, T2, T3, T4>(Events.Event<T1, T2, T3, T4> gameEvent, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<T1, T2, T3, T4>.Instance.Invoke(gameEvent.eid, param1, param2, param3, param4);
        }

        public static void SendEvent<T1, T2, T3, T4>(Enum eid, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<T1, T2, T3, T4>.Instance.Invoke(eid, param1, param2, param3, param4);
        }

        public static void RemoveListener<T1, T2, T3, T4>(Enum eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<T1, T2, T3, T4>.Instance.RemoveListener(eid, action);
        }

        public static void RemoveListener<T1, T2, T3, T4>(Events.Event<T1, T2, T3, T4> gameEvent, Action<T1, T2, T3, T4> action)
        {
            EventAgent<T1, T2, T3, T4>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T1, T2, T3, T4>(Events.Event<T1, T2, T3, T4> gameEvent)
        {
            return EventAgent<T1, T2, T3, T4>.Instance.CheckHaveListener(gameEvent.eid);
        }
    }
}

