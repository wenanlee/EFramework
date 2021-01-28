// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace EFramework.Collections {
    /// <summary>
    /// List class replacement without additional checks and public access to internal array data.
    /// </summary>
    [Serializable]
    public class FastList<T> : IList<T> {
        /// <summary>
        /// Get items count.
        /// </summary>
        public int Count { get { return _count; } }

        /// <summary>
        /// Get collection capacity.
        /// </summary>
        public int Capacity { get { return _capacity; } }

        /// <summary>
        /// Get / set item at specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public T this [int index] {
            get {
                if (index >= _count) {
                    throw new ArgumentOutOfRangeException ();
                }
                return _items[index];
            }
            set {
                if (index >= _count) {
                    throw new ArgumentOutOfRangeException ();
                }
                _items[index] = value;
            }
        }

        const int InitCapacity = 8;

        readonly bool _isNullable;

        T[] _items;

        int _count;

        int _capacity;

        readonly EqualityComparer<T> _comparer;

        bool _useObjectCastComparer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FastList () : this (null) { }

        /// <summary>
        /// Constructor with comparer initialization.
        /// </summary>
        /// <param name="comparer">Comparer. If null - default comparer will be used.</param>
        public FastList (EqualityComparer<T> comparer) : this (InitCapacity, comparer) { }

        /// <summary>
        /// Constructor with capacity initialization.
        /// </summary>
        /// <param name="capacity">Capacity on start.</param>
        /// <param name="comparer">Comparer. If null - default comparer will be used.</param>
        public FastList (int capacity, EqualityComparer<T> comparer = null) {
            var type = typeof (T);
            _isNullable = !type.IsValueType || Nullable.GetUnderlyingType (type) != null;
            _capacity = capacity > InitCapacity ? capacity : InitCapacity;
            _count = 0;
            _comparer = comparer;
            _items = new T[_capacity];
        }

        /// <summary>
        /// Add new item to end of collection.
        /// </summary>
        /// <param name="item">New item.</param>
        public void Add (T item) {
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
        /// Add items to end of this collection.
        /// </summary>
        /// <param name="data">Data.</param>
        public void AddRange (IEnumerable<T> data) {
            if (data == null) {
                throw new ArgumentNullException ("data");
            }
            var casted = data as ICollection<T>;

            if (casted != null) {
                var amount = casted.Count;

                if (amount <= 0) {
                    return;
                }
                Reserve (amount, false, false);
                casted.CopyTo (_items, _count);
                _count += amount;
            } else {
                using (var it = data.GetEnumerator ()) {
                    while (it.MoveNext ()) {
                        Add (it.Current);
                    }
                }
            }
        }

        /// <summary>
        /// Set internal data, use it on your own risk!
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="count">Count.</param>
        public void AssignData (T[] data, int count) {
            if (data == null) {
                throw new ArgumentNullException ("data");
            }
            _items = data;
            _count = count >= 0 ? count : 0;
            _capacity = _items.Length;
        }

        /// <summary>
        /// Clear collection without release memory for performance optimization.
        /// </summary>
        public void Clear () {
            Clear (false);
        }

        /// <summary>
        /// Clear collection without release memory for performance optimization.
        /// </summary>
        /// <param name="forceSetDefaultValues">Is new items should be set to their default values.
        /// Ignored (set to true) for reference types.</param>
        public void Clear (bool forceSetDefaultValues) {
            if (_isNullable || forceSetDefaultValues) {
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
            return IndexOf (item) != -1;
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
        /// Add new items with default values to end of collection.
        /// </summary>
        /// <param name="amount">Amount of new items.</param>
        /// <param name="clearCollection">Is collection should be cleared before.</param>
        /// <param name="forceSetDefaultValues">Is new items should be set to their default values (False useful
        /// for optimization).</param>
        public void FillWithEmpty (int amount, bool clearCollection = false, bool forceSetDefaultValues = true) {
            if (amount <= 0) {
                return;
            }

            if (clearCollection) {
                _count = 0;
            }
            Reserve (amount, clearCollection, forceSetDefaultValues);
            _count += amount;
        }

        /// <summary>
        /// Get index of specified item.
        /// </summary>
        /// <returns>Found index or -1.</returns>
        /// <param name="item">Item to check.</param>
        public int IndexOf (T item) {
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
            return i;
        }

        /// <summary>
        /// Insert new item at specified position of collection.
        /// </summary>
        /// <param name="index">Position.</param>
        /// <param name="item">New item.</param>
        public void Insert (int index, T item) {
            if (index < 0 || index > _count) {
                throw new ArgumentOutOfRangeException ();
            }
            Reserve (1, false, false);
            Array.Copy (_items, index, _items, index + 1, _count - index);
            _items[index] = item;
            _count++;
        }

        /// <summary>
        /// Is collection readonly (for compatibility to IList).
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Get internal data, use it on your own risk!
        /// Dont forget, length of result array equals Capacity, not Count!
        /// Can be used for external implementation any other methods.
        /// </summary>
        public T[] GetData () {
            return _items;
        }

        /// <summary>
        /// Get internal data, use it on your own risk!
        /// Dont forget, length of result array equals Capacity, not Count!
        /// Can be used for external implementation any other methods.
        /// </summary>
        /// <param name="count">Actual count of items.</param>
        public T[] GetData (out int count) {
            count = _count;
            return _items;
        }

        /// <summary>
        /// Never ever - use for-loop for iterations!
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator () {
            throw new NotSupportedException ();
        }

        IEnumerator IEnumerable.GetEnumerator () {
            throw new NotSupportedException ();
        }

        /// <summary>
        /// Try to remove specified item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public bool Remove (T item) {
            var id = IndexOf (item);
            if (id == -1) {
                return false;
            }
            RemoveAt (id);

            return true;
        }

        /// <summary>
        /// Remove item from collection at index.
        /// </summary>
        /// <param name="id">Index of item to remove.</param>
        public void RemoveAt (int id) {
            if (id < 0 || id >= _count) {
                return;
            }
            _count--;
            Array.Copy (_items, id + 1, _items, id, _count - id);
        }

        /// <summary>
        /// Try to remove last item in collection.
        /// </summary>
        /// <returns><c>true</c>, if last was removed, <c>false</c> otherwise.</returns>
        /// <param name="forceSetDefaultValues">Is new items should be set to their default values (False useful for
        /// optimization).</param>
        public bool RemoveLast (bool forceSetDefaultValues = true) {
            if (_count <= 0) {
                return false;
            }
            _count--;
            if (forceSetDefaultValues) {
                _items[_count] = default (T);
            }

            return true;
        }

        /// <summary>
        /// Reserve the specified amount of items, absolute or relative. Items amount not changed!
        /// </summary>
        /// <param name="amount">Amount.</param>
        /// <param name="totalAmount">Is amount value means - total items amount at collection or relative
        /// otherwise.</param>
        /// <param name="forceSetDefaultValues">Is new items should be set to their default values (False useful for
        /// optimization).</param>
        public void Reserve (int amount, bool totalAmount = false, bool forceSetDefaultValues = true) {
            if (amount <= 0) {
                return;
            }
            var start = totalAmount ? 0 : _count;
            var newCount = start + amount;

            if (newCount > _capacity) {
                if (_capacity <= 0) {
                    _capacity = InitCapacity;
                }
                while (_capacity < newCount) {
                    _capacity <<= 1;
                }
                var items = new T[_capacity];

                Array.Copy (_items, items, _count);
                _items = items;
            }
            if (forceSetDefaultValues) {
                for (var i = _count; i < newCount; i++) {
                    _items[i] = default (T);
                }
            }
        }

        /// <summary>
        /// Reverse items order in collection.
        /// </summary>
        public void Reverse () {
            if (_count <= 0) {
                return;
            }

            T temp;

            for (int i = 0, iMax = _count >> 1; i < iMax; i++) {
                temp = _items[i];
                _items[i] = _items[_count - i - 1];
                _items[_count - i - 1] = temp;
            }
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
        /// Set usage state of special (fastest) inlined comparer for nullable types in Contains method.
        /// Useful for MonoBehaviour-inherited classes.
        /// </summary>
        /// <param name="state">New state of usage.</param>
        public void UseCastToObjectComparer (bool state) {
            _useObjectCastComparer = state;
        }
    }
}