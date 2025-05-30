using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaLabelAttribute : NaMetaAttribute
    {
        public string Label { get; private set; }

        public NaLabelAttribute(string label)
        {
            Label = label;
        }
    }
}
