// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace EFramework.Events {
    /// <summary>
    /// Behaviour tree node execution result.
    /// </summary>
    public enum BehaviourTreeResult {
        Success,
        Fail,
        Pending
    }

    /// <summary>
    /// Behaviour tree abstract node base.
    /// </summary>
    public abstract class BehaviourTreeNodeBase {
        /// <summary>
        /// Process node logic.
        /// </summary>
        public abstract BehaviourTreeResult Process ();
    }

    /// <summary>
    /// Behaviour tree abstract container base.
    /// </summary>
    public abstract class BehaviourTreeContainerBase : BehaviourTreeNodeBase {
        protected readonly List<BehaviourTreeNodeBase> Children = new List<BehaviourTreeNodeBase> ();

        /// <summary>
        /// Helper for add new BehaviourTreeAction node.
        /// </summary>
        /// <param name="bt">Bt.</param>
        /// <param name="cb">Cb.</param>
        public BehaviourTreeContainerBase Then<T> (
            BehaviourTree<T> bt, Func<BehaviourTree<T>, BehaviourTreeResult> cb) where T : class, new () {
            Then (new BehaviourTreeAction<T> (bt, cb));
            return this;
        }

        /// <summary>
        /// Helper for add new BehaviourTreeAction node.
        /// </summary>
        /// <param name="node">Node.</param>
        public BehaviourTreeContainerBase Then (BehaviourTreeNodeBase node) {
            AddChild (node);
            return this;
        }

        /// <summary>
        /// Helper for add new BehaviourTreeSequence node.
        /// </summary>
        public BehaviourTreeContainerBase Sequence () {
            var node = new BehaviourTreeSequence ();
            AddChild (node);
            return node;
        }

        /// <summary>
        /// Helper for add new BehaviourTreeParallel node.
        /// </summary>
        public BehaviourTreeContainerBase Parallel () {
            var node = new BehaviourTreeParallel ();
            AddChild (node);
            return node;
        }

        /// <summary>
        /// Helper for add new BehaviourTreeSelector node.
        /// </summary>
        public BehaviourTreeContainerBase Select () {
            var node = new BehaviourTreeSelector ();
            AddChild (node);
            return node;
        }

        /// <summary>
        /// Helper for add new BehaviourTreeCondition node.
        /// </summary>
        /// <param name="bt">Bt.</param>
        /// <param name="condition">Condition.</param>
        public BehaviourTreeCondition<T> When<T> (
            BehaviourTree<T> bt, Func<BehaviourTree<T>, BehaviourTreeResult> condition) where T : class, new () {
            var node = new BehaviourTreeCondition<T> (bt, condition);
            AddChild (node);
            return node;
        }

        /// <summary>
        /// Helper for add new BehaviourTreeCondition node.
        /// </summary>
        /// <param name="condition">Condition.</param>
        public BehaviourTreeCondition<T> When<T> (BehaviourTreeNodeBase condition) where T : class, new () {
            var node = new BehaviourTreeCondition<T> (condition);
            AddChild (node);
            return node;
        }

        /// <summary>
        /// Adds child node to container.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="node">Node.</param>
        public BehaviourTreeContainerBase AddChild (BehaviourTreeNodeBase node) {
            if (node != null) {
                Children.Add (node);
            }
            return this;
        }
    }

    /// <summary>
    /// Behaviour tree action base node.
    /// </summary>
    public abstract class BehaviourTreeActionBase<T> : BehaviourTreeNodeBase where T : class, new () {
        protected readonly BehaviourTree<T> Tree;

        /// <summary>
        /// Initialize new instance of BehaviourTreeAction node.
        /// </summary>
        /// <param name="bt">BehaviourTree instance.</param>
        protected BehaviourTreeActionBase (BehaviourTree<T> bt) {
            if (bt == null) {
                throw new ArgumentNullException ();
            }
            Tree = bt;
        }
    }

    /// <summary>
    /// Behaviour tree action.
    /// </summary>
    public sealed class BehaviourTreeAction<T> : BehaviourTreeActionBase<T> where T : class, new () {
        readonly Func<BehaviourTree<T>, BehaviourTreeResult> _cb;

        /// <summary>
        /// Initialize new instance of BehaviourTreeAction node.
        /// </summary>
        /// <param name="bt">BehaviourTree instance.</param>
        /// <param name="cb">Callback of custom node logic.</param>
        public BehaviourTreeAction (BehaviourTree<T> bt, Func<BehaviourTree<T>, BehaviourTreeResult> cb) : base (bt) {
            if (cb == null) {
                throw new ArgumentNullException ();
            }
            _cb = cb;
        }

        /// <summary>
        /// Process node logic.
        /// </summary>
        public override BehaviourTreeResult Process () {
            return _cb (Tree);
        }
    }

    /// <summary>
    /// Behaviour tree sequence container.
    /// </summary>
    public sealed class BehaviourTreeSequence : BehaviourTreeContainerBase {
        int _currentChild;

        /// <summary>
        /// Process node logic.
        /// </summary>
        public override BehaviourTreeResult Process () {
            var res = BehaviourTreeResult.Success;
            for (int i = _currentChild, iMax = Children.Count; i < iMax; i++, _currentChild++) {
                res = Children[i].Process ();
                if (res != BehaviourTreeResult.Success) {
                    break;
                }
            }
            if (res != BehaviourTreeResult.Pending) {
                _currentChild = 0;
            }
            return res;
        }
    }

    /// <summary>
    /// Behaviour tree parallel container.
    /// </summary>
    public sealed class BehaviourTreeParallel : BehaviourTreeContainerBase {
        /// <summary>
        /// Process node logic.
        /// </summary>
        public override BehaviourTreeResult Process () {
            var isPending = false;
            for (int i = 0, iMax = Children.Count; i < iMax; i++) {
                if (Children[i].Process () == BehaviourTreeResult.Pending) {
                    isPending = true;
                }
            }
            return isPending ? BehaviourTreeResult.Pending : BehaviourTreeResult.Success;
        }
    }

    /// <summary>
    /// Behaviour tree selector container.
    /// </summary>
    public sealed class BehaviourTreeSelector : BehaviourTreeContainerBase {
        int _currentChild;

        /// <summary>
        /// Process node logic.
        /// </summary>
        public override BehaviourTreeResult Process () {
            var res = BehaviourTreeResult.Success;
            for (int i = _currentChild, iMax = Children.Count; i < iMax; i++, _currentChild++) {
                res = Children[i].Process ();
                if (res != BehaviourTreeResult.Fail) {
                    break;
                }
            }
            if (res != BehaviourTreeResult.Pending) {
                _currentChild = 0;
            }
            return res;
        }
    }

    /// <summary>
    /// Behaviour tree condition node.
    /// </summary>
    public sealed class BehaviourTreeCondition<T> : BehaviourTreeNodeBase where T : class, new () {
        readonly BehaviourTreeNodeBase _condition;

        BehaviourTreeNodeBase _node;

        /// <summary>
        /// Initialize new instance of BehaviourTreeCondition node.
        /// </summary>
        /// <param name="bt">BehaviourTree instance.</param>
        /// <param name="condition">Callback of custom node logic for condition checking.</param>
        public BehaviourTreeCondition (BehaviourTree<T> bt, Func<BehaviourTree<T>, BehaviourTreeResult> condition):
            this (new BehaviourTreeAction<T> (bt, condition)) { }

        /// <summary>
        /// Initialize new instance of BehaviourTreeCondition node.
        /// </summary>
        /// <param name="condition">Condition node.</param>
        /// <param name="successNode">Node for processing on successful condition.</param>
        public BehaviourTreeCondition (BehaviourTreeNodeBase condition, BehaviourTreeNodeBase successNode = null) {
            if (condition == null) {
                throw new ArgumentNullException ();
            }
            _condition = condition;
            _node = successNode;
        }

        /// <summary>
        /// Set node for processing on successful condition.
        /// </summary>
        /// <param name="bt">BehaviourTree instance.</param>
        /// <param name="cb">Callback of custom node logic.</param>
        public BehaviourTreeCondition<T> Then (BehaviourTree<T> bt, Func<BehaviourTree<T>, BehaviourTreeResult> cb) {
            return Then (new BehaviourTreeAction<T> (bt, cb));
        }

        /// <summary>
        /// Set node for processing on successful condition.
        /// </summary>
        /// <param name="successNode">Node for processing on successful condition.</param>
        public BehaviourTreeCondition<T> Then (BehaviourTreeNodeBase successNode) {
            _node = successNode;
            return this;
        }

        /// <summary>
        /// Process node logic.
        /// </summary>
        public override BehaviourTreeResult Process () {
            var res = _condition.Process ();
            if (res == BehaviourTreeResult.Success && _node != null) {
                return _node.Process ();
            }
            return res;
        }
    }
}