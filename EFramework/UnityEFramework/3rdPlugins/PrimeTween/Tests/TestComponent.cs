#if TEST_FRAMEWORK_INSTALLED
using System.Collections;
using UnityEngine;

public class TestComponent : MonoBehaviour {
    IEnumerator Start() {
        var test = new Tests();
        yield return test.SequenceNestingInfinite();
    }
}
#endif