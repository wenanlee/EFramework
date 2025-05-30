using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowIfTest : MonoBehaviour
    {
        public bool show1;
        public bool show2;
        public ShowIfEnum enum1;
        [NaEnumFlags] public ShowIfEnumFlag enum2;

        [NaShowIf(EConditionOperator.And, "show1", "show2")]
        [NaReorderableList]
        public int[] showIfAll;

        [NaShowIf(EConditionOperator.Or, "show1", "show2")]
        [NaReorderableList]
        public int[] showIfAny;

        [NaShowIf("enum1", ShowIfEnum.Case0)]
        [NaReorderableList]
        public int[] showIfEnum;

        [NaShowIf("enum2", ShowIfEnumFlag.Flag0)]
        [NaReorderableList]
        public int[] showIfEnumFlag;

        [NaShowIf("enum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)]
        [NaReorderableList]
        public int[] showIfEnumFlagMulti;

        public ShowIfNest1 nest1;
    }

    [System.Serializable]
    public class ShowIfNest1
    {
        public bool show1;
        public bool show2;
        public ShowIfEnum enum1;
        [NaEnumFlags] public ShowIfEnumFlag enum2;
        public bool Show1 { get { return show1; } }
        public bool Show2 { get { return show2; } }
        public ShowIfEnum Enum1 { get { return enum1; } }
        public ShowIfEnumFlag Enum2 { get { return enum2; } }

        [NaShowIf(EConditionOperator.And, "Show1", "Show2")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int showIfAll;

        [NaShowIf(EConditionOperator.Or, "Show1", "Show2")]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int showIfAny;

        [NaShowIf("Enum1", ShowIfEnum.Case1)]
        [NaAllowNesting] // Because it's nested we need to explicitly allow nesting
        public int showIfEnum;

        [NaShowIf("Enum2", ShowIfEnumFlag.Flag0)]
        [NaAllowNesting]
        public int showIfEnumFlag;

        [NaShowIf("Enum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)]
        [NaAllowNesting]
        public int showIfEnumFlagMulti;

        public ShowIfNest2 nest2;
    }

    [System.Serializable]
    public class ShowIfNest2
    {
        public bool show1;
        public bool show2;
        public ShowIfEnum enum1;
        [NaEnumFlags] public ShowIfEnumFlag enum2;
        public bool GetShow1() { return show1; }
        public bool GetShow2() { return show2; }
        public ShowIfEnum GetEnum1() { return enum1; }
        public ShowIfEnumFlag GetEnum2() { return enum2; }

        [NaShowIf(EConditionOperator.And, "GetShow1", "GetShow2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 showIfAll = new Vector2(0.25f, 0.75f);

        [NaShowIf(EConditionOperator.Or, "GetShow1", "GetShow2")]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 showIfAny = new Vector2(0.25f, 0.75f);

        [NaShowIf("GetEnum1", ShowIfEnum.Case2)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 showIfEnum = new Vector2(0.25f, 0.75f);

        [NaShowIf("GetEnum2", ShowIfEnumFlag.Flag0)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 showIfEnumFlag;

        [NaShowIf("GetEnum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)]
        [NaMinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 showIfEnumFlagMulti;
    }

    public enum ShowIfEnum
    {
        Case0,
        Case1,
        Case2
    }

    [Flags]
    public enum ShowIfEnumFlag
    {
        Flag0 = 1,
        Flag1 = 2,
        Flag2 = 4,
        Flag3 = 8
    }
}
