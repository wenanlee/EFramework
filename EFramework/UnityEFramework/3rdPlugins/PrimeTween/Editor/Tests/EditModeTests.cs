#if TEST_FRAMEWORK_INSTALLED
using NUnit.Framework;
using PrimeTween;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = NUnit.Framework.Assert;
using AssertionException = UnityEngine.Assertions.AssertionException;

public class EditModeTests {
    [Test]
    public void TestEditMode() {
        Tween.StopAll();
        Assert.AreEqual(0, PrimeTweenManager.Instance.tweensCount);
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        PrimeTweenConfig.warnZeroDuration = false;
        expectError();
        Tween.Custom(0, 1, 1, delegate {});
        var go = new GameObject();
        {
            expectError();
            Tween.Alpha(go.AddComponent<SpriteRenderer>(), 0, 1);
            expectError();
            Tween.Delay(1);
            expectError();
            Tween.Delay(0);
            expectError();
            Tween.CompleteAll();
            PrimeTweenConfig.warnEndValueEqualsCurrent = true;
            expectError();
            Tween.StopAll();
            expectError();
            Tween.SetPausedAll(true);
            expectError();
            Tween.ShakeLocalPosition(go.transform, Vector3.one, 1);
            expectError();
            Tween.ShakeCustom(go, Vector3.zero, new ShakeSettings(Vector3.one, 1), delegate {});
            expectError();
            Sequence.Create();
            expectError();
            Tween.GlobalTimeScale(0.5f, 0.1f);
            expectError();
            Tween.GetTweensCount(this);
            expectError();
            Tween.GetTweensCount();
            expectError();
            Sequence.Create(Tween.Delay(0.01f));

            TweenSettings.ValidateCustomCurveKeyframes(AnimationCurve.Linear(0, 0, 1, 1));
            PrimeTweenConfig.SetTweensCapacity(10);
            Assert.DoesNotThrow(() => PrimeTweenConfig.defaultEase = Ease.InCirc);
        }
        Object.DestroyImmediate(go);
        void expectError() {
        }
        LogAssert.NoUnexpectedReceived();
        PrimeTweenConfig.warnEndValueEqualsCurrent = true;
        PrimeTweenConfig.warnZeroDuration = true;
    }

    #if PRIME_TWEEN_SAFETY_CHECKS
    [Test]
    public void DebugStackTraces() {
        StackTraces.logDebugInfo();
    }
    #endif
}
#endif
