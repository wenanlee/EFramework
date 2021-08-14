using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnityDictionary<K,V>
{
    private Dictionary<K, V> cache = new Dictionary<K, V>();
    public List<Dict> dict = new List<Dict>();
    
    public void Add(K k,V v)
    {
        cache.Add(k, v);
        //keys.Add(k);
        dict.Add(new Dict() { key = k, value = v });
    }
    public V this[K key]
    {
        get
        {
            return cache[key];
        }
    }
    public bool ContainsKey(K k)
    {
        return cache.ContainsKey(k);
    }
    public void Remove(K k)
    {
        cache.Remove(k);
        for (int i = 0; i < dict.Count; i++)
        {
            if(dict[i].key.Equals(k))
                dict.Remove(dict[i]);
        }
        
    }
    [System.Serializable]
    public class Dict
    {
        public K key;
        public V value;
    }
}
