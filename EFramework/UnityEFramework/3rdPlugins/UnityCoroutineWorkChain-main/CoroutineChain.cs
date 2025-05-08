using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#region Accessor

/// <summary>
/// 日志类型枚举
/// </summary>
public enum ELogType
{
    NORMAL,
    WARRNING,
    ERROR
}

/// <summary>
/// MonoBehaviour扩展方法
/// </summary>
public static class MonobehaviourExtend
{
    public static ChainBase StartChain(this MonoBehaviour mono)
    {
        return ChainBase.BasePool.Spawn(mono);
    }
}

/// <summary>
/// 协程链静态入口类
/// </summary>
public static class CoroutineChain
{
    class Dispather : MonoBehaviour { }

    static Dispather m_instance;

    static Dispather Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("CoroutineChain").AddComponent<Dispather>();
                Object.DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    public static void StopAll()
    {
        m_instance.StopAllCoroutines();
    }

    public static ChainBase Start => ChainBase.BasePool.Spawn(Instance);
}

#endregion

#region Util

/// <summary>
/// 泛型内存池（带参数）
/// </summary>
public class MemoryPool<T, TParam> where T : new()
{
    Stack<T> pool = new Stack<T>();
    Action<T, TParam> onSpawn;
    Action<T> onDespawn;

    public MemoryPool(Action<T, TParam> onSpawn = null, Action<T> onDespawn = null)
    {
        this.onSpawn = onSpawn;
        this.onDespawn = onDespawn;
    }

    public T Spawn(TParam param)
    {
        T item = pool.Count > 0 ? pool.Pop() : new T();
        onSpawn?.Invoke(item, param);
        return item;
    }

    public void Despawn(T item)
    {
        onDespawn?.Invoke(item);
        pool.Push(item);
    }
}

/// <summary>
/// 泛型内存池（无参数）
/// </summary>
public class MemoryPool<T> where T : new()
{
    Stack<T> pool = new Stack<T>();
    Action<T> onSpawn;
    Action<T> onDespawn;

    public MemoryPool(Action<T> onSpawn = null, Action<T> onDespawn = null)
    {
        this.onSpawn = onSpawn;
        this.onDespawn = onDespawn;
    }

    public T Spawn()
    {
        T item = pool.Count > 0 ? pool.Pop() : new T();
        onSpawn?.Invoke(item);
        return item;
    }

    public void Despawn(T item)
    {
        onDespawn?.Invoke(item);
        pool.Push(item);
    }
}

#endregion

#region CChainInternal

/// <summary>
/// 协程链节点
/// </summary>
public class Chain
{
    public EType type;
    public MonoBehaviour player;
    public IEnumerator routine;
    public IEnumerator[] parallelRoutine;
    public Action action;

    public Coroutine Play()
    {
        switch (type)
        {
            case EType.NonCoroutine:
                action?.Invoke();
                return null;
            case EType.Parallel:
                return player.StartCoroutine(Parallel(parallelRoutine));
            case EType.Single:
                return player.StartCoroutine(routine);
            default:
                return null;
        }
    }

    public Chain SetupRoutine(IEnumerator r, MonoBehaviour p)
    {
        type = EType.Single;
        routine = r;
        player = p;
        return this;
    }

    public Chain SetupParallel(IEnumerator[] rs, MonoBehaviour p)
    {
        type = EType.Parallel;
        parallelRoutine = rs;
        player = p;
        return this;
    }

    public Chain SetupNon(Action a, MonoBehaviour p)
    {
        type = EType.NonCoroutine;
        action = a;
        player = p;
        return this;
    }

    public void Clear()
    {
        player = null;
        routine = null;
        action = null;
        parallelRoutine = null;
    }

    IEnumerator Parallel(IEnumerator[] routines)
    {
        int all = routines.Length;
        int done = 0;
        foreach (var r in routines)
        {
            player.StartChain().Play(r).Call(() => { done++; });
        }
        while (done < all) yield return null;
    }

    public enum EType
    {
        Single,
        Parallel,
        NonCoroutine
    }
}

/// <summary>
/// 协程链基类
/// </summary>
public class ChainBase : CustomYieldInstruction
{
    public static MemoryPool<ChainBase, MonoBehaviour> BasePool =
        new MemoryPool<ChainBase, MonoBehaviour>((c, m) => c.Setup(m), c => c.Clear());

    public static MemoryPool<Chain> ChainPool = new MemoryPool<Chain>(null, c => c.Clear());

    public MonoBehaviour player;
    public Queue<Chain> queue = new Queue<Chain>();
    public bool isPlaying = true;

    public override bool keepWaiting => isPlaying;

