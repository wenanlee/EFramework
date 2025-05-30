using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NaOnValueChangedAttribute : NaMetaAttribute
    {
        public string CallbackName { get; private set; }

        public NaOnValueChangedAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }
    }
}
