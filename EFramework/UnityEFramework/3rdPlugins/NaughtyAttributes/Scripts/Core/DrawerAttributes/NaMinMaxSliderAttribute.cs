using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaMinMaxSliderAttribute : NaDrawerAttribute
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public NaMinMaxSliderAttribute(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
