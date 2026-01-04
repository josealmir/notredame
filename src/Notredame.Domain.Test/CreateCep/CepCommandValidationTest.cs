using FluentValidation.TestHelper;
using Shouldly;
using Xunit;
using Notredame.App.CreateCep;

namespace Notredame.Domain.Test.CreateCep;

public sealed class CepCommandValidationTest
{
    private readonly CepCommandValidation _validator;

    public CepCommandValidationTest()
        => _validator = new CepCommandValidation();

    [Fact]
    public void WhenZipcodeIsValid_ShouldNotHaveError()
    {
        // Arrange
        var command = new CepCommand
        {
            ZipCode = "01001-000"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
    }

    [Fact]
    public void WhenZipcodeIsEmpty_ShouldHaveError()
    {
        // Arrange
        var command = new CepCommand
        {
            ZipCode = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    }

    [Fact]
    public void WhenZipcodeIsNull_ShouldHaveError()
    {
        // Arrange
        var command = new CepCommand
        {
            ZipCode = null
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("ABCDE")]
    [InlineData("1234P678")]
    [InlineData("9999-999")]
    public void WhenZipcodeFormatIsInvalid_ShouldHaveError(string zipCode)
    {
        // Arrange
        var command = new CepCommand
        {
            ZipCode = zipCode
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode)
            .First().ErrorMessage.ShouldBe("Invalid CEP");
    }
}
