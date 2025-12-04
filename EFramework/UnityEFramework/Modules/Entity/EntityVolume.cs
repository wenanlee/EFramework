using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Entity
{


    // 为了向后兼容，保留原有的非泛型版本
    [Serializable]
    public class EntityVolume : EntityVolumeBase<EntityComponent>
    {

    }
}