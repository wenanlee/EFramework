using System;
using System.Collections;
using System.Collections.Generic;
using CommandTerminal;
using NaughtyAttributes;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [Serializable]
    public class CommandHandler
    {
        [NaLabel("命令执行主体，可为空")]
        public string commandHandlerId;
        [NaLabel("触发的事件")]
        [NaDropdown("GetInstructionType")]
        public string eventNameId;
        [NaLabel("命令行参数")]
        
        public List<string> commandChainId;
    }
}
