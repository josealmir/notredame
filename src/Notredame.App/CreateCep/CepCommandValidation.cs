using FluentValidation;

using Notredame.Domain.VOs;

namespace Notredame.App.CreateCep;

public class CepCommandValidation : AbstractValidator<CepCommand>
{
    public CepCommandValidation()
    {
        RuleFor(x=> x.ZipCode)
            .NotEmpty()
            .Matches(Cep.CepRegex())
            .WithMessage("Invalid CEP");
    }
}