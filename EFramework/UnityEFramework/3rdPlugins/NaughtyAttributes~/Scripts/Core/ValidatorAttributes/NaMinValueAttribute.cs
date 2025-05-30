using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaMinValueAttribute : NaValidatorAttribute
    {
        public float MinValue { get; private set; }

        public NaMinValueAttribute(float minValue)
        {
            MinValue = minValue;
        }

        public NaMinValueAttribute(int minValue)
        {
            MinValue = minValue;
        }
    }
}
