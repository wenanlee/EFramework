#if ODIN_INSPECTOR
using EFramework.Unity.DataTable;
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class GameEntity : MonoBehaviour
    {
        [HideIf("@UnityEngine.Application.isPlaying"), LabelText("Components(编辑器)")]
        [InlineEditor(Expanded = true), SerializeField]
        [InlineButton("CreateNewScriptableObject", "新建",ShowIf = "@ComponentsVolumeSO == null")]
        public EntityVolumeSO ComponentsVolumeSO;
        [ShowIf("@UnityEngine.Application.isPlaying"), LabelText("Components(运行时)")]
        public EntityVolume ComponentsVolume;
        private void Awake() => Init();
        private void OnDestroy() => Destroy();
        /// <summary>
        /// 编辑器中用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        [Button("编辑器初始化")]
        public virtual void EditorInit()
        {
            if (name.Contains("UUID") == false)
            {
                name += "_UUID" + UUID.New();
            }




        }
        /// <summary>
        /// 运行时用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Init()
        {

            //var volume = ScriptableObjectUtility.FindScriptableObject<EntityVolume>(name);
            //if (ComponentsVolume == null)
            //{
            //    if (name.Contains("DO") || name.Contains("SS"))
            //        Debug.LogError($"EntityObject<{name}>的ComponentsVolume为空,请先编辑器初始化");
            //    return;
            //}
            //if (volume == null)
            //{
            //    if (name.Contains("DO") || name.Contains("SS"))
            //        Debug.LogError($"EntityObject<{name}>的ComponentsVolume找不到,请先编辑器初始化");
            //    return;
            //}
            //ComponentsVolume = Instantiate(volume);
            //ComponentsVolume.InitAllComponent(this);

        }

        private void CreateNewScriptableObject()
        {
            if (ComponentsVolumeSO == null || ComponentsVolumeSO.name != name)
            {
                ComponentsVolumeSO = ScriptableObjectUtility.FindScriptableObject<EntityVolumeSO>(name);
                if (ComponentsVolumeSO == null)
                {
                    ComponentsVolumeSO = ScriptableObjectUtility.CreateScriptableObject<EntityVolumeSO>(ProjectConfig.Instance.soDataPath + "CompetentsVolume/", name);
                    ComponentsVolumeSO.volume.Uuid = name.GetUUID();
                }
                foreach (var item in ComponentsVolumeSO.volume.components)
                {
                    item.EditorInit(this);
                }
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(ComponentsVolumeSO);
#endif
        }
        public virtual void Destroy() { }
    }
}
