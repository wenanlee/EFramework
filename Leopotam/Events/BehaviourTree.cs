// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace EFramework.Events {
    /// <summary>
    /// Behaviour tree.
    /// </summary>
    public sealed class BehaviourTree<T> where T : class, new () {
        readonly BehaviourTreeSequence _root = new BehaviourTreeSequence ();

        readonly T _store;

        /// <summary>
        /// Initialize new instance BehaviourTree class with custom store logic.
        /// If logic instance  will be null - new store logic instance will be created.
        /// </summary>
        /// <param name="store">Store logic instance.</param>
        public BehaviourTree (T store = null) {
            _store = store ?? new T ();
        }

        /// <summary>
        /// Gets root node of graph.
        /// </summary>
        /// <returns>The root node.</returns>
        public BehaviourTreeContainerBase GetRootNode () {
            return _root;
        }

        /// <summary>
        /// Gets store logic instance.
        /// </summary>
        /// <returns>The store.</returns>
        public T GetStore () {
            return _store;
        }

        /// <summary>
        /// Process logic of behaviour tree graph.
        /// </summary>
        public BehaviourTreeResult Process () {
            return _root.Process ();
        }
    }
}