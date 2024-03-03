using System.ComponentModel.DataAnnotations;

namespace Noc_App.Helpers
{
    public class NumericValidationAttribute: ValidationAttribute
    {
        private readonly Type _numericType;
        public NumericValidationAttribute(Type numericType)
        {
            _numericType = numericType;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // You may choose to handle null values differently based on your requirements
                return ValidationResult.Success;
            }

            if (!IsNumeric(value))
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a {_numericType.Name}.");
            }

            return ValidationResult.Success;
        }

        private bool IsNumeric(object value)
        {
            return _numericType switch
            {
                Type t when t == typeof(int) => int.TryParse(value.ToString(), out _),
                Type t when t == typeof(double) => double.TryParse(value.ToString(), out _),
                Type t when t == typeof(float) => float.TryParse(value.ToString(), out _),
                Type t when t == typeof(decimal) => decimal.TryParse(value.ToString(), out _),
                _ => false,
            };
        }
    }
}
