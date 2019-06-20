/*
 * Copyright (C) 2018 Slicol Tang (slicol@qq.com). All rights reserved.
 * 
 * 一个高性能的事件(信号)系统
 * A high performance event (signal) system
 * 
 * 它的思路与系统自带的Action类似，
 * 但是在性能与逻辑上比原生的Action更加可控，
 * 并且能够以日志的方式进行追踪和调试。
 * 这在实际应用中非常有用。
 * 并且非常高性能和低GC
 * Its idea is similar to the Action, 
 * but it is more controllable than the original Action in terms of performance and logic, 
 * and can be tracked and debugged in log mode.
 * This is very useful in practical applications.
 *
 *
 * Licensed under the MIT License (the "License"); 
 * you may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, 
 * software distributed under the License is distributed on an "AS IS" BASIS, 
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. 
 * See the License for the specific language governing permissions and limitations under the License.
*/


using System;
using UnityEngine;

namespace EFramework.Event
{
    public class SignalBase
    {
        protected int _methodCount;
        protected Delegate[] _methods;

        public SignalBase(int capacity)
        {
            _methods = new Delegate[capacity];
            _methodCount = 0;
        }

        public SignalBase()
        {
            _methods = new Delegate[10];
            _methodCount = 0;
        }

        protected void AddListener(Delegate del, bool bInsertAtFirst)
        {
            if (bInsertAtFirst)
            {
                if (_methods[0] == del)
                {
                    return;
                }

                Delegate[] listTemp;
                if (_methodCount + 1 > _methods.Length)
                {
                    listTemp = new Delegate[_methods.Length * 2];
                }
                else
                {
                    listTemp = new Delegate[_methods.Length];
                }

                bool hasExist = false;
                int j = 0;
                listTemp[j++] = del;
                for (int i = 0; i < _methodCount; i++)
                {
                    if (_methods[i] == del)
                    {
                        hasExist = true;
                        continue;
                    }
                    listTemp[j++] = _methods[i];
                }

                if (!hasExist)
                {
                    _methodCount++;
                }

                _methods = listTemp;
            }
            else
            {
                for (int i = 0; i < _methodCount; i++)
                {
                    if (_methods[i] == del)
                    {
                        return;
                    }
                }

                if (_methodCount + 1 > _methods.Length)
                {
                    Delegate[] listTemp;
                    listTemp = new Delegate[_methods.Length * 2];
                    Array.Copy(_methods, 0, listTemp, 0, _methodCount);
                    _methods = listTemp;
                }
                _methods[_methodCount] = del;
                _methodCount++;
            }

        }

        protected void RemoveListener(Delegate del)
        {
            int j = 0;
            int c = _methodCount;
            for (int i = 0; i < c; i++)
            {
                if (_methods[i] == del)
                {
                    _methodCount--;
                    _methods[i] = null;
                    for (int k = i; k < _methodCount; k++)
                    {
                        _methods[k] = _methods[k + 1];
                    }
                    break;
                }
            }
        }

        public void RemoveAllListeners()
        {
            for (int i = 0; i < _methodCount; i++)
            {
                _methods[i] = null;
            }
            _methodCount = 0;
        }



        public int GetListenerCount()
        {
            return _methodCount;
        }
    }


    /// <summary>
    /// 尽管Signal的实现已经具备很高性能
    /// 但是肯定比不上直接的函数调用，所以，如果需要最高性能，请用Delegate
    /// 但是，Signal在内存上比Delegate有较大优势
    /// 请综合考虑
    /// </summary>
    public class Signal : SignalBase
    {

        public static Signal operator +(Signal p1, Action p2)
        {
            p1.AddListener(p2, false);
            return p1;
        }

        public static Signal operator -(Signal p1, Action p2)
        {
            p1.RemoveListener(p2);
            return p1;
        }

        public void AddListener(Action a, bool bInsertAtFirst = false)
        {
            base.AddListener(a, bInsertAtFirst);
        }

        public void RemoveListener(Action a)
        {
            base.RemoveListener(a);
        }

        public void Invoke()
        {

            int c = _methodCount;
            for (int i = 0; i < c; i++)
            {
                var item = _methods[i];
                if (item == null)
                    continue;
                var action = item as Action;
                action();
            }

        }

