using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class OnValueChangedTest : MonoBehaviour
    {
        [NaOnValueChanged("OnValueChangedMethod1")]
        [NaOnValueChanged("OnValueChangedMethod2")]
        public int int0;

        private void OnValueChangedMethod1()
        {
            Debug.LogFormat("int0: {0}", int0);
        }

        private void OnValueChangedMethod2()
        {
            Debug.LogFormat("int0: {0}", int0);
        }

        public OnValueChangedNest1 nest1;
    }

    [System.Serializable]
    public class OnValueChangedNest1
    {
        [NaOnValueChanged("OnValueChangedMethod")]
        [NaAllowNesting]
        public int int1;

        private void OnValueChangedMethod()
        {
            Debug.LogFormat("int1: {0}", int1);
        }

        public OnValueChangedNest2 nest2;
    }

    [System.Serializable]
    public class OnValueChangedNest2
    {
        [NaOnValueChanged("OnValueChangedMethod")]
        [NaAllowNesting]
        public int int2;

        private void OnValueChangedMethod()
        {
            Debug.LogFormat("int2: {0}", int2);
        }
    }
}
