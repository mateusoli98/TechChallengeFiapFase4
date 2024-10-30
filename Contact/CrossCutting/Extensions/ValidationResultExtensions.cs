using ErrorOr;
using FluentValidation.Results;

namespace CrossCutting.Extensions
{
    public static class ValidationResultExtensions
    {
        public static List<Error> ToErrorList(this ValidationResult validationResult)
        {
            var errors = new List<Error>();

            foreach (var error in validationResult.Errors)
            {
                errors.Add(Error.Validation(
                    code: error.PropertyName,
                    description: error.ErrorMessage
                )); ;
            }

            return errors;
        }
    }
}
