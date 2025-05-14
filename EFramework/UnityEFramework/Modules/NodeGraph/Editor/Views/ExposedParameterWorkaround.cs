using UnityEngine;
using System;
using System.Collections.Generic;

namespace EFramework.Unity.Node
{
    [Serializable]
    public class ExposedParameterWorkaround : ScriptableObject
    {
        [SerializeReference]
        public List<ExposedParameter>   parameters = new List<ExposedParameter>();
        public BaseGraph                graph;
    }
}