using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EFramework.Unity.UIFramework
{
    /// <summary>
    /// ≤‚ ‘UI
    /// </summary>
    public class UITest : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                UIManager.Instance.ShowUI(1001);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                UIManager.Instance.ShowUI(1005);
            }
        }
    }
}
