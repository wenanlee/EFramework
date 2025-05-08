# UnityCoroutineWorkChain
unity 协程 链式编程
```C#
//方式1
        WorkChain works = new WorkChain();
        works.AddWork(() => { Debug.Log("test0"); })
            .RunAt(this);
//方式2
        this.Work()
            .WaitTime(1)
            .AddWork(() => { Debug.Log("test1"); })
            .WaitReturn(aaa, 0)
            .AddWork(() => { Debug.Log("test1"); })
            .Run();
```
