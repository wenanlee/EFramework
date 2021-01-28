// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.Tweening
{
    /// <summary>
    /// Tweening rotation.
    /// </summary>
    public class TweeningRotate : TweeningBase
    {
        /// <summary>
        /// Target transform. If null on start - current transform will be used.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// Start value of rotation in degrees.
        /// </summary>
        public Vector3 StartValue = Vector3.zero;

        /// <summary>
        /// End value of rotation in degrees.
        /// </summary>
        public Vector3 EndValue = Vector3.zero;

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

                Target.localRotation = Quaternion.Euler(Vector3.Lerp(StartValue, EndValue, Value));
            }
        }

        /// <summary>
        /// Begin tweening.
        /// </summary>
        /// <param name="start">Start rotation.</param>
        /// <param name="end">End rotation.</param>
        /// <param name="time">Time for tweening.</param>
        public TweeningRotate Begin(Vector3 start, Vector3 end, float time)
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
public static class ExtensionTweeningRotate
{
    public static void TweeningLocalRotateTo(this Transform transform, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles, end, time);
    }
    public static void TweeningLocalRotateTo(this GameObject gameObject, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles, end, time);
    }
    public static void TweeningLocalRotateTo(this Transform transform, Vector3 start, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, start, end, time);
    }
    public static void TweeningLocalRotateTo(this GameObject gameObject, Vector3 start, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, start, end, time);
    }
    #region X
    public static void TweeningLocalRotateXTo(this Transform transform, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles, end, time);
    }
    public static void TweeningLocalRotateXTo(this GameObject gameObject, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles, end, time);
    }
    public static void TweeningLocalRotateXTo(this Transform transform, float start, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles.SetX(start).SetY(0).SetZ(0), transform.rotation.eulerAngles.SetX(end).SetY(0).SetZ(0), time);
    }
    public static void TweeningLocalRotateXTo(this GameObject gameObject, float start, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles.SetX(start).SetY(0).SetZ(0), gameObject.transform.rotation.eulerAngles.SetX(end).SetY(0).SetZ(0), time);
    }
    #endregion

    #region Y
    public static void TweeningLocalRotateYTo(this Transform transform, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles, transform.rotation.eulerAngles.SetX(0).SetY(end).SetZ(0), time);
    }
    public static void TweeningLocalRotateYTo(this GameObject gameObject, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles, gameObject.transform.rotation.eulerAngles.SetX(0).SetY(end).SetZ(0), time);
    }
    public static void TweeningLocalRotateYTo(this Transform transform, float start, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles.SetX(start).SetY(0).SetZ(0), transform.rotation.eulerAngles.SetX(end).SetY(0).SetZ(0), time);
    }
    public static void TweeningLocalRotateYTo(this GameObject gameObject, float start, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles.SetX(start).SetY(0).SetZ(0), gameObject.transform.rotation.eulerAngles.SetX(end).SetY(0).SetZ(0), time);
    }
    #endregion

    #region Z
    public static void TweeningLocalRotateZTo(this Transform transform, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles, end, time);
    }
    public static void TweeningLocalRotateZTo(this GameObject gameObject, Vector3 end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles, end, time);
    }
    public static void TweeningLocalRotateZTo(this Transform transform, float start, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(transform.gameObject, transform.rotation.eulerAngles.SetX(start).SetY(0).SetZ(0), transform.rotation.eulerAngles.SetX(end).SetY(0).SetZ(0), time);
    }
    public static void TweeningLocalRotateZTo(this GameObject gameObject, float start, float end, float time)
    {
        EFramework.Tweening.TweeningRotate.Begin(gameObject, gameObject.transform.rotation.eulerAngles.SetX(start).SetY(0).SetZ(0), gameObject.transform.rotation.eulerAngles.SetX(end).SetY(0).SetZ(0), time);
    }
    #endregion

    public static void SetX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public static Vector3 SetX(this Vector3 position, float x)
    {
        return new Vector3(x, position.y, position.z);
    }
    public static void SetY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public static Vector3 SetY(this Vector3 position, float y)
    {
        return new Vector3(position.x, y, position.z);
    }
    public static void SetZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
    public static Vector3 SetZ(this Vector3 position, float z)
    {
        return new Vector3(position.x, position.y, z);
    }
}