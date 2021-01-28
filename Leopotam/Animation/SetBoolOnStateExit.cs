// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace EFramework.Animation {
    /// <summary>
    /// Set Animator bool parameter to new state on node exit.
    /// </summary>
    public sealed class SetBoolOnStateExit : StateMachineBehaviour {
        [SerializeField]
        string _boolName;

        [SerializeField]
        bool _boolValue;

        int _fieldHash = -1;

        public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateExit (animator, stateInfo, layerIndex);
            if (_fieldHash == -1) {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty (_boolName)) {
                    Debug.LogWarning ("Bool field name is empty", animator);
                    return;
                }
#endif
                _fieldHash = Animator.StringToHash (_boolName);
            }
            animator.SetBool (_fieldHash, _boolValue);
        }
    }
}