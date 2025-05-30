using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class MinMaxValueTest : MonoBehaviour
    {
        [NaMinValue(0)]
        public int min0Int;

        [NaMaxValue(0)]
        public int max0Int;

        [NaMinValue(0), NaMaxValue(1)]
        public float range01Float;

        [NaMinValue(0), NaMaxValue(1)]
        public Vector2 range01Vector2;

        [NaMinValue(0), NaMaxValue(1)]
        public Vector3 range01Vector3;

        [NaMinValue(0), NaMaxValue(1)]
        public Vector4 range01Vector4;

        [NaMinValue(0)]
        public Vector2Int min0Vector2Int;

        [NaMaxValue(100)]
        public Vector2Int max100Vector2Int;

        [NaMinValue(0)]
        public Vector3Int min0Vector3Int;

        [NaMaxValue(100)]
        public Vector3Int max100Vector3Int;

        public MinMaxValueNest1 nest1;
    }

    [System.Serializable]
    public class MinMaxValueNest1
    {
        [NaMinValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int min0Int;

        [NaMaxValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int max0Int;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public float range01Float;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2 range01Vector2;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3 range01Vector3;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector4 range01Vector4;

        [NaMinValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int min0Vector2Int;

        [NaMaxValue(100)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int max100Vector2Int;

        [NaMinValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int min0Vector3Int;

        [NaMaxValue(100)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int max100Vector3Int;

        public MinMaxValueNest2 nest2;
    }

    [System.Serializable]
    public class MinMaxValueNest2
    {
        [NaMinValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int min0Int;

        [NaMaxValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int max0Int;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public float range01Float;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2 range01Vector2;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3 range01Vector3;

        [NaMinValue(0), NaMaxValue(1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector4 range01Vector4;

        [NaMinValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int min0Vector2Int;

        [NaMaxValue(100)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int max100Vector2Int;

        [NaMinValue(0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int min0Vector3Int;

        [NaMaxValue(100)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int max100Vector3Int;
    }
}
