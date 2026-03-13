using Notredame.Domain.Exceptions;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test;

public class CepVoTest
{
    [Theory]
    [InlineData("607a4-111")]
    [InlineData("ddese-140")]
    public void WhenInformCepWithLettersShouldThrowInvalidCepException(string value)
    {
        // Arrange
        
        // Action
        var action = () => { VOs.Cep.Parse(value);  };

        // Assert
        Should.Throw<InvalidRequestException>(action);
    }

    [Theory]
    [InlineData("60784111")]
    [InlineData("60477-140")]
    public void WhenInformCepWithNumberShouldCepVo(string value)
    {
        var action = () => VOs.Cep.Parse(value);
        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData("60784111", true)]
    [InlineData("60477-140", true)]
    [InlineData("607a4-111", false)]
    [InlineData("123", false)]
    [InlineData("abcdefgh", false)]
    public void TryParse_ShouldReturnExpectedResult(string value, bool expected)
    {
        // Act
        var result = VOs.Cep.TryParse(value, out _);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("60784111", "60784-111")]
    [InlineData("60477140", "60477-140")]
    public void ToString_ShouldReturnFormattedCep(string input, string expected)
    {
        // Arrange
        var cep = VOs.Cep.Parse(input);

        // Act & Assert
        cep.ToString().ShouldBe(expected);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldReturnDigits()
    {
        // Arrange
        var cep = VOs.Cep.Parse("60784111");

        // Act
        string result = cep;

        // Assert
        result.ShouldBe("60784111");
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldCreateCepVo()
    {
        // Act
        VOs.Cep cep = "60784111";

        // Assert
        string value = cep;
        value.ShouldBe("60784111");
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("123456789")]
    public void Parse_WhenDigitCountIsNot8_ShouldThrowInvalidCepException(string value)
    {
        var action = () => { VOs.Cep.Parse(value); };
        Should.Throw<InvalidCepException>(action);
    }
}
