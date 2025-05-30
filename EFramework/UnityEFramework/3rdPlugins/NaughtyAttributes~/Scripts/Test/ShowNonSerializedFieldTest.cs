using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowNonSerializedFieldTest : MonoBehaviour
    {
#pragma warning disable 414
        [NaShowNonSerializedField]
        private ushort myUShort = ushort.MaxValue;

        [NaShowNonSerializedField]
        private short myShort = short.MaxValue;

        [NaShowNonSerializedField]
        private uint myUInt = uint.MaxValue;

        [NaShowNonSerializedField]
        private int myInt = 10;

        [NaShowNonSerializedField]
        private ulong myULong = ulong.MaxValue;

        [NaShowNonSerializedField]
        private long myLong = long.MaxValue;

        [NaShowNonSerializedField]
        private const float PI = 3.14159f;

        [NaShowNonSerializedField]
        private static readonly Vector3 CONST_VECTOR = Vector3.one;
#pragma warning restore 414
    }
}
