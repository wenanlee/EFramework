using System;
using System.Collections.Generic;
using System.Text;

namespace EFramework.Core
{
    /// <summary>
    /// 事件基类
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <typeparam name="K">Key</typeparam>
    /// <typeparam name="P">参数</typeparam>

    public class EventBase<T, K , P> : IDisposable where T:new()
    {
        public void Dispose()
        {
            
        }
        private static T instance;
        public static T Instance
        {
            get
            {

                if (instance==null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        public delegate void OnActionHandler(P p);
        public Dictionary<K, List<OnActionHandler>> dic = new Dictionary<K, List<OnActionHandler>>();
        public void AddListener(K key,OnActionHandler handler)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(handler);
            }
            else
            {
                List<OnActionHandler> lstList = new List<OnActionHandler>();
                lstList.Add(handler);
                dic[key] = lstList;
            }
        }
        public void RemoveListener(K key,OnActionHandler handler)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Remove(handler);
                if (dic[key].Count==0)
                {
                    dic.Remove(key);
                }
            }
        }
        public void Dispatch(K key,P p)
        {
            if (dic.ContainsKey(key))
            {
                List<OnActionHandler> lstList = dic[key];
                if (lstList!=null && lstList.Count>0)
                {
                    foreach (var item in lstList)
                    {
                        if (item!=null)
                        {
                            item(p);
                        }
                    }
                }
            }
        }
        public void Dispatch(K key)
        {
            //Dispatch(key,);
        }
    }
}
