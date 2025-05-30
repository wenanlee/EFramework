using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowAssetPreviewTest : MonoBehaviour
    {
        [NaShowAssetPreview]
        public Sprite sprite0;

        [NaShowAssetPreview(96, 96)]
        public GameObject prefab0;

        public ShowAssetPreviewNest1 nest1;
    }

    [System.Serializable]
    public class ShowAssetPreviewNest1
    {
        [NaShowAssetPreview]
        public Sprite sprite1;

        [NaShowAssetPreview(96, 96)]
        public GameObject prefab1;

        public ShowAssetPreviewNest2 nest2;
    }

    [System.Serializable]
    public class ShowAssetPreviewNest2
    {
        [NaShowAssetPreview]
        public Sprite sprite2;

        [NaShowAssetPreview(96, 96)]
        public GameObject prefab2;
    }
}
