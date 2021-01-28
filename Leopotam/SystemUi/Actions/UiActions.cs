// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace EFramework.SystemUi.Actions {
    /// <summary>
    /// Helpers for Ui actions.
    /// </summary>
    public static class UiActions {
        /// <summary>
        /// Calculate hash of string form of group event. Can be changed later.
        /// </summary>
        /// <param name="groupName">Group name for hashing.</param>
        public static int GetUiActionGroupId (this string groupName) {
            return groupName != null ? groupName.GetHashCode () : 0;
        }
    }
}