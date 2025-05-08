using System;
using EFramework.Tweens.Core;
using UnityEngine;

namespace EFramework.Tweens
{
    public static class FadeTween
    {
        public static Tween<float> TweenFade(this Component self, float to, float duration) =>
          Tween<float>.Add<Driver>(self).Finalize(duration, to);

        public static Tween<float> TweenFade(this CanvasGroup self, float to, float duration) =>
          Tween<float>.Add<Driver>(self).Finalize(duration, to);

        private class Driver : Tween<float>
        {
            private CanvasGroup canvasGroup;
            private float alpha;

            public override bool OnInitialize()
            {
                this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
                return this.canvasGroup != null;
            }
            public override float OnGetFrom()
            {
                return this.canvasGroup.alpha;
            }

            public override void OnUpdate(float easedTime)
            {
                this.alpha = this.canvasGroup.alpha;
                this.valueCurrent = this.InterpolateValue(this.valueFrom, this.valueTo, easedTime);
                this.canvasGroup.alpha = this.valueCurrent;
            }
        }
    }
}