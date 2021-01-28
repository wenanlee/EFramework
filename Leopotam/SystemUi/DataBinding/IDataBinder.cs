// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace EFramework.SystemUi.DataBinding {
    /// <summary>
    /// Data binding events receiver.
    /// </summary>
    public interface IDataBinder {
        /// <summary>
        /// Logical name of binded data source.
        /// </summary>
        string BindedSource { get; }

        /// <summary>
        /// /// Logical name of binded data property.
        /// </summary>
        string BindedProperty { get; }

        /// <summary>
        /// Raise on binded property changes.
        /// </summary>
        /// <param name="data">New value.</param>
        void OnBindedDataChanged (object data);
    }
}