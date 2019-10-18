using System.Collections;
using System.Collections.Generic;
using System;
using EFramework.Core;
namespace EFramework.Core
{
    public class EventManager
    {
        private List<IEventAgent> gameEventAgentList = new List<IEventAgent>(30);//根据项目中的事件数量调整

        public void AddEventAgent(IEventAgent agent)
        {
            gameEventAgentList.Add(agent);
        }

        #region Enum
        //----------------------------------------无参--------------------------------------------
        public static void AddListener(Enum eid, Action action)
        {
            EventAgent<Enum>.Instance.AddListener(eid, action);
        }

        public static void SendEvent(Enum eid)
        {
            EventAgent<Enum>.Instance.Invoke(eid);
        }

        public static void RemoveListener(Enum eid, Action action)
        {
            EventAgent<Enum>.Instance.RemoveListener(eid, action);
        }

        //----------------------------------T参数-----------------------------------------------
        public static void AddListener<T>(Enum eid, Action<T> action)
        {
            EventAgent<Enum, T>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T>(Enum eid, T param)
        {
            EventAgent<Enum, T>.Instance.Invoke(eid, param);
        }

        public static void RemoveListener<T>(Enum eid, Action<T> action)
        {
            EventAgent<Enum, T>.Instance.RemoveListener(eid, action);
        }

        //-----------------------------T1 T2参数-------------------------------------------------
        public static void AddListener<T1, T2>(Enum eid, Action<T1, T2> action)
        {
            EventAgent<Enum, T1, T2>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2>(Enum eid, T1 param1, T2 param2)
        {
            EventAgent<Enum, T1, T2>.Instance.Invoke(eid, param1, param2);
        }

        public static void RemoveListener<T1, T2>(Enum eid, Action<T1, T2> action)
        {
            EventAgent<Enum, T1, T2>.Instance.RemoveListener(eid, action);
        }

        //--------------------------------------------------------------------------------------
        public static void AddListener<T1, T2, T3>(Enum eid, Action<T1, T2, T3> action)
        {
            EventAgent<Enum, T1, T2, T3>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2, T3>(Enum eid, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<Enum, T1, T2, T3>.Instance.Invoke(eid, param1, param2, param3);
        }
        public static void RemoveListener<T1, T2, T3>(Enum eid, Action<T1, T2, T3> action)
        {
            EventAgent<Enum, T1, T2, T3>.Instance.RemoveListener(eid, action);
        }

        //-----------------------------------T1T2T3T4参数----------------------------------------
        public static void AddListener<T1, T2, T3, T4>(Enum eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<Enum, T1, T2, T3, T4>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2, T3, T4>(Enum eid, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<Enum, T1, T2, T3, T4>.Instance.Invoke(eid, param1, param2, param3, param4);
        }

        public static void RemoveListener<T1, T2, T3, T4>(Enum eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<Enum, T1, T2, T3, T4>.Instance.RemoveListener(eid, action);
        }
        #endregion

        #region String
        //----------------------------------------无参--------------------------------------------
        public static void AddListener(string eid, Action action)
        {
            EventAgent<string>.Instance.AddListener(eid, action);
        }

        public static void SendEvent(string eid)
        {
            EventAgent<string>.Instance.Invoke(eid);
        }

        public static void RemoveListener(string eid, Action action)
        {
            EventAgent<string>.Instance.RemoveListener(eid, action);
        }
        //----------------------------------T参数-----------------------------------------------
        public static void AddListener<T>(string eid, Action<T> action)
        {
            EventAgent<string, T>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T>(string eid, T param)
        {
            EventAgent<string, T>.Instance.Invoke(eid, param);
        }

        public static void RemoveListener<T>(string eid, Action<T> action)
        {
            EventAgent<string, T>.Instance.RemoveListener(eid, action);
        }
        //-----------------------------T1 T2参数-------------------------------------------------
        public static void AddListener<T1, T2>(string eid, Action<T1, T2> action)
        {
            EventAgent<string, T1, T2>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2>(string eid, T1 param1, T2 param2)
        {
            EventAgent<string, T1, T2>.Instance.Invoke(eid, param1, param2);
        }

        public static void RemoveListener<T1, T2>(string eid, Action<T1, T2> action)
        {
            EventAgent<string, T1, T2>.Instance.RemoveListener(eid, action);
        }

        //--------------------------------------------------------------------------------------
        public static void AddListener<T1, T2, T3>(string eid, Action<T1, T2, T3> action)
        {
            EventAgent<string, T1, T2, T3>.Instance.AddListener(eid, action);
        }
        public static void SendEvent<T1, T2, T3>(string eid, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<string, T1, T2, T3>.Instance.Invoke(eid, param1, param2, param3);
        }
        public static void RemoveListener<T1, T2, T3>(string eid, Action<T1, T2, T3> action)
        {
            EventAgent<string, T1, T2, T3>.Instance.RemoveListener(eid, action);
        }
        //-----------------------------------T1T2T3T4参数----------------------------------------
        public static void AddListener<T1, T2, T3, T4>(string eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<string, T1, T2, T3, T4>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2, T3, T4>(string eid, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<string, T1, T2, T3, T4>.Instance.Invoke(eid, param1, param2, param3, param4);
        }

        public static void RemoveListener<T1, T2, T3, T4>(string eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<string, T1, T2, T3, T4>.Instance.RemoveListener(eid, action);
        }
        #endregion

        #region Int
        //----------------------------------------无参--------------------------------------------
        public static void AddListener(int eid, Action action)
        {
            EventAgent<int>.Instance.AddListener(eid, action);
        }

        public static void SendEvent(int eid)
        {
            EventAgent<int>.Instance.Invoke(eid);
        }

        public static void RemoveListener(int eid, Action action)
        {
            EventAgent<int>.Instance.RemoveListener(eid, action);
        }
        //----------------------------------T参数-----------------------------------------------
        public static void AddListener<T>(int eid, Action<T> action)
        {
            EventAgent<int, T>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T>(int eid, T param)
        {
            EventAgent<int, T>.Instance.Invoke(eid, param);
        }

        public static void RemoveListener<T>(int eid, Action<T> action)
        {
            EventAgent<int, T>.Instance.RemoveListener(eid, action);
        }
        //-----------------------------T1 T2参数-------------------------------------------------
        public static void AddListener<T1, T2>(int eid, Action<T1, T2> action)
        {
            EventAgent<int, T1, T2>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2>(int eid, T1 param1, T2 param2)
        {
            EventAgent<int, T1, T2>.Instance.Invoke(eid, param1, param2);
        }

        public static void RemoveListener<T1, T2>(int eid, Action<T1, T2> action)
        {
            EventAgent<int, T1, T2>.Instance.RemoveListener(eid, action);
        }

        //--------------------------------------------------------------------------------------
        public static void AddListener<T1, T2, T3>(int eid, Action<T1, T2, T3> action)
        {
            EventAgent<int, T1, T2, T3>.Instance.AddListener(eid, action);
        }
        public static void SendEvent<T1, T2, T3>(int eid, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<int, T1, T2, T3>.Instance.Invoke(eid, param1, param2, param3);
        }
        public static void RemoveListener<T1, T2, T3>(int eid, Action<T1, T2, T3> action)
        {
            EventAgent<int, T1, T2, T3>.Instance.RemoveListener(eid, action);
        }
        //-----------------------------------T1T2T3T4参数----------------------------------------
        public static void AddListener<T1, T2, T3, T4>(int eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<int, T1, T2, T3, T4>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2, T3, T4>(int eid, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<int, T1, T2, T3, T4>.Instance.Invoke(eid, param1, param2, param3, param4);
        }

        public static void RemoveListener<T1, T2, T3, T4>(int eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<int, T1, T2, T3, T4>.Instance.RemoveListener(eid, action);
        }
        #endregion

        #region Object
        //----------------------------------------无参--------------------------------------------
        public static void AddListener(object eid, Action action)
        {
            EventAgent<object>.Instance.AddListener(eid, action);
        }

        public static void SendEvent(object eid)
        {
            EventAgent<object>.Instance.Invoke(eid);
        }

        public static void RemoveListener(object eid, Action action)
        {
            EventAgent<object>.Instance.RemoveListener(eid, action);
        }
        //----------------------------------T参数-----------------------------------------------
        public static void AddListener<T>(object eid, Action<T> action)
        {
            EventAgent<object, T>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T>(object eid, T param)
        {
            EventAgent<object, T>.Instance.Invoke(eid, param);
        }

        public static void RemoveListener<T>(object eid, Action<T> action)
        {
            EventAgent<object, T>.Instance.RemoveListener(eid, action);
        }
        //-----------------------------T1 T2参数-------------------------------------------------
        public static void AddListener<T1, T2>(object eid, Action<T1, T2> action)
        {
            EventAgent<object, T1, T2>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2>(object eid, T1 param1, T2 param2)
        {
            EventAgent<object, T1, T2>.Instance.Invoke(eid, param1, param2);
        }

        public static void RemoveListener<T1, T2>(object eid, Action<T1, T2> action)
        {
            EventAgent<object, T1, T2>.Instance.RemoveListener(eid, action);
        }

        //--------------------------------------------------------------------------------------
        public static void AddListener<T1, T2, T3>(object eid, Action<T1, T2, T3> action)
        {
            EventAgent<object, T1, T2, T3>.Instance.AddListener(eid, action);
        }
        public static void SendEvent<T1, T2, T3>(object eid, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<object, T1, T2, T3>.Instance.Invoke(eid, param1, param2, param3);
        }
        public static void RemoveListener<T1, T2, T3>(object eid, Action<T1, T2, T3> action)
        {
            EventAgent<object, T1, T2, T3>.Instance.RemoveListener(eid, action);
        }
        //-----------------------------------T1T2T3T4参数----------------------------------------
        public static void AddListener<T1, T2, T3, T4>(object eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<object, T1, T2, T3, T4>.Instance.AddListener(eid, action);
        }

        public static void SendEvent<T1, T2, T3, T4>(object eid, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<object, T1, T2, T3, T4>.Instance.Invoke(eid, param1, param2, param3, param4);
        }

        public static void RemoveListener<T1, T2, T3, T4>(object eid, Action<T1, T2, T3, T4> action)
        {
            EventAgent<object, T1, T2, T3, T4>.Instance.RemoveListener(eid, action);
        }
        #endregion

        #region Event
        //----------------------------------------无参--------------------------------------------

        public static void AddListener<T>(Events.Event<T> gameEvent, Action action)
        {
            EventAgent<T>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T>(Events.Event<T> gameEvent)
        {
            EventAgent<T>.Instance.Invoke(gameEvent.eid);
        }

        public static void RemoveListener<T>(Events.Event<T> gameEvent, Action action)
        {
            EventAgent<T>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T>(Events.Event<T> gameEvent)
        {
            return EventAgent<T>.Instance.CheckHaveListener(gameEvent.eid);
        }

        //----------------------------------T参数-----------------------------------------------

        public static void AddListener<T, T1>(Events.Event<T, T1> gameEvent, Action<T1> action)
        {
            EventAgent<T, T1>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T, T1>(Events.Event<T, T1> gameEvent, T1 param)
        {
            EventAgent<T, T1>.Instance.Invoke(gameEvent.eid, param);
        }

        public static void RemoveListener<T, T1>(Events.Event<T, T1> gameEvent, Action<T1> action)
        {
            EventAgent<T, T1>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T, T1>(Events.Event<T> gameEvent)
        {
            return EventAgent<T, T1>.Instance.CheckHaveListener(gameEvent.eid);
        }

        ////-----------------------------T1 T2参数-------------------------------------------------
        public static void AddListener<T, T1, T2>(Events.Event<T, T1, T2> gameEvent, Action<T1, T2> action)
        {
            EventAgent<T, T1, T2>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T, T1, T2>(Events.Event<T, T1, T2> gameEvent, T1 param1, T2 param2)
        {
            EventAgent<T, T1, T2>.Instance.Invoke(gameEvent.eid, param1, param2);
        }

        public static void RemoveListener<T, T1, T2>(Events.Event<T, T1, T2> gameEvent, Action<T1, T2> action)
        {
            EventAgent<T, T1, T2>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T, T1, T2>(Events.Event<T, T1, T2> gameEvent)
        {
            return EventAgent<T, T1, T2>.Instance.CheckHaveListener(gameEvent.eid);
        }

        ////--------------------------------------------------------------------------------------
        public static void AddListener<T, T1, T2, T3>(Events.Event<T, T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
        {
            EventAgent<T, T1, T2, T3>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T, T1, T2, T3>(Events.Event<T, T1, T2, T3> gameEvent, T1 param1, T2 param2, T3 param3)
        {
            EventAgent<T, T1, T2, T3>.Instance.Invoke(gameEvent.eid, param1, param2, param3);
        }

        public static void RemoveListener<T, T1, T2, T3>(Events.Event<T, T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
        {
            EventAgent<T, T1, T2, T3>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T, T1, T2, T3>(Events.Event<T, T1, T2, T3> gameEvent)
        {
            return EventAgent<T, T1, T2, T3>.Instance.CheckHaveListener(gameEvent.eid);
        }

        ////-----------------------------------T1T2T3T4参数----------------------------------------
        public static void AddListener<T, T1, T2, T3, T4>(Events.Event<T, T1, T2, T3, T4> gameEvent, Action<T1, T2, T3, T4> action)
        {
            EventAgent<T, T1, T2, T3, T4>.Instance.AddListener(gameEvent.eid, action);
        }

        public static void SendEvent<T, T1, T2, T3, T4>(Events.Event<T, T1, T2, T3, T4> gameEvent, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            EventAgent<T, T1, T2, T3, T4>.Instance.Invoke(gameEvent.eid, param1, param2, param3, param4);
        }

        public static void RemoveListener<T, T1, T2, T3, T4>(Events.Event<T, T1, T2, T3, T4> gameEvent, Action<T1, T2, T3, T4> action)
        {
            EventAgent<T, T1, T2, T3, T4>.Instance.RemoveListener(gameEvent.eid, action);
        }

        public static bool CheckHaveListener<T, T1, T2, T3, T4>(Events.Event<T, T1, T2, T3, T4> gameEvent)
        {
            return EventAgent<T, T1, T2, T3, T4>.Instance.CheckHaveListener(gameEvent.eid);
        }
        #endregion
    }
}

