using System.Collections;
using System;
namespace EFramework.Core
{
    /// <summary>
    /// 定义事件
    /// </summary>
    public class Events
    {
        public class Event<T>
        {
            public readonly T eid;
            public Event(T eid)
            {
                this.eid = eid;
            }
        }

        public class Event<T,T1>
        {
            public readonly T eid;
            public Event(T eid)
            {
                this.eid = eid;
            }
        }
        public class Event<T,T1, T2>
        {
            public readonly T eid;
            public Event(T eid)
            {
                this.eid = eid;
            }
        }
        public class Event<T,T1, T2, T3>
        {
            public readonly T eid;
            public Event(T eid)
            {
                this.eid = eid;
            }
        }
        public class Event<T,T1, T2, T3, T4>
        {
            public readonly T eid;
            public Event(T eid)
            {
                this.eid = eid;
            }
        }

    }
}