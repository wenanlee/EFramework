using System;

namespace NaughtyAttributes
{
    public enum ENaButtonEnableMode
    {
        /// <summary>
        /// Button should be active always
        /// </summary>
        Always,
        /// <summary>
        /// Button should be active only in editor
        /// </summary>
        Editor,
        /// <summary>
        /// Button should be active only in playmode
        /// </summary>
        Playmode
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NaButtonAttribute : NaSpecialCaseDrawerAttribute
    {
        public string Text { get; private set; }
        public ENaButtonEnableMode SelectedEnableMode { get; private set; }

        public NaButtonAttribute(string text = null, ENaButtonEnableMode enabledMode = ENaButtonEnableMode.Always)
        {
            this.Text = text;
            this.SelectedEnableMode = enabledMode;
        }
    }
}
