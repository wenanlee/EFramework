using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFramework.Unity.Pool
{
    public interface IObjPool
    {
        void OnGet();
        void OnRelease();
    }
}
