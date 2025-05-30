using System;
using System.Collections;
using System.Collections.Generic;
using CommandTerminal;
using EditorAttributes;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [Serializable]
    public class CommandHandler
    {
        [Rename("命令执行主体，可为空")]
        public string commandHandlerId;
        [Rename("触发的事件")]
        [Dropdown("GetInstructionType")]
        public string eventNameId;
        [Rename("命令行参数")]
        
        public List<string> commandChainId;
    }
}
