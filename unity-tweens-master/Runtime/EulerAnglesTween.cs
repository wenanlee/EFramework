using EFramework.Tweens.Core;
using UnityEngine;

namespace EFramework.Tweens {
  public static class EulerAnglesTween {
    public static Tween<Vector3> TweenRotation (this Component self, Vector3 to, float duration) =>
      Tween<Vector3>.Add<Driver> (self).Finalize (duration, to);

    public static Tween<Vector3> TweenRotation (this GameObject self, Vector3 to, float duration) =>
      Tween<Vector3>.Add<Driver> (self).Finalize (duration, to);

    private class Driver : Tween<Vector3> {
      private Quaternion quaternionValueFrom;
      private Quaternion quaternionValueTo;

      public override bool OnInitialize () {
        this.quaternionValueTo = Quaternion.Euler (this.valueTo);
        return true;
      }

      public override Vector3 OnGetFrom () {
        var _from = this.transform.eulerAngles;
        this.quaternionValueFrom = Quaternion.Euler (_from);
        return _from;
      }

      public override void OnUpdate (float easedTime) {
        if (easedTime == 0)
          this.transform.rotation = this.quaternionValueFrom;
        else if (easedTime == 1)
          this.transform.rotation = this.quaternionValueTo;
        else
          this.transform.rotation = Quaternion.Lerp (this.quaternionValueFrom, this.quaternionValueTo, easedTime);
      }
    }
  }
}