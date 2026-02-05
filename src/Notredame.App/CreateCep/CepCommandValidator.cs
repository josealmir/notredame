using FluentValidation;

using Notredame.Domain.VOs;

namespace Notredame.App.CreateCep;

public class CepCommandValidator : AbstractValidator<CepCommand>
{
    public CepCommandValidator()
    {
        RuleFor(x=> x.ZipCode)
            .NotEmpty()
            .Matches(Cep.CepRegex())
            .WithMessage("Invalid CEP");
    }
}