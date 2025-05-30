using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class InfoBoxTest : MonoBehaviour
    {
        [NaInfoBox("Normal", ENaInfoBoxType.Normal)]
        public int normal;

        public InfoBoxNest1 nest1;
    }

    [System.Serializable]
    public class InfoBoxNest1
    {
        [NaInfoBox("Warning", ENaInfoBoxType.Warning)]
        public int warning;

        public InfoBoxNest2 nest2;
    }

    [System.Serializable]
    public class InfoBoxNest2
    {
        [NaInfoBox("Error", ENaInfoBoxType.Error)]
        public int error;
    }
}
