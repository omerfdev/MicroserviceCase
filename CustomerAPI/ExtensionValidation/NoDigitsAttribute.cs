namespace CustomerAPI.ExtensionValidation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NoDigitsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string stringValue = value.ToString();
                if (stringValue.Any(char.IsDigit))
                {
                    return new ValidationResult("Name should not contain digits.");
                }
            }

            return ValidationResult.Success;
        }
    }

}
