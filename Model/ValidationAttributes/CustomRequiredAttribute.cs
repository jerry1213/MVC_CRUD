using System.ComponentModel.DataAnnotations;

namespace Model.ValidationAttributes
{
    public class CustomRequiredAttribute : RequiredAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext) != null ? new ValidationResult($"請輸入[{validationContext.DisplayName}]。") : null;
        }
    }
}
