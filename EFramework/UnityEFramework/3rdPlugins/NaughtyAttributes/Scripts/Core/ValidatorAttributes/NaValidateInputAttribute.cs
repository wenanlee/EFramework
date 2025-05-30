using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaValidateInputAttribute : NaValidatorAttribute
    {
        public string CallbackName { get; private set; }
        public string Message { get; private set; }

        public NaValidateInputAttribute(string callbackName, string message = null)
        {
            CallbackName = callbackName;
            Message = message;
        }
    }
}
