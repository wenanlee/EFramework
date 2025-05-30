using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class LabelTest : MonoBehaviour
    {
        [NaLabel("Label 0")]
        public int int0;

        public LabelNest1 nest1;
    }

    [System.Serializable]
    public class LabelNest1
    {
        [NaLabel("Label 1")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int int1;

        public LabelNest2 nest2;
    }

    [System.Serializable]
    public class LabelNest2
    {
        [NaLabel("Label 2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 vector2;
    }
}
