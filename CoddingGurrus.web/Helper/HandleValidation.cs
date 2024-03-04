
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;

namespace CoddingGurrus.web.Helper
{
    public static class ValidationHelper
    {
        public static ValidationResult ValidateModel<T>(T model, AbstractValidator<T> validator)
        {
            return validator.Validate(model);
        }

        public static IDictionary<string, string> GetValidationErrors(ValidationResult validationResult)
        {
            return validationResult.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage);
        }
    }
}

