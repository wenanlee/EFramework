// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    /// <summary>
    /// Post solution generation attribute. Methods with PostSolutionGeneration attribute will be executed after C# project recompile.
    /// </summary>
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class PostSolutionGenerationAttribute : Attribute {
        class PostSolutionGenerationProcessor : AssetPostprocessor {
            static void OnGeneratedCSProjectFiles () {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ()) {
                    foreach (var type in assembly.GetTypes ()) {
                        foreach (var method in type.GetMethods (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                            var attrs = method.GetCustomAttributes (typeof (PostSolutionGenerationAttribute), false);
                            if (attrs.Length > 0) {
                                try {
                                    method.Invoke (null, null);
                                } catch (Exception ex) {
                                    Debug.LogError (ex);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}