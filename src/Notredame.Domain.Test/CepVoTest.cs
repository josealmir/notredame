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
}