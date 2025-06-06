using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EFramework.Unity.XNode.Node;
[CreateNodeMenu("流程节点/场景加载节点"), NodeWidth(500)]
public class SceneLoadNode : GameProcessBaseNode
{
    [ValueDropdown(nameof(loadModeStr))]
    public string LoadMode;
    [ValueDropdown("@OdinDataBinding.SceneSO.GetItems()")]
    public string scenePath;

    private string[] loadModeStr= new string[] { "Sync", "Async" };
 
    public override IEnumerator ExecuteCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (scenePath != null)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(scenePath);
            yield return new WaitForSeconds(0.1f);
            GameObject sceneInstance = GameObject.Instantiate(prefab);
            // 这里可以添加更多的初始化逻辑
        }
        else
        {
            Debug.LogWarning("Scene prefab is not assigned.");
        }
        yield return new WaitForSeconds(1);
    }
}