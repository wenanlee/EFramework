// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using EFramework.Common;
using UnityEngine;

namespace EFramework.Fx {
    /// <summary>
    /// Setup FX parameters on enable.
    /// </summary>
    public sealed class SoundOnEnable : MonoBehaviour {
        [SerializeField]
        AudioClip _sound;

        [SerializeField]
        SoundFxChannel _channel = SoundFxChannel.First;

        /// <summary>
        /// Should new FX force interrupts FX at channel or not.
        /// </summary>
        [SerializeField]
        bool _isInterrupt;

        void OnEnable () {
            Service<SoundManager>.Get ().PlayFx (_sound, _channel, _isInterrupt);
        }
    }
}