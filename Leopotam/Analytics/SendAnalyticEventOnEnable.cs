// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Common;
using UnityEngine;

namespace EFramework.Analytics {
    /// <summary>
    /// Send analytic event on enable.
    /// </summary>
    public sealed class SendAnalyticEventOnEnable : MonoBehaviour {
        [SerializeField]
        string _category = "Category";

        [SerializeField]
        string _event = "Event";

        void OnEnable () {
            if (!string.IsNullOrEmpty (_category) && !string.IsNullOrEmpty (_event)) {
                Service<GoogleAnalyticsManager>.Get ().TrackEvent (_category, _event);
            }
        }
    }
}