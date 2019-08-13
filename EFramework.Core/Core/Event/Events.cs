using System.Collections;
using System;

/// <summary>
/// 定义事件
/// </summary>
public class Events
{
    public class Event
    {
        public readonly Enum eid;
        public Event(Enum eid)
        {
            this.eid = eid;
        }
        public static Event CreateEvent(Enum eid)
        {
            return new Event(eid);
        }
        public void Send()
        {

        }
    }

    public class Event<T>
    {
        public readonly Enum eid;
        public Event(Enum eid)
        {
            this.eid = eid;
        }
    }
    public class Event<T1, T2>
    {
        public readonly Enum eid;
        public Event(Enum eid)
        {
            this.eid = eid;
        }
    }
    public class Event<T1, T2, T3>
    {
        public readonly Enum eid;
        public Event(Enum eid)
        {
            this.eid = eid;
        }
    }
    public class Event<T1, T2, T3, T4>
    {
        public readonly Enum eid;
        public Event(Enum eid)
        {
            this.eid = eid;
        }
    }
   
}
