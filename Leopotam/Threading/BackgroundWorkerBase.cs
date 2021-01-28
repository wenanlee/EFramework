// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

#if !UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.Threading;
using EFramework.Common;
using UnityEngine;

namespace EFramework.Threading {
    /// <summary>
    /// Base class for processing data at background thread, singleton based.
    /// Base - final inherited type.
    /// T - Parameter type for exchange data between main and background threads.
    /// </summary>
    public abstract class BackgroundWorkerBase<Base, T> : MonoBehaviourService<Base> where Base : class {
        /// <summary>
        /// Is background thread started and inited.
        /// </summary>
        public bool IsWorkerStarted {
            get {
                lock (_inSyncObj) { return _isWorkerStarted; }
            }
        }

        /// <summary>
        /// Dont sleep after processing item in background thread.
        /// Processing speed can increase, but owerall performance can degrade.
        /// Disable by default.
        /// </summary>
        protected bool DontSleepAfterItemProcess {
            get {
                lock (_inSyncObj) { return _dontSleepAfterItemProcess; }
            }
            set {
                lock (_inSyncObj) { _dontSleepAfterItemProcess = value; }
            }
        }

        /// <summary>
        /// Length of input data queue.
        /// </summary>
        protected int InputQueueLength {
            get {
                lock (_inSyncObj) { return _inQueue.Count; }
            }
        }

        /// <summary>
        /// Length of output data queue.
        /// </summary>
        protected int OutputQueueLength {
            get {
                lock (_outSyncObj) { return _outQueue.Count; }
            }
        }

        bool _isWorkerStarted;

        bool _dontSleepAfterItemProcess;

        readonly object _inSyncObj = new object ();

        readonly object _outSyncObj = new object ();

        readonly List<T> _inQueue = new List<T> (64);

        readonly List<T> _outQueue = new List<T> (64);

        Thread _thread;

        /// <summary>
        /// Amount of items to process as result from background worker. Negative / zero values means - all items.
        /// By default, 1 item will be processed.
        /// </summary>
        protected int ItemsAmountToProcessAtForground;

        protected override void OnCreateService () {
            ItemsAmountToProcessAtForground = 1;
            _thread = new Thread (OnBackgroundThreadProc);
            _thread.Start ();
        }

        protected override void OnDestroyService () {
            try {
                if (_thread != null) {
                    _thread.Interrupt ();
                    _thread.Join (100);
                }
            } catch (Exception ex) { Debug.LogError (ex); }
            _thread = null;
        }

        protected virtual void Update () {
            OnWorkerProcessOutQueueAtForeground ();
        }

        protected virtual void OnEnqueueItemAtForeground (T item) {
            _inQueue.Add (item);
        }

        protected virtual void OnClearItemAtForeground (T item) { }

        /// <summary>
        /// Method for custom reaction on thread start. Important - will be called at background thread!
        /// </summary>
        protected virtual void OnWorkerStartInBackground () { }

        /// <summary>
        /// Method for custom reaction on thread stop. Important - will be called at background thread!
        /// </summary>
        protected virtual void OnWorkerStopInBackground () { }

        /// <summary>
        /// Method for processing item. Important - will be called at background thread!
        /// </summary>
        /// <param name="item">Item for processing.</param>
        /// <returns>Result of processing.</returns>
        protected abstract T OnWorkerTickInBackground (T item);

        /// <summary>
        /// Method for custom reaction on receiving result of background processing.
        /// </summary>
        /// <param name="result">Result of processing</param>
        protected abstract void OnResultFromWorker (T result);

        /// <summary>
        /// Method for run processing outQueue. Important - should be called in unity thread!
        /// </summary>
        protected void OnWorkerProcessOutQueueAtForeground () {
            int count;
            lock (_outSyncObj) {
                count = _outQueue.Count;
            }
            if (count > 0) {
                var maxAmount = ItemsAmountToProcessAtForground;
                if (maxAmount > 0) {
                    count = count < maxAmount ? count : maxAmount;
                }
                T result;
                for (var i = 0; i < count; i++) {
                    lock (_outSyncObj) {
                        result = _outQueue[0];
                        _outQueue.RemoveAt (0);
                    }
                    OnResultFromWorker (result);
                }
            }
        }

        void OnBackgroundThreadProc () {
            lock (_inSyncObj) {
                _isWorkerStarted = true;
            }
            try {
                OnWorkerStartInBackground ();
                var dontSleep = false;
                T item = default (T);
                bool isFound;
                while (Thread.CurrentThread.IsAlive) {
                    lock (_inSyncObj) {
                        dontSleep = _dontSleepAfterItemProcess;
                        isFound = _inQueue.Count > 0;
                        if (isFound) {
                            item = _inQueue[0];
                            _inQueue.RemoveAt (0);
                        }
                    }
                    if (isFound) {
                        var result = OnWorkerTickInBackground (item);
                        lock (_outSyncObj) {
                            _outQueue.Add (result);
                        }
                    }
                    if (!isFound || !dontSleep) {
                        Thread.Sleep (1);
                    }
                }
            } catch { }
            lock (_inSyncObj) {
                _isWorkerStarted = false;
                _inQueue.Clear ();
            }
            lock (_outSyncObj) {
                _outQueue.Clear ();
            }
            OnWorkerStopInBackground ();
        }

        public void ClearInputQueue () {
            lock (_inSyncObj) {
                for (var i = _inQueue.Count - 1; i >= 0; i--) {
                    OnClearItemAtForeground (_inQueue[i]);
                }
                _inQueue.Clear ();
            }
        }

        public bool EnqueueItem (T item) {
            lock (_inSyncObj) {
                if (!_isWorkerStarted) {
#if UNITY_EDITOR
                    Debug.LogWarning ("Worker not started");
#endif
                    return false;
                }
                OnEnqueueItemAtForeground (item);
            }
            return true;
        }
    }
}
#endif