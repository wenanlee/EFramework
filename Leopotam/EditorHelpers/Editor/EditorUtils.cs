// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    /// <summary>
    /// For internal use.
    /// </summary>
    sealed class CustomEndNameAction : EndNameEditAction {
        public Action<string> Callback;

        public override void Action (int instanceId, string pathName, string resourceFile) {
            if (Callback != null) {
                Callback (pathName);
            }
        }
    }

    /// <summary>
    /// Editor utils.
    /// </summary>
    sealed class EditorUtils {
        /// <summary>
        /// Create asset with name editing behaviour.
        /// </summary>
        /// <param name="fileName">Asset filename with extension.</param>
        /// <param name="icon">Optional icon, can be null.</param>
        /// <param name="onSuccess">Callback on success, parameter - final name of asset.</param>
        public static void CreateAndRenameAsset (string fileName, Texture2D icon, Action<string> onSuccess) {
            if (string.IsNullOrEmpty (fileName)) {
                throw new ArgumentException ("fileName");
            }
            if (onSuccess == null) {
                throw new ArgumentException ("onSuccess");
            }
            var action = ScriptableObject.CreateInstance<CustomEndNameAction> ();
            action.Callback = onSuccess;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists (0, action, fileName, icon, null);
        }
    }
}