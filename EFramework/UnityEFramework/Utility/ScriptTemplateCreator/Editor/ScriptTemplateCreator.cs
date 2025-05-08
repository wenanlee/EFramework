#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class ScriptTemplateCreator : EditorWindow
{
    private string scriptName = "TestScript";

    [MenuItem("Assets/Create/Custom IBL Script", false, 80)]
    private static void CreateCustomScript()
    {
        GetWindow<ScriptTemplateCreator>("Create IBL Script").Show();
    }

    void OnGUI()
    {
        GUILayout.Label("创建节点脚本", EditorStyles.boldLabel);
        scriptName = EditorGUILayout.TextField("脚本名称", scriptName);

        if (GUILayout.Button("创建") && !string.IsNullOrEmpty(scriptName))
        {
            CreateScript();
            Close();
        }
    }

    private void CreateScript()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!Directory.Exists(path))
        {
            path = Path.GetDirectoryName(path);
        }

        string fullPath = Path.Combine(path, scriptName + ".cs");
        string template = 
            $@"using UnityEngine;
[System.Serializable]
public class {scriptName}Info : InfoBase
{{
    // 在此添加数据字段
    // public int exampleValue;
}}

[System.Serializable]
public class {scriptName}Logic
{{
    public {scriptName}Info info;
    public {scriptName} bridge;

    public {scriptName}Logic({scriptName} bridge, {scriptName}Info info)
    {{
        this.bridge = bridge;
        this.info = info;
    }}

    // 在此添加逻辑方法
    // public void UpdateLogic() {{ }}
}}

public class {scriptName} : MonoBehaviour
{{
    public {scriptName}Logic logic;

    public void Init({scriptName}Info info)
    {{
        logic = new {scriptName}Logic(this, info);
    }}

    public void Refresh({scriptName}Info info)
    {{
        // 刷新逻辑
        // logic.UpdateLogic();
    }}
}}";

        File.WriteAllText(fullPath, template);
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Object created = AssetDatabase.LoadAssetAtPath(fullPath, typeof(Object));
        ProjectWindowUtil.ShowCreatedAsset(created);
    }
}
#endif