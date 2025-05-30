using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaBoxGroupAttribute : NaMetaAttribute, INaGroupAttribute
    {
        public string Name { get; private set; }

        public NaBoxGroupAttribute(string name = "")
        {
            Name = name;
        }
    }
}
