using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ReorderableListTest : MonoBehaviour
    {
        [NaReorderableList]
        public int[] intArray;

        [NaReorderableList]
        public List<Vector3> vectorList;

        [NaReorderableList]
        public List<SomeStruct> structList;

        [NaReorderableList]
        public GameObject[] gameObjectsList;

        [NaReorderableList]
        public List<Transform> transformsList;

        [NaReorderableList]
        public List<MonoBehaviour> monoBehavioursList;
    }

    [System.Serializable]
    public struct SomeStruct
    {
        public int Int;
        public float Float;
        public Vector3 Vector;
    }
}
