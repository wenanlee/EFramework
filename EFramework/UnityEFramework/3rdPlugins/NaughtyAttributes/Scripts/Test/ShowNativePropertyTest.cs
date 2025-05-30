using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowNativePropertyTest : MonoBehaviour
    {
        [NaShowNativeProperty]
        private Transform Transform
        {
            get
            {
                return transform;
            }
        }

        [NaShowNativeProperty]
        private Transform ParentTransform
        {
            get
            {
                return transform.parent;
            }
        }

        [NaShowNativeProperty]
        private ushort MyUShort
        {
            get
            {
                return ushort.MaxValue;
            }
        }

        [NaShowNativeProperty]
        private short MyShort
        {
            get
            {
                return short.MaxValue;
            }
        }

        [NaShowNativeProperty]
        private ulong MyULong
        {
            get
            {
                return ulong.MaxValue;
            }
        }

        [NaShowNativeProperty]
        private long MyLong
        {
            get
            {
                return long.MaxValue;
            }
        }

        [NaShowNativeProperty]
        private uint MyUInt
        {
            get
            {
                return uint.MaxValue;
            }
        }

        [NaShowNativeProperty]
        private int MyInt
        {
            get
            {
                return int.MaxValue;
            }
        }
    }
}
