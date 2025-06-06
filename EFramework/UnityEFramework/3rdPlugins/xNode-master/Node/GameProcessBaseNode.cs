using EFramework.Unity.XNode.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameProcessBaseNode : ProcessNodeBase
{
    public virtual IEnumerator ExecuteCoroutine()
    {
        //yield return new WaitForSeconds(1);
        //yield return XXXXX();// 具体方法写在这里
        //yield return new WaitForSeconds(1);
        yield return null;
    }
    public override void Execute()
    {
        NodeTempMonoBehaviour.Instance.StartCoroutine(ExecuteCoroutine());
        base.Execute();
    }
}
