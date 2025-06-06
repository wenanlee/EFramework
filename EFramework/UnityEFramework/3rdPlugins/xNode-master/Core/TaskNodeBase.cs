using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.XNode.Core
{
    public abstract class ProcessNodeBase : NodeBase
    {
        [Input]
        public Empty enter;
        [Output]
        public Empty exit;
    }
}
