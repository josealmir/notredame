using FluentValidation.TestHelper;
using Notredame.App.DeleteCep;
using Xunit;

namespace Notredame.Domain.Test.DeleteCep;

public sealed class DeleteCepValidatorTest
{
    private readonly DeleteCepValidator _validator;

    public DeleteCepValidatorTest()
        => _validator = new DeleteCepValidator();

    [Fact]
    public void WhenExternalIdIsValid_ShouldNotHaveError()
    {
        // Arrange
        var command = new DeleteCepCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ExternalId);
    }

    [Fact]
    public void WhenExternalIdIsEmpty_ShouldHaveError()
    {
        // Arrange
        var command = new DeleteCepCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExternalId);
    }
}
