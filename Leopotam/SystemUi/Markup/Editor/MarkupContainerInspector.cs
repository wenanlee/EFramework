// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace EFramework.SystemUi.Markup.UnityEditors {
    [CanEditMultipleObjects]
    [CustomEditor (typeof (MarkupContainer), true)]
    sealed class MarkupContainerInspector : Editor {
        public override void OnInspectorGUI () {
            DrawDefaultInspector ();

            if (!Application.isPlaying) {
                MarkupContainer container;
                if (GUILayout.Button ("Remove visuals")) {
                    foreach (var item in targets) {
                        container = item as MarkupContainer;
                        container.Clear ();
                    }
                }
                if (GUILayout.Button ("Create visuals")) {
                    foreach (var item in targets) {
                        container = item as MarkupContainer;
                        container.Clear ();
                        container.CreateVisuals ();
                    }
                }
            }
        }
    }
}