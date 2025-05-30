using System;

namespace NaughtyAttributes
{
    public abstract class NaEnableIfAttributeBase : NaMetaAttribute
    {
        public string[] Conditions { get; private set; }
        public EConditionOperator ConditionOperator { get; private set; }
        public bool Inverted { get; protected set; }

        /// <summary>
        ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
        /// </summary>
        public Enum EnumValue { get; private set; }

        public NaEnableIfAttributeBase(string condition)
        {
            ConditionOperator = EConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public NaEnableIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }

        public NaEnableIfAttributeBase(string enumName, Enum enumValue)
            : this(enumName)
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException(nameof(enumValue), "This parameter must be an enum value.");
            }

            EnumValue = enumValue;
        }
    }
}
