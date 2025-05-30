using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class SceneTest : MonoBehaviour
    {
        [NaScene]
        public string scene0;

        public SceneNest1 nest1;
    }

    [System.Serializable]
    public class SceneNest1
    {
        [NaScene]
        public string scene1;

        public SceneNest2 nest2;
    }

    [System.Serializable]
    public struct SceneNest2
    {
        [NaScene]
        public int scene2;
    }
}
