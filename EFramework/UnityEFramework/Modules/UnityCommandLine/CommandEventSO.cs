using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFramework.Unity.Command
{
    [CreateAssetMenu(fileName = "CommandEventSO", menuName = "EFramework/UnityCommandLine/CommandEventSO", order = 1)]
    public class CommandEventSO : ScriptableObject
    {
        public List<commandEventArgs> commandEvents = new List<commandEventArgs>();
    }
    [Serializable]
    public class commandEventArgs
    {
        public string uuid;
        [LabelText("命令名称")]
        public string commandName;
        [LabelText("命令分组")]
        [ValueDropdown("GetGroupNames")]
        public string group;
        [LabelText("命令简介")]
        public string description;
        
        public commandEventArgs()
        {
            uuid = UUID.New();
        }
        public IEnumerable GetGroupNames()
        {
            List<string> groupNames = new();
            groupNames.Add("默认分组");
            groupNames.Add("其他分组");
            return groupNames;
        }
    }
}
