using System;
using System.Collections.Generic;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyValidatorBase
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }

    public static class ValidatorAttributeExtensions
    {
        private static Dictionary<Type, PropertyValidatorBase> _validatorsByAttributeType;

        static ValidatorAttributeExtensions()
        {
            _validatorsByAttributeType = new Dictionary<Type, PropertyValidatorBase>();
            _validatorsByAttributeType[typeof(NaMinValueAttribute)] = new MinValuePropertyValidator();
            _validatorsByAttributeType[typeof(NaMaxValueAttribute)] = new MaxValuePropertyValidator();
            _validatorsByAttributeType[typeof(NaRequiredAttribute)] = new RequiredPropertyValidator();
            _validatorsByAttributeType[typeof(NaValidateInputAttribute)] = new ValidateInputPropertyValidator();
        }

        public static PropertyValidatorBase GetValidator(this NaValidatorAttribute attr)
        {
            PropertyValidatorBase validator;
            if (_validatorsByAttributeType.TryGetValue(attr.GetType(), out validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}
