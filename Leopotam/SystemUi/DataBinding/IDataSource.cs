// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace EFramework.SystemUi.DataBinding {
    /// <summary>
    /// Handler for raise event on binded property changes.
    /// </summary>
    /// <param name="data">New value.</param>
    public delegate void OnBindedDataChangedHandler (IDataSource source, string property);

    /// <summary>
    /// Interface of user data source.
    /// </summary>
    /// <param name="data">New value.</param>
    public interface IDataSource {
        /// <summary>
        /// Should be raised on each binded property value. IDataSource parameter should be equals to "this". 
        /// </summary>
        event OnBindedDataChangedHandler OnBindedDataChanged;
    }
}