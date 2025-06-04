using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.XNode.Core
{
    [CreateNodeMenu("BaseNode/TaskNodeBase")]
    public class TaskNodeBase : NodeBase
    {
        [Input]
        public Empty input;
        [Output]
        public Empty output;
    }
}
