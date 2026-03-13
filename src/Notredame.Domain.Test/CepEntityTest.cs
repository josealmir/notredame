using Notredame.Domain.DTOs;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test;

public sealed class CepEntityTest
{
    [Fact]
    public void Map_ShouldReturnCepCreatedDtoWithCorrectValues()
    {
        // Arrange
        var cep = new Cep
        {
            ZipCode = "60872140",
            City = "Fortaleza",
            District = "Messejana",
            State = "CE",
            Ibge = "2304400",
            Provider = ProviderCep.Brazilapi,
            Location = new Location { Lat = -3.8326, Lon = -38.4958 }
        };

        // Act
        var result = cep.Map();

        // Assert
        result.ShouldBeOfType<CepCreatedDTO>();
        result.ZipCode.ShouldBe("60872140");
        result.City.ShouldBe("Fortaleza");
        result.District.ShouldBe("Messejana");
        result.State.ShouldBe("CE");
        result.Provider.ShouldBe(ProviderCep.Brazilapi);
        result.ExternalId.ShouldBe(cep.ExternalId);
        result.CreatedAt.ShouldBe(cep.CreatedAt);
        result.Location.Lat.ShouldBe(-3.8326);
        result.Location.Lon.ShouldBe(-38.4958);
    }

    [Fact]
    public void Entity_ShouldHaveDefaultValues()
    {
        // Act
        var cep = new Cep();

        // Assert
        cep.ExternalId.ShouldNotBe(Guid.Empty);
        cep.CreatedAt.ShouldNotBe(default);
        cep.ModifiedAt.ShouldBeNull();
        cep.ZipCode.ShouldBe(string.Empty);
    }

    [Fact]
    public void LocationMap_ShouldReturnLocationDTO()
    {
        // Arrange
        var location = new Location { Lat = -3.8326, Lon = -38.4958 };

        // Act
        var dto = location.Map();

        // Assert
        dto.ShouldBeOfType<LocationDTO>();
        dto.Lat.ShouldBe(-3.8326);
        dto.Lon.ShouldBe(-38.4958);
    }
}
