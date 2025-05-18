#if UNITY_EDITOR && TEST_FRAMEWORK_INSTALLED
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable PartialTypeWithSinglePart
using System;
using PrimeTween;
using UnityEngine;
using Assert = NUnit.Framework.Assert;

[ExecuteInEditMode]
public partial class EditModeTest : MonoBehaviour {
    [SerializeField] TweenSettings _settings = CreateSettings();
    static TweenSettings CreateSettings() {
        TweenSettings res = default;
        if (!PrimeTweenManager.HasInstance) {
            ExpectConstructorException(() => res = new TweenSettings(1, AnimationCurve.Linear(0, 0, 1, 1)));
        }
        return res;
    }

    Tween tween = TestConstructor();

    static Tween TestConstructor() {
        if (PrimeTweenManager.HasInstance) {
            if (PrimeTweenManager.Instance.tweensCount > 0) {
                ExpectConstructorException(() => Tween.StopAll());
            }
            return TestRegular();
        }
        ExpectConstructorException(() => Sequence.Create());
        ExpectConstructorException(() => PrimeTweenConfig.SetTweensCapacity(PrimeTweenManager.Instance.currentPoolCapacity + 1));
        ExpectConstructorException(() => PrimeTweenConfig.warnZeroDuration = !PrimeTweenConfig.warnZeroDuration);
        ExpectConstructorException(() => Tween.GlobalTimeScale(1f, 0.1f));
        ExpectConstructorException(() => Tween.GetTweensCount());
        ExpectConstructorException(() => {
            Sequence.Create()
                .ChainCallback(() => {})
                .InsertCallback(0f, delegate {})
                .Group(StartTween())
                .Chain(StartTween())
                .Insert(0f, Sequence.Create())
                .Insert(0, StartTween());
        });
        ExpectConstructorException(() => Tween.Delay(new object(), 1f, () => {}));
        ExpectConstructorException(() => Tween.Delay(new object(), 1f, _ => {}));
        ExpectConstructorException(() => Tween.Delay(1f, () => { }));
        ExpectConstructorException(() => Tween.Custom(0, 1, 1, delegate {}));
        return default;
    }

    static void ExpectConstructorException(Action action) {
        try {
            action();
            // Assert.Fail(nameof(action) + " should throw when called from constructor."); // calling Unity API is allowed from constructor when Editor is opening for the first time, so this assertion is commented out
        } catch (Exception e) {
            string message = e.Message;
            Assert.IsTrue(message.Contains("is not allowed to be called from a MonoBehaviour constructor"), message);
        }
    }

    static void test() {
        Tween.StopAll();
        TestRegular();
    }

    static Tween TestRegular() {
        PrimeTweenConfig.SetTweensCapacity(PrimeTweenManager.Instance.currentPoolCapacity + 1);
        Assert.DoesNotThrow(() => PrimeTweenConfig.warnZeroDuration = false);
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        Tween.GlobalTimeScale(1f, 0.1f);
        PrimeTweenConfig.warnEndValueEqualsCurrent = true;
        Tween.GetTweensCount();
        Sequence.Create()
            .ChainCallback(() => {})
            .InsertCallback(0f, delegate {})
            .Group(StartTween())
            .Chain(StartTween())
            .Insert(0f, Sequence.Create())
            .Insert(0, StartTween());
        Tween.Delay(new object(), 1f, () => {});
        Tween.Delay(new object(), 1f, _ => {});
        Tween.Delay(1f, () => { });
        return Tween.Custom(0, 1, 1, delegate {});
    }

    static Tween StartTween() => Tween.Custom(0f, 1f, 1f, delegate { });
    
    void Awake() => test();
    void OnValidate() => test();
    void Reset() => test();
    void OnEnable() => test();
    void OnDisable() => test();
    void OnDestroy() => test();
}

/*[UnityEditor.InitializeOnLoad]
public partial class EditModeTest {
    static EditModeTest() => TestConstructor();
    EditModeTest() => TestConstructor();

    [RuntimeInitializeOnLoadMethod]
    static void runtimeInitOnLoad() => test();
}*/
#endif