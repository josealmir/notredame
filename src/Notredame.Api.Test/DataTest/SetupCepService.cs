using Notredame.Domain;
using Notredame.Domain.DTOs;
using Notredame.Domain.Services;

using NSubstitute;

namespace Notredame.Api.Test.DataTest;

public static class SetupCepService
{
    private static CepDTO CepDtoOk()
        => new CepDTO("60872140", "Fortaleza", "Messejana", "Ceara", "60874", ProviderCep.Viacep, null);

    private static CepDTO CepDtoNotFound()
        => new CepDTO("", "", "", "", "", ProviderCep.Viacep, null);
    
    public static ICepService CepService()
    {
        var mockCepService = Substitute.For<ICepService>();
        mockCepService.SearchCepAsync("60872140").Returns(Task.FromResult(SetupCepService.CepDtoOk()));
        mockCepService.SearchCepAsync("60874300").Returns(Task.FromResult(SetupCepService.CepDtoNotFound()));
        mockCepService.When(x => x.SearchCepAsync("00000000")).Do(x=> throw new TimeoutException());
        return mockCepService;
    }
}