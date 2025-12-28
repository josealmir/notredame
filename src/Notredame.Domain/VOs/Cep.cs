global using cep = Notredame.Domain.VOs.Cep;
using System.Text.RegularExpressions;
using Notredame.Domain.Exceptions;

namespace Notredame.Domain.VOs;

public readonly partial record struct Cep
{
    private readonly string _number;

    private Cep(string number)
    {
        _number = number;
    }

    public static cep Parse(string value)
    {
        var digits = new string(value.Where(char.IsDigit).ToArray());

        return digits.Length != 8 ? throw new InvalidCepException("CEP invalid") : new cep(digits);
    }

    public static bool TryParse(string value, out Cep cep)
    {
        cep = default;
        if (!CepRegex().IsMatch(value))
            return false;
        
        cep = Parse(value);
        return true;
    }

    public override string ToString()
        => $"{_number[..5]}-{_number[5..]}";
    
    public static implicit operator string(Cep cep) 
        => cep._number;
    
    public static implicit operator Cep(string cep)
        => new (cep);
    
    [GeneratedRegex(@"^\d{5}-?\d{3}$")]
    private static partial Regex CepRegex();
}

