using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HorizontalLineTest : MonoBehaviour
    {
        [NaHorizontalLine(color: EColor.Black)]
        [Header("Black")]
        [NaHorizontalLine(color: EColor.Blue)]
        [Header("Blue")]
        [NaHorizontalLine(color: EColor.Gray)]
        [Header("Gray")]
        [NaHorizontalLine(color: EColor.Green)]
        [Header("Green")]
        [NaHorizontalLine(color: EColor.Indigo)]
        [Header("Indigo")]
        [NaHorizontalLine(color: EColor.Orange)]
        [Header("Orange")]
        [NaHorizontalLine(color: EColor.Pink)]
        [Header("Pink")]
        [NaHorizontalLine(color: EColor.Red)]
        [Header("Red")]
        [NaHorizontalLine(color: EColor.Violet)]
        [Header("Violet")]
        [NaHorizontalLine(color: EColor.White)]
        [Header("White")]
        [NaHorizontalLine(color: EColor.Yellow)]
        [Header("Yellow")]
        [NaHorizontalLine(10.0f)]
        [Header("Thick")]
        public int line0;

        public HorizontalLineNest1 nest1;
    }

    [System.Serializable]
    public class HorizontalLineNest1
    {
        [NaHorizontalLine]
        public int line1;

        public HorizontalLineNest2 nest2;
    }

    [System.Serializable]
    public class HorizontalLineNest2
    {
        [NaHorizontalLine]
        public int line2;
    }
}
