using Notredame.Domain;
using Notredame.Domain.DTOs;
using Notredame.Domain.Services;

using NSubstitute;

namespace Notredame.Api.Test.DataTest;

public static class SetupCepService
{
    public static Cep CepDtoOk()
        => new Cep
        {
            City = "Fortaleza",
            Location = new Location
            {
                Lat = -897.15649,
                Lon =  897.15649
            },
            District = "Messejana",
            State = "CE",
            Provider = ProviderCep.Brazilapi,
            ZipCode = "60872140",
            Ibge = "979"
        };

    public static CepDTO CepDtoNotFound()
        => new CepDTO("", "", "", "", "", ProviderCep.Viacep, null);
}