using EFramework.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Tweening
{
    public class TweeningFloat : TweeningBase
    {
        public float CurrentValue;
        public float StartValue;
        public float EndValue;

        protected override void OnInit()
        {
    
        }

        protected override void OnUpdateValue()
        {
            CurrentValue = Mathf.Lerp(StartValue, EndValue, Value);
        }

        /// <summary>
        /// Begin tweening.
        /// </summary>
        /// <param name="start">Start rotation.</param>
        /// <param name="end">End rotation.</param>
        /// <param name="time">Time for tweening.</param>
        public TweeningFloat Begin(float start, float end, float time)
        {
            enabled = false;
            StartValue = start;
            EndValue = end;
            TweenTime = time;
            enabled = true;
            return this;
        }

        /// <summary>
        /// Begin tweening at specified GameObject.
        /// </summary>
        /// <param name="go">Holder of tweener.</param>
        /// <param name="start">Start rotation.</param>
        /// <param name="end">End rotation.</param>
        /// <param name="time">Time for tweening.</param>
        public static TweeningRotate Begin(GameObject go, Vector3 start, Vector3 end, float time)
        {
            var tweener = Get<TweeningRotate>(go);
            if (tweener != null)
            {
                tweener.Begin(start, end, time);
            }
            return tweener;
        }
    }

}
