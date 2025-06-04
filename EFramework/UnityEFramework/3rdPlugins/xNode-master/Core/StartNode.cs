using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.XNode.Core
{
    [CreateNodeMenu("BaseNode/StartNode")]
    public class StartNode : NodeBase
    {
        [Output] public Empty output;
    }
}
