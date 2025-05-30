using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NaFoldoutAttribute : NaMetaAttribute, INaGroupAttribute
    {
        public string Name { get; private set; }

        public NaFoldoutAttribute(string name)
        {
            Name = name;
        }
    }
}
