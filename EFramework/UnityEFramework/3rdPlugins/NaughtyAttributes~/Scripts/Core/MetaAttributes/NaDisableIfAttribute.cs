using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NaDisableIfAttribute : NaEnableIfAttributeBase
    {
        public NaDisableIfAttribute(string condition)
            : base(condition)
        {
            Inverted = true;
        }

        public NaDisableIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = true;
        }

        public NaDisableIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = true;
        }
    }
}
