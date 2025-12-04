#if ODIN_INSPECTOR
using EFramework.Unity.Utility;
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class GameEntity : MonoBehaviour
    {
        [InlineEditor(Expanded = true/*, ObjectFieldMode = InlineEditorObjectFieldModes.Hidden*/)]
        public EntityVolume ComponentsVolume;
        private void Awake() => Init();
        private void OnDestroy() => Destroy();
        private void OnValidate()
        {
            EditorInit();
        }
        /// <summary>
        /// 编辑器中用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        [Button("编辑器初始化")]
        public virtual void EditorInit()
        {

            if (ComponentsVolume == null || ComponentsVolume.name!=name)
            {
                ComponentsVolume = ScriptableObjectUtility.FindScriptableObject<EntityVolume>(name);
                if (ComponentsVolume == null)
                    ComponentsVolume = ScriptableObjectUtility.CreateScriptableObject<EntityVolume>("Assets/GameMain/Resources/Data/CompetentsVolume", name);
                foreach (var item in ComponentsVolume.components)
                {
                    item.EditorInit(this);
                }
            }
            if (name.Contains("UUID") == false)
            {
                ComponentsVolume.Uuid = UUID.New();
                name += "_UUID" + ComponentsVolume.Uuid;
            }
            else
            {
                ComponentsVolume.Uuid = name.GetUUID();
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(ComponentsVolume);
#endif
        }
        /// <summary>
        /// 运行时用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Init()
        {

            var volume = ScriptableObjectUtility.FindScriptableObject<EntityVolume>(name);
            if (ComponentsVolume == null)
            {
                if(name.Contains("DO")||name.Contains("SS"))
                    Debug.LogError($"EntityObject<{name}>的ComponentsVolume为空,请先编辑器初始化");
                return;
            }
            if(volume == null)
            {
                if (name.Contains("DO") || name.Contains("SS"))
                    Debug.LogError($"EntityObject<{name}>的ComponentsVolume找不到,请先编辑器初始化");
                return;
            }
            ComponentsVolume = Instantiate(volume);
            ComponentsVolume.InitAllComponent(this);
        }
        public virtual void Destroy() { }
    }
}
