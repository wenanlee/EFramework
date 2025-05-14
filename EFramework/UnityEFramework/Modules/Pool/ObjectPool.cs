using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjPool<T> where T : Component
{
    [SerializeField] public T prefab;
    [SerializeField] public Transform parent;
    [SerializeField] private int defaultSize = 100; //对象池默认容量
    [SerializeField] private int maxSize = 500; //对象池最大容量
    [SerializeField] private ObjectPool<T> pool;
    public int ActiveCount => pool.CountActive;
    public int InactiveCount => pool.CountInactive;
    public int TotalCount => pool.CountAll;
    public ObjectPool<T> Pool
    {
        get
        {
            if (pool == null)
                pool = new ObjectPool<T>(OnCreatePoolItem, OnGetPoolItem, OnRealeasePoolItem, OnDestroyPoolItem, true, defaultSize, maxSize);

            return pool;
        }
    }
    public ObjPool(T prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }
    protected void Initialize(bool collectionCheck = true) =>
        pool = new ObjectPool<T>(OnCreatePoolItem, OnGetPoolItem, OnRealeasePoolItem, OnDestroyPoolItem,
            collectionCheck, defaultSize, maxSize);


    protected virtual T OnCreatePoolItem() => UnityEngine.GameObject.Instantiate(prefab, parent);
    protected virtual void OnRealeasePoolItem(T obj) => obj.gameObject.SetActive(false); //将物体返回对象池
    protected virtual void OnGetPoolItem(T obj) => obj.gameObject.SetActive(true);

    protected virtual void OnDestroyPoolItem(T obj) => UnityEngine.Object.Destroy(obj.gameObject);

    public T Get()
    {
        T t = Pool.Get();
        return t;
    }

    public void Release(T obj) {
        Pool.Release(obj);
    }
    public void Clear() => Pool.Clear();
}