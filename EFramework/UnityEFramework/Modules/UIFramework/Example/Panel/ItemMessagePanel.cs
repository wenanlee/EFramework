using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EFramework.Tweens;
namespace EFramework.Unity.UIFramework
{
    /// <summary>
    /// 物品信息面板
    /// </summary>
    public class ItemMessagePanel : UIBase
    {

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// 面板进入
        /// </summary>
        public override void OnEnter()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

            // 放大动画
            transform.localScale = Vector3.zero;
            transform.TweenLocalScale(Vector3.one, 0.2f);
        }

        /// <summary>
        /// 处理面板的关闭
        /// </summary>
        public override void OnExit()
        {
            canvasGroup.blocksRaycasts = false; // 停止鼠标交互

            // 缩小动画
            transform.TweenLocalScale(Vector3.zero, 0.2f).SetOnComplete(() =>
            {
                canvasGroup.alpha = 0; // 隐藏
                Destroy(gameObject);
            });
        }

        /// <summary>
        /// 关闭当前面板
        /// </summary>
        public void OnClosePanel()
        {
            UIManager.Instance.HideUI();
        }
    }
}