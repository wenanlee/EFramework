using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaRequiredAttribute : NaValidatorAttribute
    {
        public string Message { get; private set; }

        public NaRequiredAttribute(string message = null)
        {
            Message = message;
        }
    }
}
