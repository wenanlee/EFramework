// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.Tweening
{
    /// <summary>
    /// Tweening scale.
    /// </summary>
    public class TweeningScale : TweeningBase
    {
        /// <summary>
        /// Target transform. If null on start - current transform will be used.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// Start value of scale.
        /// </summary>
        public Vector3 StartValue = Vector3.one;

        /// <summary>
        /// End value of scale.
        /// </summary>
        public Vector3 EndValue = Vector3.one * 0.97f;

        protected override void OnInit()
        {
            if (Target == null)
            {
                Target = transform;
            }
        }

        protected override void OnUpdateValue()
        {
            if ((object)Target != null)
            {
                Target.localScale = Vector3.Lerp(StartValue, EndValue, Value);
            }
        }

        /// <summary>
        /// Begin tweening.
        /// </summary>
        /// <param name="start">Start scale.</param>
        /// <param name="end">End scale.</param>
        /// <param name="time">Time of tweening.</param>
        public TweeningScale Begin(Vector3 start, Vector3 end, float time)
        {
            enabled = false;
            StartValue = start;
            EndValue = end;
            TweenTime = time;
            enabled = true;
            return this;
        }

        /// <summary>
        /// Begin the specified go, start, end and time.
        /// </summary>
        /// <param name="go">Holder of tweener.</param>
        /// <param name="start">Start scale.</param>
        /// <param name="end">End scale.</param>
        /// <param name="time">Time of tweening.</param>
        public static TweeningScale Begin(GameObject go, Vector3 start, Vector3 end, float time)
        {
            var tweener = Get<TweeningScale>(go);
            if (tweener != null)
            {
                tweener.Begin(start, end, time);
            }
            return tweener;
        }
    }
}
public static class ExtensionTweeningScale
{
    public static void TweeningLocalScaleTo(this Transform transform, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningScale.Begin(transform.gameObject, transform.localScale, end, time);
    }
    public static void TweeningLocalScaleTo(this GameObject gameObject, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningScale.Begin(gameObject, gameObject.transform.localScale, end, time);
    }
}