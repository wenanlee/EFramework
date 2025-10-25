#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace EFramework.Unity.Entity
{
    public class EntityObject : MonoBehaviour
    {
        [InlineEditor(Expanded = true,ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public EntityVolume ComponentsVolume;
        private void Awake()=>Init();
        private void OnDestroy()=>Destroy();
        /// <summary>
        /// 编辑器中用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        [Button("编辑器初始化")]
        public virtual void EditorInit() { }
        /// <summary>
        /// 运行时用来初始化组件
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual void Init() { }
        public virtual void Destroy() { }
    }
}
