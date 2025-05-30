using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniversalDictionary", menuName = "Universal/Dictionary")]
public class UniversalDictionarySO : ScriptableObject
{
    public List<UniversalKeyValuePair> items = new List<UniversalKeyValuePair>();
    //{
    //    new UniversalKeyValuePair
    //    {
    //        key = "ExampleKey",
    //        type = typeof(string),
    //        value = "ExampleValue"
    //    },
    //    new UniversalKeyValuePair
    //    {
    //        key = "ExampleInt",
    //        type = typeof(int),
    //        value = 42
    //    },
    //    new UniversalKeyValuePair
    //    {
    //        key = "ExampleFloat",
    //        type = typeof(MyClass),
    //        value = new MyClass()
    //    },
    //};
    //[Button]
    public void add()
    {
        items.Add(new UniversalKeyValuePair() { key = "名字", type = typeof(string), value = "李佳琦" });
        items.Add(new UniversalKeyValuePair() { key = "年龄", type = typeof(int), value = 30 });
        items.Add(new UniversalKeyValuePair() { key = "模型", type = typeof(GameObject), value = null });
        items.Add(new UniversalKeyValuePair() { key = "其他", type = typeof(MyClass), value = new MyClass() });
    }
}

internal class MyClass
{
    public int id;
    public string name;
    public string description;
}

[Serializable]
public class UniversalKeyValuePair
{
    public string key;
    public Type type;
    public object value;
}