        public void InvokeSafe()
        {
            try
            {
                int c = _methodCount;
                for (int i = 0; i < c; i++)
                {
                    var item = _methods[i];
                    if (item == null)
                        continue;
                    var action = item as Action;
                    action();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public Signal(int capacity) : base(capacity)
        {
        }

        public Signal()
        {
        }
    }


    public class Signal<T> : SignalBase
    {
        public static Signal<T> operator +(Signal<T> p1, Action<T> p2)
        {
            p1.AddListener(p2, false);
            return p1;
        }

        public static Signal<T> operator -(Signal<T> p1, Action<T> p2)
        {
            p1.RemoveListener(p2);
            return p1;
        }

        public void AddListener(Action<T> a, bool bInsertAtFirst = false)
        {
            base.AddListener(a, bInsertAtFirst);
        }

        public void RemoveListener(Action<T> a)
        {
            base.RemoveListener(a);
        }

        public void Invoke(T t1)
        {

            int c = _methodCount;
            for (int i = 0; i < c; i++)
            {
                var item = _methods[i];
                if (item == null)
                    continue;
                var action = item as Action<T>;
                action(t1);
            }

        }

        public void InvokeSafe(T t)
        {
            try
            {

                int c = _methodCount;
                for (int i = 0; i < c; i++)
                {
                    var item = _methods[i];
                    if (item == null)
                        continue;
                    var action = item as Action<T>;
                    action(t);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public Signal(int capacity) : base(capacity)
        {
        }

        public Signal()
        {
        }
    }

    public class Signal<T1, T2> : SignalBase
    {
        public static Signal<T1, T2> operator +(Signal<T1, T2> p1, Action<T1, T2> p2)
        {
            p1.AddListener(p2, false);
            return p1;
        }

        public static Signal<T1, T2> operator -(Signal<T1, T2> p1, Action<T1, T2> p2)
        {
            p1.RemoveListener(p2);
            return p1;
        }

        public void AddListener(Action<T1, T2> a, bool bInsertAtFirst = false)
        {
            base.AddListener(a, bInsertAtFirst);
        }

        public void RemoveListener(Action<T1, T2> a)
        {
            base.RemoveListener(a);
        }

        public void Invoke(T1 t1, T2 t2)
        {

            int c = _methodCount;
            for (int i = 0; i < c; i++)
            {
                var item = _methods[i];
                if (item == null)
                    continue;
                var action = item as Action<T1, T2>;
                action(t1, t2);
            }

        }

        public void InvokeSafe(T1 t1, T2 t2)
        {
            try
            {
                int c = _methodCount;
                for (int i = 0; i < c; i++)
                {
                    var item = _methods[i];
                    if (item == null)
                        continue;
                    var action = item as Action<T1, T2>;
                    action(t1, t2);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public Signal(int capacity) : base(capacity)
        {
        }

        public Signal()
        {
        }
    }

    public class Signal<T1, T2, T3> : SignalBase
    {
        public static Signal<T1, T2, T3> operator +(Signal<T1, T2, T3> p1, Action<T1, T2, T3> p2)
        {
            p1.AddListener(p2, false);
            return p1;
        }

        public static Signal<T1, T2, T3> operator -(Signal<T1, T2, T3> p1, Action<T1, T2, T3> p2)
        {
            p1.RemoveListener(p2);
            return p1;
        }

        public void AddListener(Action<T1, T2, T3> a, bool bInsertAtFirst = false)
        {
            base.AddListener(a, bInsertAtFirst);
        }

        public void RemoveListener(Action<T1, T2, T3> a)
        {
            base.RemoveListener(a);
        }

        public void Invoke(T1 t1, T2 t2, T3 t3)
        {

            int c = _methodCount;
            for (int i = 0; i < c; i++)
            {
                var item = _methods[i];
                if (item == null)
                    continue;
                var action = item as Action<T1, T2, T3>;
                action(t1, t2, t3);
            }

        }

        public void InvokeSafe(T1 t1, T2 t2, T3 t3)
        {
            try
            {
                int c = _methodCount;
                for (int i = 0; i < c; i++)
                {
                    var item = _methods[i];
                    if (item == null)
                        continue;
                    var action = item as Action<T1, T2, T3>;
                    action(t1, t2, t3);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        public Signal(int capacity) : base(capacity)
        {
        }

        public Signal()
        {
        }
    }

    public class Signal<T1, T2, T3, T4> : SignalBase
    {
        public static Signal<T1, T2, T3, T4> operator +(Signal<T1, T2, T3, T4> p1, Action<T1, T2, T3, T4> p2)
        {
            p1.AddListener(p2, false);
            return p1;
        }

        public static Signal<T1, T2, T3, T4> operator -(Signal<T1, T2, T3, T4> p1, Action<T1, T2, T3, T4> p2)
        {
            p1.RemoveListener(p2);
            return p1;
        }

        public void AddListener(Action<T1, T2, T3, T4> a, bool bInsertAtFirst = false)
        {
            base.AddListener(a, bInsertAtFirst);
        }

        public void RemoveListener(Action<T1, T2, T3, T4> a)
        {
            base.RemoveListener(a);
        }

        public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
        {

            int c = _methodCount;
            for (int i = 0; i < c; i++)
            {
                var item = _methods[i];
                if (item == null)
                    continue;
                var action = item as Action<T1, T2, T3, T4>;
                action(t1, t2, t3, t4);
            }

        }
        public void InvokeSafe(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            try
            {
                int c = _methodCount;
                for (int i = 0; i < c; i++)
                {
                    var item = _methods[i];
                    if (item == null)
                        continue;
                    var action = item as Action<T1, T2, T3, T4>;
                    action(t1, t2, t3, t4);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public Signal(int capacity) : base(capacity)
        {
        }

        public Signal()
        {
        }
        
    }
}