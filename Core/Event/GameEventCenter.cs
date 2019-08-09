using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameEventCenter
{
    private static GameEventCenter mInstance;
    public static GameEventCenter Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = new GameEventCenter();
            }
            return mInstance;
        }
    }

    private List<IGameEventAgent> gameEventAgentList = new List<IGameEventAgent>(30);//根据项目中的事件数量调整
    private GameEventCenter()
    {

    }
    public void AddGameEventAgent(IGameEventAgent agent)
    {
        gameEventAgentList.Add(agent);
    }

    //----------------------------------------无参--------------------------------------------
    public static void AddListener(Enum eid, Action action)
    {
        GameEventAgent.Instance.AddListener(eid, action);
    }

    public static void AddListener(GameEvents.Event gameEvent, Action action)
    {
        GameEventAgent.Instance.AddListener(gameEvent.eid, action);
    }

    public static void SendEvent(GameEvents.Event gameEvent)
    {
        GameEventAgent.Instance.Invoke(gameEvent.eid);
    }

    public static void SendEvent(Enum eid)
    {
        GameEventAgent.Instance.Invoke(eid);
    }

    public static void RemoveListener(Enum eid, Action action)
    {
        GameEventAgent.Instance.RemoveListener(eid, action);
    }

    public static void RemoveListener(GameEvents.Event gameEvent , Action action)
    {
        GameEventAgent.Instance.RemoveListener(gameEvent.eid, action);
    }

    public static bool CheckHaveListener(GameEvents.Event gameEvent)
    {
        return GameEventAgent.Instance.CheckHaveListener(gameEvent.eid);
    }

    //----------------------------------T参数-----------------------------------------------
    public static void AddListener<T>(Enum eid, Action<T> action)
    {
        GameEventAgent<T>.Instance.AddListener(eid, action);
    }

    public static void AddListener<T>(GameEvents.Event<T> gameEvent, Action<T> action)
    {
        GameEventAgent<T>.Instance.AddListener(gameEvent.eid, action);
    }

    public static void SendEvent<T>(GameEvents.Event<T> gameEvent, T param)
    {
        GameEventAgent<T>.Instance.Invoke(gameEvent.eid, param);
    }

    public static void SendEvent<T>(Enum eid, T param)
    {
        GameEventAgent<T>.Instance.Invoke(eid, param);
    }

    public static void RemoveListener<T>(Enum eid, Action<T> action)
    {
        GameEventAgent<T>.Instance.RemoveListener(eid, action);
    }

    public static void RemoveListener<T>(GameEvents.Event<T> gameEvent, Action<T> action)
    {
        GameEventAgent<T>.Instance.RemoveListener(gameEvent.eid, action);
    }

    public static bool CheckHaveListener<T>(GameEvents.Event<T> gameEvent)
    {
        return GameEventAgent<T>.Instance.CheckHaveListener(gameEvent.eid);
    }

    //-----------------------------T1 T2参数-------------------------------------------------
    public static void AddListener<T1, T2>(Enum eid, Action<T1, T2> action)
    {
        GameEventAgent<T1, T2>.Instance.AddListener(eid, action);
    }

    public static void AddListener<T1, T2>(GameEvents.Event<T1, T2> gameEvent, Action<T1, T2> action)
    {
        GameEventAgent<T1, T2>.Instance.AddListener(gameEvent.eid, action);
    }

    public static void SendEvent<T1, T2>(GameEvents.Event<T1, T2> gameEvent, T1 param1, T2 param2)
    {
        GameEventAgent<T1, T2>.Instance.Invoke(gameEvent.eid, param1, param2);
    }

    public static void SendEvent<T1, T2>(Enum eid, T1 param1, T2 param2)
    {
        GameEventAgent<T1, T2>.Instance.Invoke(eid, param1, param2);
    }

    public static void RemoveListener<T1, T2>(Enum eid, Action<T1, T2> action)
    {
        GameEventAgent<T1, T2>.Instance.RemoveListener(eid, action);
    }

    public static void RemoveListener<T1, T2>(GameEvents.Event<T1, T2> gameEvent, Action<T1, T2> action)
    {
        GameEventAgent<T1, T2>.Instance.RemoveListener(gameEvent.eid, action);
    }

    public static bool CheckHaveListener<T1, T2>(GameEvents.Event<T1, T2> gameEvent)
    {
        return GameEventAgent<T1, T2>.Instance.CheckHaveListener(gameEvent.eid);
    }

    //--------------------------------------------------------------------------------------
    public static void AddListener<T1, T2, T3>(Enum eid, Action<T1, T2, T3> action)
    {
        GameEventAgent<T1, T2, T3>.Instance.AddListener(eid, action);
    }

    public static void AddListener<T1, T2, T3>(GameEvents.Event<T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
    {
        GameEventAgent<T1, T2, T3>.Instance.AddListener(gameEvent.eid, action);
    }

    public static void SendEvent<T1, T2, T3>(GameEvents.Event<T1, T2, T3> gameEvent, T1 param1, T2 param2, T3 param3)
    {
        GameEventAgent<T1, T2, T3>.Instance.Invoke(gameEvent.eid, param1, param2, param3);
    }
    public static void SendEvent<T1, T2, T3>(Enum eid, T1 param1, T2 param2, T3 param3)
    {
        GameEventAgent<T1, T2, T3>.Instance.Invoke(eid, param1, param2, param3);
    }
    public static void RemoveListener<T1, T2, T3>(Enum eid, Action<T1, T2, T3> action)
    {
        GameEventAgent<T1, T2, T3>.Instance.RemoveListener(eid, action);
    }

    public static void RemoveListener<T1, T2, T3>(GameEvents.Event<T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
    {
        GameEventAgent<T1, T2, T3>.Instance.RemoveListener(gameEvent.eid, action);
    }

    public static bool CheckHaveListener<T1, T2, T3>(GameEvents.Event<T1, T2, T3> gameEvent)
    {
        return GameEventAgent<T1, T2, T3>.Instance.CheckHaveListener(gameEvent.eid);
    }

    //-----------------------------------T1T2T3T4参数----------------------------------------
    public static void AddListener<T1, T2, T3, T4>(Enum eid, Action<T1, T2, T3, T4> action)
    {
        GameEventAgent<T1, T2, T3, T4>.Instance.AddListener(eid, action);
    }

    public static void AddListener<T1, T2, T3, T4>(GameEvents.Event<T1, T2, T3, T4> gameEvent, Action<T1, T2, T3, T4> action)
    {
        GameEventAgent<T1, T2, T3, T4>.Instance.AddListener(gameEvent.eid, action);
    }

    public static void SendEvent<T1, T2, T3, T4>(GameEvents.Event<T1, T2, T3, T4> gameEvent, T1 param1, T2 param2, T3 param3, T4 param4)
    {
        GameEventAgent<T1, T2, T3, T4>.Instance.Invoke(gameEvent.eid, param1, param2, param3, param4);
    }

    public static void SendEvent<T1, T2, T3, T4>(Enum eid, T1 param1, T2 param2, T3 param3, T4 param4)
    {
        GameEventAgent<T1, T2, T3, T4>.Instance.Invoke(eid, param1, param2, param3, param4);
    }

    public static void RemoveListener<T1, T2, T3, T4>(Enum eid, Action<T1, T2, T3, T4> action)
    {
        GameEventAgent<T1, T2, T3, T4>.Instance.RemoveListener(eid, action);
    }

    public static void RemoveListener<T1, T2, T3, T4>(GameEvents.Event<T1, T2, T3, T4> gameEvent, Action<T1, T2, T3, T4> action)
    {
        GameEventAgent<T1, T2, T3, T4>.Instance.RemoveListener(gameEvent.eid, action);
    }

    public static bool CheckHaveListener<T1, T2, T3, T4>(GameEvents.Event<T1, T2, T3, T4> gameEvent)
    {
        return GameEventAgent<T1, T2, T3, T4>.Instance.CheckHaveListener(gameEvent.eid);
    }
}
