using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class EnableIfTest : MonoBehaviour
    {
        public bool enable1;
        public bool enable2;
        public EnableIfEnum enum1;
        [NaEnumFlags] public EnableIfEnumFlag enum2;

        [NaEnableIf(EConditionOperator.And, "enable1", "enable2")]
        [NaReorderableList]
        public int[] enableIfAll;

        [NaEnableIf(EConditionOperator.Or, "enable1", "enable2")]
        [NaReorderableList]
        public int[] enableIfAny;

        [NaEnableIf("enum1", EnableIfEnum.Case0)]
        [NaReorderableList]
        public int[] enableIfEnum;

        [NaEnableIf("enum2", EnableIfEnumFlag.Flag0)]
        [NaReorderableList]
        public int[] enableIfEnumFlag;

        [NaEnableIf("enum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [NaReorderableList]
        public int[] enableIfEnumFlagMulti;

        public EnableIfNest1 nest1;
    }

    [System.Serializable]
    public class EnableIfNest1
    {
        public bool enable1;
        public bool enable2;
        public EnableIfEnum enum1;
        [NaEnumFlags] public EnableIfEnumFlag enum2;
        public bool Enable1 { get { return enable1; } }
        public bool Enable2 { get { return enable2; } }
        public EnableIfEnum Enum1 { get { return enum1; } }
        public EnableIfEnumFlag Enum2 { get { return enum2; } }

        [NaEnableIf(EConditionOperator.And, "Enable1", "Enable2")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfAll;

        [NaEnableIf(EConditionOperator.Or, "Enable1", "Enable2")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfAny;

        [NaEnableIf("Enum1", EnableIfEnum.Case1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfEnum;

        [NaEnableIf("Enum2", EnableIfEnumFlag.Flag0)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfEnumFlag;

        [NaEnableIf("Enum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfEnumFlagMulti;

        public EnableIfNest2 nest2;
    }

    [System.Serializable]
    public class EnableIfNest2
    {
        public bool enable1;
        public bool enable2;
        public EnableIfEnum enum1;
        [NaEnumFlags] public EnableIfEnumFlag enum2;
        public bool GetEnable1() { return enable1; }
        public bool GetEnable2() { return enable2; }
        public EnableIfEnum GetEnum1() { return enum1; }
        public EnableIfEnumFlag GetEnum2() { return enum2; }

        [NaEnableIf(EConditionOperator.And, "GetEnable1", "GetEnable2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfAll = new Vector2(0.25f, 0.75f);

        [NaEnableIf(EConditionOperator.Or, "GetEnable1", "GetEnable2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfAny = new Vector2(0.25f, 0.75f);

        [NaEnableIf("GetEnum1", EnableIfEnum.Case2)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfEnum = new Vector2(0.25f, 0.75f);

        [NaEnableIf("GetEnum2", EnableIfEnumFlag.Flag0)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfEnumFlag;

        [NaEnableIf("GetEnum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfEnumFlagMulti;
    }

    [System.Serializable]
    public enum EnableIfEnum
    {
        Case0,
        Case1,
        Case2
    }

    [Flags]
    public enum EnableIfEnumFlag
    {
        Flag0 = 1,
        Flag1 = 2,
        Flag2 = 4,
        Flag3 = 8
    }
}
