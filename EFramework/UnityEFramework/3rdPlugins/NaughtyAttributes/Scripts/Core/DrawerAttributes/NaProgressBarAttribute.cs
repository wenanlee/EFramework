using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaProgressBarAttribute : NaDrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; set; }
        public string MaxValueName { get; private set; }
        public EColor Color { get; private set; }

        public NaProgressBarAttribute(string name, float maxValue, EColor color = EColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }

        public NaProgressBarAttribute(string name, string maxValueName, EColor color = EColor.Blue)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;
        }

        public NaProgressBarAttribute(float maxValue, EColor color = EColor.Blue)
            : this("", maxValue, color)
        {
        }

        public NaProgressBarAttribute(string maxValueName, EColor color = EColor.Blue)
            : this("", maxValueName, color)
        {
        }
    }
}
