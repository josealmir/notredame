using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Notredame.Api.Extensions;

public static class FluentValidationExtension
{
    extension(IEnumerable<ValidationFailure> result)
    {
        public ModelStateDictionary ToModalState(ModelStateDictionary modelState)
        {
            result.ToList().ForEach(error => modelState.AddModelError(error.PropertyName, error.ErrorMessage));
            return modelState;
        }
    }
}