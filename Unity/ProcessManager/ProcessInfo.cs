using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProcessInfoBase : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void Run() { }
    public virtual void Exit() { }
}