    public ChainBase Setup(MonoBehaviour p)
    {
        player = p;
        isPlaying = true;
        player.StartCoroutine(Routine());
        return this;
    }

    public void Clear()
    {
        player = null;
        queue.Clear();
    }

    IEnumerator Routine()
    {
        yield return null;
        while (queue.Count > 0)
        {
            var chain = queue.Dequeue();
            var cr = chain.Play();
            if (cr != null) yield return cr;
            ChainPool.Despawn(chain);
        }
        isPlaying = false;
        BasePool.Despawn(this);
    }

    // 核心方法
    public ChainBase Play(IEnumerator r) { queue.Enqueue(ChainPool.Spawn().SetupRoutine(r, player)); return this; }
    public ChainBase Wait(float t) { queue.Enqueue(ChainPool.Spawn().SetupRoutine(WaitRoutine(t), player)); return this; }
    public ChainBase Parallel(params IEnumerator[] rs) { queue.Enqueue(ChainPool.Spawn().SetupParallel(rs, player)); return this; }
    public ChainBase Sequential(params IEnumerator[] rs) { foreach (var r in rs) queue.Enqueue(ChainPool.Spawn().SetupRoutine(r, player)); return this; }
    public ChainBase Log(string s, ELogType t = ELogType.NORMAL) { queue.Enqueue(ChainPool.Spawn().SetupNon(ToAction(t, s), player)); return this; }
    public ChainBase Call(Action a) { queue.Enqueue(ChainPool.Spawn().SetupNon(a, player)); return this; }
    public ChainBase WaitFor(Func<bool> f) { queue.Enqueue(ChainPool.Spawn().SetupRoutine(WaitForRoutine(f), player)); return this; }
    public ChainBase Loop(float interval, int loopCount) { SetupLoop(interval, loopCount); return this; }

    // 新增方法：Loop
    private void SetupLoop(float interval, int loopCount)
    {
        // 克隆当前队列
        var originalQueue = CloneQueue();
        // 清空当前队列以重新安排循环逻辑
        queue.Clear();

        // 构造循环协程
        IEnumerator loopRoutine()
        {
            int count = 0;
            while (loopCount == -1 || count < loopCount)
            {
                // 依次执行队列中的任务
                foreach (var chain in originalQueue)
                {
                    var cr = chain.Play();
                    if (cr != null) yield return cr;
                }

                // 计数器处理
                if (loopCount != -1) count++;

                // 等待间隔时间
                yield return new WaitForSeconds(interval);
            }
        }

        // 将循环协程加入队列
        queue.Enqueue(ChainPool.Spawn().SetupRoutine(loopRoutine(), player));
    }

    // 辅助方法
    private Queue<Chain> CloneQueue()
    {
        var cloned = new Queue<Chain>();
        foreach (var chain in queue)
        {
            var newChain = ChainPool.Spawn();
            newChain.type = chain.type;
            newChain.player = chain.player;
            newChain.routine = chain.routine;
            newChain.parallelRoutine = chain.parallelRoutine;
            newChain.action = chain.action;
            cloned.Enqueue(newChain);
        }
        return cloned;
    }

    private Action ToAction(ELogType type, string log)
    {
        switch (type)
        {
            case ELogType.WARRNING:
                return () => Debug.LogWarning(log);
            case ELogType.ERROR:
                return () => Debug.LogError(log);
            default:
                return () => Debug.Log(log);
        }
    }

    private IEnumerator WaitRoutine(float t) { yield return new WaitForSeconds(t); }
    private IEnumerator WaitForRoutine(Func<bool> f)
    {
        while (!f()) yield return null;
    }
}

#endregion

/* 使用示例 */
// public class Example : MonoBehaviour
// {
//     void Start()
//     {
//         // 测试无限循环
//         this.StartChain()
//             .Log("开始无限循环")
//             .Loop(1f, -1) // 每1秒重复当前链
//             .Call(() => Debug.Log("循环体执行"))
//             .Wait(0.5f)
//             .Call(() => Debug.Log("等待0.5秒后继续循环"));
//
//         // 测试有限循环
//         this.StartChain()
//             .Log("开始有限循环")
//             .Loop(0.5f, 3) // 循环3次，每次间隔0.5秒
//             .Call(() => Debug.Log("有限循环体"))
//             .Wait(0.2f)
//             .Call(() => Debug.Log("有限循环结束"));
//
//         // 测试嵌套循环
//         this.StartChain()
//             .Log("开始嵌套循环")
//             .Loop(2f, 2) // 外层循环2次，间隔2秒
//             .Call(() => Debug.Log("外层循环"))
//             .Loop(0.5f, 3) // 内层循环3次，间隔0.5秒
//             .Call(() => Debug.Log("内层循环"))
//             .Wait(0.1f);
//     }
// }