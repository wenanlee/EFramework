using System;
using System.Collections;
using System.Collections.Generic;
using CommandTerminal;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [Serializable]
    public class CommandHandler
    {
        [LabelText("命令执行主体，可为空")]
        public string commandHandlerId;
        [LabelText("触发的事件")]
        [ValueDropdown("GetInstructionType")]
        public string eventNameId;
        [LabelText("命令行参数")]
        
        public List<string> commandChainId;
    }
}
