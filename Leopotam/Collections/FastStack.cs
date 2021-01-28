// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace EFramework.Collections {
    /// <summary>
    /// Stack class replacement with custom EqualityComparer and fastest comparation with direct cast to "System.Object"
    /// (useful for MonoBehaviour-inherited classes).
    /// </summary>
    public class FastStack<T> {
        const int InitCapacity = 8;

        T[] _items;

        int _capacity;

        int _count;

        readonly bool _isNullable;

        readonly EqualityComparer<T> _comparer;

        bool _useObjectCastComparer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FastStack () : this (null) { }

        /// <summary>
        /// Constructor with comparer initialization.
        /// </summary>
        /// <param name="comparer">Comparer. If null - default comparer will be used.</param>
        public FastStack (EqualityComparer<T> comparer) : this (InitCapacity, comparer) { }

        /// <summary>
        /// Constructor with capacity and comparer initialization.
        /// </summary>
        /// <param name="capacity">Capacity on start.</param>
        /// <param name="comparer">Comparer. If null - default comparer will be used.</param>
        public FastStack (int capacity, EqualityComparer<T> comparer = null) {
            var type = typeof (T);
            _isNullable = !type.IsValueType || (Nullable.GetUnderlyingType (type) != null);
            _capacity = capacity > InitCapacity ? capacity : InitCapacity;
            _count = 0;
            _comparer = comparer;
            _items = new T[_capacity];
        }

        /// <summary>
        /// Get items count.
        /// </summary>
        public int Count { get { return _count; } }

        /// <summary>
        /// Clear collection without release memory for performance optimization.
        /// </summary>
        public void Clear () {
            if (_isNullable) {
                for (var i = _count - 1; i >= 0; i--) {
                    _items[i] = default (T);
                }
            }
            _count = 0;
        }

        /// <summary>
        /// Is collection contains specified item.
        /// </summary>
        /// <param name="item">Item to check.</param>
        public bool Contains (T item) {
            int i;
            if (_useObjectCastComparer && _isNullable) {
                for (i = _count - 1; i >= 0; i--) {
                    if ((object) _items[i] == (object) item) {
                        break;
                    }
                }
            } else {
                if (_comparer != null) {
                    for (i = _count - 1; i >= 0; i--) {
                        if (_comparer.Equals (_items[i], item)) {
                            break;
                        }
                    }
                } else {
                    i = Array.IndexOf (_items, item, 0, _count);
                }
            }
            return i != -1;
        }

        /// <summary>
        /// Copy collection to array and insert from specified index.
        /// </summary>
        /// <param name="array">Target array.</param>
        /// <param name="arrayIndex">Start index at target array.</param>
        public void CopyTo (T[] array, int arrayIndex) {
            Array.Copy (_items, 0, array, arrayIndex, _count);
        }

        /// <summary>
        /// Get top item without remove from stack.
        /// </summary>
        public T Peek () {
            if (_count == 0) {
                throw new IndexOutOfRangeException ();
            }
            return _items[_count - 1];
        }

        /// <summary>
        /// Get top item with remove from stack.
        /// </summary>
        public T Pop () {
            if (_count == 0) {
                throw new IndexOutOfRangeException ();
            }
            _count--;
            T target = _items[_count];
            if (_isNullable) {
                _items[_count] = default (T);
            }
            return target;
        }

        /// <summary>
        /// Add new item to stack.
        /// </summary>
        /// <param name="item">New item.</param>
        public void Push (T item) {
            if (_count == _capacity) {
                if (_capacity > 0) {
                    _capacity <<= 1;
                } else {
                    _capacity = InitCapacity;
                }
                var items = new T[_capacity];

                Array.Copy (_items, items, _count);
                _items = items;
            }
            _items[_count] = item;
            _count++;
        }

        /// <summary>
        /// Copy collection items to array.
        /// </summary>
        public T[] ToArray () {
            var target = new T[_count];

            if (_count > 0) {
                Array.Copy (_items, target, _count);
            }

            return target;
        }

        /// <summary>
        /// Set capacity to the actual number of elements.
        /// </summary>
        public void TrimExcess () {
            throw new NotSupportedException ();
        }

        /// <summary>
        /// Set usage state of special (fastest) inlined comparer for nullable types in Contains method.
        /// Useful for MonoBehaviour-inherited classes.
        /// </summary>
        /// <param name="state">New state of usage.</param>
        public void UseCastToObjectComparer (bool state) {
            _useObjectCastComparer = state;
        }
    }
}