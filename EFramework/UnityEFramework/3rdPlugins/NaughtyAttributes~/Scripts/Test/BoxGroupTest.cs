using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class BoxGroupTest : MonoBehaviour
    {
        [NaBoxGroup("Integers")]
        public int int0;
        [NaBoxGroup("Integers")]
        public int int1;

        [NaBoxGroup("Floats")]
        public float float0;
        [NaBoxGroup("Floats")]
        public float float1;

        [NaBoxGroup("Sliders")]
        [NaMinMaxSlider(0, 1)]
        public Vector2 slider0;
        [NaBoxGroup("Sliders")]
        [NaMinMaxSlider(0, 1)]
        public Vector2 slider1;

        public string str0;
        public string str1;

        [NaBoxGroup]
        public Transform trans0;
        [NaBoxGroup]
        public Transform trans1;
    }
}
