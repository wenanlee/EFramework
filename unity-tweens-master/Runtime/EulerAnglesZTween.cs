using EFramework.Tweens.Core;
using UnityEngine;

namespace EFramework.Tweens {
  public static class EulerAnglesZTween {
    public static Tween<float> TweenRotationZ (this Component self, float to, float duration) =>
      Tween<float>.Add<Driver> (self).Finalize (duration, to);

    public static Tween<float> TweenRotationZ (this GameObject self, float to, float duration) =>
      Tween<float>.Add<Driver> (self).Finalize (duration, to);

    private class Driver : Tween<float> {
      private Quaternion quaternionValueFrom;
      private Quaternion quaternionValueTo;

      public override bool OnInitialize () {
        return true;
      }

      public override float OnGetFrom () {
        return this.transform.eulerAngles.z;
      }

      public override void OnUpdate (float easedTime) {
        this.quaternionValueFrom = Quaternion.Euler (this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.valueFrom);
        this.quaternionValueTo = Quaternion.Euler (this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.valueTo);
        this.transform.rotation = Quaternion.Lerp (this.quaternionValueFrom, this.quaternionValueTo, easedTime);
      }
    }
  }
}