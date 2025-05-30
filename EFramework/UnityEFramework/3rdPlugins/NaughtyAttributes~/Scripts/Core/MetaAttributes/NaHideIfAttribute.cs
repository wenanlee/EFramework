using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NaHideIfAttribute : NaShowIfAttributeBase
    {
        public NaHideIfAttribute(string condition)
            : base(condition)
        {
            Inverted = true;
        }

        public NaHideIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = true;
        }

        public NaHideIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = true;
        }
    }
}
