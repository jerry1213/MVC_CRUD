using System.ComponentModel.DataAnnotations;

namespace YungChing_MVC.Validations
{
    public class CommonValidator
    {
        public (bool isValid, string errorMessages) GetValidationResults(object reqModel)
        {
            var context = new ValidationContext(reqModel);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(reqModel, context, validationResults, true);
            var errorMessages = "";
            if (!isValid)
            {
                errorMessages = string.Join("<br>", validationResults.Select(s => s.ErrorMessage));
            }
            return (isValid, errorMessages);
        }
    }
}
