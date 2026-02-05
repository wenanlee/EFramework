using EFramework.Unity.Entity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EntityScriptableObjectBase<TComponent> : ScriptableObject where TComponent : class
{
    [HideLabel]
    public EntityVolumeBase<TComponent> volume;
}
