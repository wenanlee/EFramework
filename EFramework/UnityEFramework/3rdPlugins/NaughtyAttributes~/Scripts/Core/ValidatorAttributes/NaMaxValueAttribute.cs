using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaMaxValueAttribute : NaValidatorAttribute
    {
        public float MaxValue { get; private set; }

        public NaMaxValueAttribute(float maxValue)
        {
            MaxValue = maxValue;
        }

        public NaMaxValueAttribute(int maxValue)
        {
            MaxValue = maxValue;
        }
    }
}
