using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ResizableTextAreaTest : MonoBehaviour
    {
        [NaResizableTextArea]
        public string text0;

        public ResizableTextAreaNest1 nest1;
    }

    [System.Serializable]
    public class ResizableTextAreaNest1
    {
        [NaResizableTextArea]
        public string text1;

        public ResizableTextAreaNest2 nest2;
    }

    [System.Serializable]
    public class ResizableTextAreaNest2
    {
        [NaResizableTextArea]
        public string text2;
    }
}
