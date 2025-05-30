using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ReadOnlyTest : MonoBehaviour
    {
        [NaReadOnly]
        public int readOnlyInt = 5;

        public ReadOnlyNest1 nest1;
    }

    [System.Serializable]
    public class ReadOnlyNest1
    {
        [NaReadOnly]
        [NaAllowNesting]
        public float readOnlyFloat = 3.14f;

        public ReadOnlyNest2 nest2;
    }

    [System.Serializable]
    public struct ReadOnlyNest2
    {
        [NaReadOnly]
        [NaAllowNesting]
        public string readOnlyString;
    }
}
