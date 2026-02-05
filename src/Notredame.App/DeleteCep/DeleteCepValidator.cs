using FluentValidation;

namespace Notredame.App.DeleteCep;

public class DeleteCepValidator : AbstractValidator<DeleteCepCommand>
{
    public DeleteCepValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}