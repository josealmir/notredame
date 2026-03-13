using Notredame.Domain.DTOs;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test.DTOs;

public sealed class CepDtoTest
{
    [Fact]
    public void IsValid_WhenDtoIsNull_ShouldReturnFalse()
    {
        CepDTO.IsValid(null).ShouldBeFalse();
    }

    [Fact]
    public void IsValid_WhenZipCodeIsEmpty_ShouldReturnFalse()
    {
        var dto = new CepDTO("", "City", "District", "ST", "1234", ProviderCep.Viacep, null);
        CepDTO.IsValid(dto).ShouldBeFalse();
    }

    [Fact]
    public void IsValid_WhenZipCodeIsWhitespace_ShouldReturnFalse()
    {
        var dto = new CepDTO("   ", "City", "District", "ST", "1234", ProviderCep.Viacep, null);
        CepDTO.IsValid(dto).ShouldBeFalse();
    }

    [Fact]
    public void IsValid_WhenDtoHasValidZipCode_ShouldReturnTrue()
    {
        var dto = new CepDTO("60872-140", "Fortaleza", "Messejana", "CE", "1234", ProviderCep.Brazilapi, new LocationDTO());
        CepDTO.IsValid(dto).ShouldBeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetAllProperties()
    {
        var location = new LocationDTO { Lat = -3.8, Lon = -38.5 };
        var dto = new CepDTO("60872-140", "Fortaleza", "Messejana", "CE", "1234", ProviderCep.Brazilapi, location);

        dto.ZipCode.ShouldBe("60872-140");
        dto.City.ShouldBe("Fortaleza");
        dto.District.ShouldBe("Messejana");
        dto.State.ShouldBe("CE");
        dto.Ibge.ShouldBe("1234");
        dto.Provider.ShouldBe(ProviderCep.Brazilapi);
        dto.Location.ShouldBe(location);
    }

    [Fact]
    public void DefaultConstructor_ShouldHaveEmptyDefaults()
    {
        var dto = new CepDTO();

        dto.ZipCode.ShouldBe(string.Empty);
        dto.City.ShouldBe(string.Empty);
    }
}
