// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using EFramework.Common;
using UnityEngine;

namespace EFramework.Fx {
    /// <summary>
    /// Setup music parameters on start.
    /// </summary>
    public sealed class MusicOnStart : MonoBehaviour {
        [SerializeField]
        string _music;

        [SerializeField]
        bool _isLooped = true;

        IEnumerator Start () {
            yield return null;
            var sm = Service<SoundManager>.Get ();
            if (sm.MusicVolume <= 0f) {
                sm.StopMusic ();
            }
            sm.PlayMusic (_music, _isLooped);
        }
    }
}