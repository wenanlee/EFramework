using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Unity.XNode;

public class ValueNodeBase<T> : Node {
    [Output]
    public T output;
    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }
}