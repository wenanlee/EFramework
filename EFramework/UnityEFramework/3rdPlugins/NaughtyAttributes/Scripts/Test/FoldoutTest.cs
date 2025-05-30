using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class FoldoutTest : MonoBehaviour
    {
        [NaFoldout("Integers")]
        public int int0;
        [NaFoldout("Integers")]
        public int int1;

        [NaFoldout("Floats")]
        public float float0;
        [NaFoldout("Floats")]
        public float float1;

        [NaFoldout("Sliders")]
        [NaMinMaxSlider(0, 1)]
        public Vector2 slider0;
        [NaFoldout("Sliders")]
        [NaMinMaxSlider(0, 1)]
        public Vector2 slider1;

        public string str0;
        public string str1;

        [NaFoldout("Transforms")]
        public Transform trans0;
        [NaFoldout("Transforms")]
        public Transform trans1;
    }
}
