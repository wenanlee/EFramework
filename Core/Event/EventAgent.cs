using System.Collections.Generic;
using System;
using UnityEngine;

namespace EFramework.Core
{
    /// <summary>
    /// 事件系统底层类
    /// </summary>

    public class EventAgent<T>
    {

        private static EventAgent<T> inatance;
        public static EventAgent<T> Instance
        {
            get
            {
                if (inatance == null)
                {
                    inatance = new EventAgent<T>();
                }
                return inatance;
            }
        }

        Dictionary<T, Signal> eventList = new Dictionary<T, Signal>();

        private EventAgent()
        {
        }

        public void AddListener(T eid, Action action)
        {
            Signal eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate) == false)
            {
                eventDelegate = new Signal();
                eventList.Add(eid, eventDelegate);
            }
            eventDelegate.AddListener(action);
        }

        public void Invoke(T eid)
        {
            Signal eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                eventDelegate.InvokeSafe();
            }
        }

        public void RemoveListener(T eid, Action action)
        {
            Signal eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                eventDelegate.RemoveListener(action);
            }
        }
        private void RemoveListener(T eid)
        {
            if (CheckHaveListener(eid))
            {
                eventList.Remove(eid);
            }
        }

        public bool CheckHaveListener(T eid)
        {
            Signal eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                Delegate[] delegateList = eventDelegate.GetInvokeList();
                if (delegateList != null && delegateList.Length > 0)
                    return true;
            }
            return false;
        }

        private void RemoveListener(Action listener)
        {
            Dictionary<T, Signal>.Enumerator tor = eventList.GetEnumerator();//返回实例的枚举数 就是返回集的中所有元素一个一个列出来
            while (tor.MoveNext())
            {
                tor.Current.Value.RemoveListener(listener);
            }
        }
    }
    public class EventAgent<T, T1>
    {

        private static EventAgent<T, T1> mInatance;
        public static EventAgent<T, T1> Instance
        {
            get
            {
                if (mInatance == null)
                {
                    mInatance = new EventAgent<T, T1>();
                }
                return mInatance;
            }
        }


        Dictionary<T, Signal<T1>> eventList = new Dictionary<T, Signal<T1>>();

        private EventAgent()
        {
        }

        public void AddListener(T eid, Action<T1> action)
        {
            Signal<T1> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate) == false)
            {
                gameEventDelegate = new Signal<T1>();
                eventList.Add(eid, gameEventDelegate);
            }
            gameEventDelegate.AddListener(action);
        }

        public void Invoke(T eid, T1 param1)
        {
            Signal<T1> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.Invoke(param1);
            }
        }

        public void RemoveListener(T eid, Action<T1> action)
        {
            Signal<T1> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.RemoveListener(action);
            }
        }
        private void RemoveListener(T eid)
        {
            if (CheckHaveListener(eid))
            {
                eventList.Remove(eid);
            }
        }

        public bool CheckHaveListener(T eid)
        {
            Signal<T1> eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                Delegate[] delegateList = eventDelegate.GetInvokeList();
                if (delegateList != null && delegateList.Length > 0)
                    return true;
            }
            return false;
        }

        private void RemoveListener(Action<T1> listener)
        {
            Dictionary<T, Signal<T1>>.Enumerator tor = eventList.GetEnumerator();//返回实例的枚举数 就是返回集的中所有元素一个一个列出来
            while (tor.MoveNext())
            {
                tor.Current.Value.RemoveListener(listener);
            }
        }
    }
    public class EventAgent<T, T1, T2>
    {

        private static EventAgent<T, T1, T2> mInatance;
        public static EventAgent<T, T1, T2> Instance
        {
            get
            {
                if (mInatance == null)
                {
                    mInatance = new EventAgent<T, T1, T2>();
                }
                return mInatance;
            }
        }


        Dictionary<T, Signal<T1, T2>> eventList = new Dictionary<T, Signal<T1, T2>>();

        private EventAgent()
        {
        }

        public void AddListener(T eid, Action<T1, T2> action)
        {
            Signal<T1, T2> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate) == false)
            {
                gameEventDelegate = new Signal<T1, T2>();
                eventList.Add(eid, gameEventDelegate);
            }
            gameEventDelegate.AddListener(action);
        }

        public void Invoke(T eid, T1 param1, T2 param2)
        {
            Signal<T1, T2> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.Invoke(param1, param2);
            }
        }

        public void RemoveListener(T eid, Action<T1, T2> action)
        {
            Signal<T1, T2> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.RemoveListener(action);
            }
        }
        private void RemoveListener(T eid)
        {
            if (CheckHaveListener(eid))
            {
                eventList.Remove(eid);
            }
        }

        public bool CheckHaveListener(T eid)
        {
            Signal<T1, T2> eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                Delegate[] delegateList = eventDelegate.GetInvokeList();
                if (delegateList != null && delegateList.Length > 0)
                    return true;
            }
            return false;
        }

        private void RemoveListener(Action<T1, T2> listener)
        {
            Dictionary<T, Signal<T1, T2>>.Enumerator tor = eventList.GetEnumerator();//返回实例的枚举数 就是返回集的中所有元素一个一个列出来
            while (tor.MoveNext())
            {
                tor.Current.Value.RemoveListener(listener);
            }
        }
    }

    public class EventAgent<T, T1, T2, T3>
    {

        private static EventAgent<T, T1, T2, T3> mInatance;
        public static EventAgent<T, T1, T2, T3> Instance
        {
            get
            {
                if (mInatance == null)
                {
                    mInatance = new EventAgent<T, T1, T2, T3>();
                }
                return mInatance;
            }
        }


        Dictionary<T, Signal<T1, T2, T3>> eventList = new Dictionary<T, Signal<T1, T2, T3>>();

        private EventAgent()
        {
        }

        public void AddListener(T eid, Action<T1, T2, T3> action)
        {
            Signal<T1, T2, T3> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate) == false)
            {
                gameEventDelegate = new Signal<T1, T2, T3>();
                eventList.Add(eid, gameEventDelegate);
            }
            gameEventDelegate.AddListener(action);
        }

        public void Invoke(T eid, T1 param1, T2 param2, T3 param3)
        {
            Signal<T1, T2, T3> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.Invoke(param1, param2, param3);
            }
        }

        public void RemoveListener(T eid, Action<T1, T2, T3> action)
        {
            Signal<T1, T2, T3> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.RemoveListener(action);
            }
        }
        private void RemoveListener(T eid)
        {
            if (CheckHaveListener(eid))
            {
                eventList.Remove(eid);
            }
        }

        public bool CheckHaveListener(T eid)
        {
            Signal<T1, T2, T3> eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                Delegate[] delegateList = eventDelegate.GetInvokeList();
                if (delegateList != null && delegateList.Length > 0)
                    return true;
            }
            return false;
        }

        private void RemoveListener(Action<T1, T2, T3> listener)
        {
            Dictionary<T, Signal<T1, T2, T3>>.Enumerator tor = eventList.GetEnumerator();//返回实例的枚举数 就是返回集的中所有元素一个一个列出来
            while (tor.MoveNext())
            {
                tor.Current.Value.RemoveListener(listener);
            }
        }
    }

    public class EventAgent<T, T1, T2, T3, T4>
    {

        private static EventAgent<T, T1, T2, T3, T4> mInatance;
        public static EventAgent<T, T1, T2, T3, T4> Instance
        {
            get
            {
                if (mInatance == null)
                {
                    mInatance = new EventAgent<T, T1, T2, T3, T4>();
                }
                return mInatance;
            }
        }


        Dictionary<T, Signal<T1, T2, T3, T4>> eventList = new Dictionary<T, Signal<T1, T2, T3, T4>>();

        private EventAgent()
        {
        }

        public void AddListener(T eid, Action<T1, T2, T3, T4> action)
        {
            Signal<T1, T2, T3, T4> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate) == false)
            {
                gameEventDelegate = new Signal<T1, T2, T3, T4>();
                eventList.Add(eid, gameEventDelegate);
            }
            gameEventDelegate.AddListener(action);
        }

        public void Invoke(T eid, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            Signal<T1, T2, T3, T4> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.Invoke(param1, param2, param3, param4);
            }
        }

        public void RemoveListener(T eid, Action<T1, T2, T3, T4> action)
        {
            Signal<T1, T2, T3, T4> gameEventDelegate;
            if (eventList.TryGetValue(eid, out gameEventDelegate))
            {
                gameEventDelegate.RemoveListener(action);
            }
        }
        private void RemoveListener(T eid)
        {
            if (CheckHaveListener(eid))
            {
                eventList.Remove(eid);
            }
        }

        public bool CheckHaveListener(T eid)
        {
            Signal<T1, T2, T3, T4> eventDelegate;
            if (eventList.TryGetValue(eid, out eventDelegate))
            {
                Delegate[] delegateList = eventDelegate.GetInvokeList();
                if (delegateList != null && delegateList.Length > 0)
                    return true;
            }
            return false;
        }

        private void RemoveListener(Action<T1, T2, T3, T4> listener)
        {
            Dictionary<T, Signal<T1, T2, T3, T4>>.Enumerator tor = eventList.GetEnumerator();//返回实例的枚举数 就是返回集的中所有元素一个一个列出来
            while (tor.MoveNext())
            {
                tor.Current.Value.RemoveListener(listener);
            }
        }
    }
}
