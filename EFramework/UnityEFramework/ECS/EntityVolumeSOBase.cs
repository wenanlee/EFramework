using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.ECS
{
    public class EntityVolumeSOBase<TComponent>:ScriptableObject where TComponent : ComponentBase
    {
        [HideLabel]
        public EntityVolumeBase<TComponent> volume;
    }
}