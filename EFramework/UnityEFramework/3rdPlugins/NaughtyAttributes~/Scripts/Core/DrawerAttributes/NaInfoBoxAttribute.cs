using System;

namespace NaughtyAttributes
{
    public enum ENaInfoBoxType
    {
        Normal,
        Warning,
        Error
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NaInfoBoxAttribute : NaDrawerAttribute
    {
        public string Text { get; private set; }
        public ENaInfoBoxType Type { get; private set; }

        public NaInfoBoxAttribute(string text, ENaInfoBoxType type = ENaInfoBoxType.Normal)
        {
            Text = text;
            Type = type;
        }
    }
}
