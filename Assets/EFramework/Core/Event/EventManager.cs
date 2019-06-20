using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace EFramework.Event
{
    public class EventManager : MonoBehaviour
    {
        Dictionary<Enum, List<EventObserver>> observerList = new Dictionary<Enum, List<EventObserver>>();

        Queue eventQueue = new Queue(); //消息队列

        private static EventManager _instance = null;

        public static EventManager instance()
        {
            return _instance;
        }

        void Awake()
        {
            Debug.Log("===========启动消息系统===========");
            _instance = this;
        }

        void Update()
        {
            while (eventQueue.Count > 0)
            {
                EventBase eve = (EventBase)eventQueue.Dequeue();
                if (!observerList.ContainsKey(eve.eid))
                {
                    continue;
                }
                List<EventObserver> observers = observerList[eve.eid];

                for (int i = 0; i < observers.Count; i++)
                {
                    if (observers[i] == null) continue;
                    observers[i].HandleEvent(eve);
                }

            }
        }

        //发送事件
        public void SendEvent(EventBase eve)
        {
            eventQueue.Enqueue(eve);
        }


        //添加监听者
        void RegisterObj(EventObserver newobj, Enum eid)
        {
            if (!observerList.ContainsKey(eid))
            {
                List<EventObserver> list = new List<EventObserver>();
                list.Add(newobj);
                observerList.Add(eid, list);
            }
            else
            {
                List<EventObserver> list = observerList[eid];
                foreach (EventObserver obj in list)
                {
                    if (obj == newobj)
                    {
                        return;
                    }
                }
                list.Add(newobj);
            }
        }

        //移除监听者
        void RemoveObj(EventObserver removeobj)
        {
            foreach (KeyValuePair<Enum, List<EventObserver>> kv in observerList)
            {
                List<EventObserver> list = kv.Value;
                foreach (EventObserver obj in list)
                {
                    if (obj == removeobj)
                    {
                        list.Remove(obj);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 移除一个监听者
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="removeobj">Removeobj.</param>
        public static void Remove(EventObserver removeobj)
        {
            if (EventManager.instance() == null) return;
            EventManager.instance().RemoveObj(removeobj);
        }

        /// <summary>
        /// 监听者在这里注册
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="newobj">Newobj.</param>
        /// <param name="eids">需要监听的事件列表.</param>
        public static void Register(EventObserver newobj, params Enum[] eids)
        {
            if (EventManager.instance() == null) return;
            foreach (Enum eid in eids)
            {
                EventManager.instance().RegisterObj(newobj, eid);
            }
        }
    }
}

public enum MyEnum
{
    test0, test1
}