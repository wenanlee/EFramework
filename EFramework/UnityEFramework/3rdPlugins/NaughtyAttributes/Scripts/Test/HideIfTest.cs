using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HideIfTest : MonoBehaviour
    {
        public bool hide1;
        public bool hide2;
        public HideIfEnum enum1;
        [NaEnumFlags] public HideIfEnumFlag enum2;

        [NaHideIf(EConditionOperator.And, "hide1", "hide2")]
        [NaReorderableList]
        public int[] hideIfAll;

        [NaHideIf(EConditionOperator.Or, "hide1", "hide2")]
        [NaReorderableList]
        public int[] hideIfAny;

        [NaHideIf("enum1", HideIfEnum.Case0)]
        [NaReorderableList]
        public int[] hideIfEnum;

        [NaHideIf("enum2", HideIfEnumFlag.Flag0)]
        [NaReorderableList]
        public int[] hideIfEnumFlag;

        [NaHideIf("enum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [NaReorderableList]
        public int[] hideIfEnumFlagMulti;

        public HideIfNest1 nest1;
    }

    [System.Serializable]
    public class HideIfNest1
    {
        public bool hide1;
        public bool hide2;
        public HideIfEnum enum1;
        [NaEnumFlags] public HideIfEnumFlag enum2;
        public bool Hide1 { get { return hide1; } }
        public bool Hide2 { get { return hide2; } }
        public HideIfEnum Enum1 { get { return enum1; } }
        public HideIfEnumFlag Enum2 { get { return enum2; } }

        [NaHideIf(EConditionOperator.And, "Hide1", "Hide2")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int hideIfAll;

        [NaHideIf(EConditionOperator.Or, "Hide1", "Hide2")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int hideIfAny;

        [NaHideIf("Enum1", HideIfEnum.Case1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int hideIfEnum;

        [NaHideIf("Enum2", HideIfEnumFlag.Flag0)]
        [NaAllowNesting]
        public int hideIfEnumFlag;

        [NaHideIf("Enum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [NaAllowNesting]
        public int hideIfEnumFlagMulti;

        public HideIfNest2 nest2;
    }

    [System.Serializable]
    public class HideIfNest2
    {
        public bool hide1;
        public bool hide2;
        public HideIfEnum enum1;
        [NaEnumFlags] public HideIfEnumFlag enum2;
        public bool GetHide1() { return hide1; }
        public bool GetHide2() { return hide2; }
        public HideIfEnum GetEnum1() { return enum1; }
        public HideIfEnumFlag GetEnum2() { return enum2; }

        [NaHideIf(EConditionOperator.And, "GetHide1", "GetHide2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfAll = new Vector2(0.25f, 0.75f);

        [NaHideIf(EConditionOperator.Or, "GetHide1", "GetHide2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfAny = new Vector2(0.25f, 0.75f);

        [NaHideIf("GetEnum1", HideIfEnum.Case2)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfEnum = new Vector2(0.25f, 0.75f);

        [NaHideIf("GetEnum2", HideIfEnumFlag.Flag0)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfEnumFlag;

        [NaHideIf("GetEnum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfEnumFlagMulti;
    }

    public enum HideIfEnum
    {
        Case0,
        Case1,
        Case2
    }

    [Flags]
    public enum HideIfEnumFlag
    {
        Flag0 = 1,
        Flag1 = 2,
        Flag2 = 4,
        Flag3 = 8
    }
}
