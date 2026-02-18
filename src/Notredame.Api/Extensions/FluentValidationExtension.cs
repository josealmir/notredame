using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Notredame.Api.Extensions;

public static class FluentValidationExtension
{
    /// <summary>
    /// Converts a list of FluentValidation errors into a ModelStateDictionary.
    /// </summary>
    /// <param name="result">The list of FluentValidation errors.</param>
    /// <param name="modelState">The ModelStateDictionary to populate.</returns>
    /// <returns>The populated ModelStateDictionary.</returns>
    extension(IEnumerable<ValidationFailure> result)
    {
        public ModelStateDictionary ToModalState(ModelStateDictionary modelState)
        {
            result.ToList().ForEach(error => modelState.AddModelError(error.PropertyName, error.ErrorMessage));
            return modelState;
        }
    }
}