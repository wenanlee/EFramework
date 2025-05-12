using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Pool
{
    using UnityEngine;
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public class PoolMgr : MonoSingleton<PoolMgr>, I_104_GameInit
    {
        /**
        [Header("HP")]
        [SerializeField]
        private Transform hpContainer;
        [SerializeField]
        private Transform hpTemplate;
        public ObjPool<Transform> hpPool;

        [Header("UI Asset")]
        public AssetPool<Sprite> uiPool;

        public virtual void OnGameInit()
        {
            hpPool = new ObjPool<Transform>(hpTemplate, hpContainer);
            uiPool = new AssetPool<Sprite>();
        }
         */

        public virtual void OnGameInit()
        {

        }
    }
}
