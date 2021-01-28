using EFramework.Tweens.Core;
using UnityEngine;

namespace EFramework.Tweens {
  public static class LocalPositionTween {
    public static Tween<Vector3> TweenLocalPosition (this Component self, Vector3 to, float duration) =>
      Tween<Vector3>.Add<Driver> (self).Finalize (duration, to);

    public static Tween<Vector3> TweenLocalPosition (this GameObject self, Vector3 to, float duration) =>
      Tween<Vector3>.Add<Driver> (self).Finalize (duration, to);

    private class Driver : Tween<Vector3> {
      public override bool OnInitialize () {
        return true;
      }

      public override Vector3 OnGetFrom () {
        return this.transform.localPosition;
      }

      public override void OnUpdate (float easedTime) {
        this.valueCurrent.x = this.InterpolateValue (this.valueFrom.x, this.valueTo.x, easedTime);
        this.valueCurrent.y = this.InterpolateValue (this.valueFrom.y, this.valueTo.y, easedTime);
        this.valueCurrent.z = this.InterpolateValue (this.valueFrom.z, this.valueTo.z, easedTime);
        this.transform.localPosition = this.valueCurrent;
      }
    }
  }
}