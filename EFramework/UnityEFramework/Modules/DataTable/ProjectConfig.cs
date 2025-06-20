using EFramework.Unity.Command;
using NaughtyAttributes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EFramework.Unity.DataTable
{

    public class ProjectConfig : ScriptableObject
    {
        [LabelText("项目名称")]
        public string projectName;

        [LabelText("项目路径")]
        [FolderPath(ParentFolder = "")]
        public string projectParentPath;
        [ShowInInspector, TableList,LabelText("表单")]
        public List<TableInfo> tables =new List<TableInfo>();
    }
}
