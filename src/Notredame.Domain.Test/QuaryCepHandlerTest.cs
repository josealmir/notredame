using Microsoft.Extensions.Logging;

using Notredame.App.GetCep;
using Notredame.Domain.DTOs;
using Notredame.Domain.Exceptions;
using Notredame.Domain.Services;

using NSubstitute;

using Shouldly;

using Xunit;

namespace Notredame.Domain.Test;

public sealed class QueryCepHandlerTest
{
    private readonly QueryCepHandle _queryCepHandle;
    private readonly ILogger<QueryCepHandle> _logger;
    private readonly ICepService _cepService;
    
    public QueryCepHandlerTest()
    {
        _cepService = Substitute.For<ICepService>();
        _logger =  Substitute.For<ILogger<QueryCepHandle>>();
        _queryCepHandle = new QueryCepHandle( _logger, _cepService);
    }

    [Theory]
    [InlineData("60784111")]
    [InlineData("60477-140")]
    public async Task WhenInformCepValid_ShouldReturnCepDTO(string cep)
    {
       // Arrange
        var request = new QueryCep("cep");
       _cepService.SearchCepAsync(request.Cep).Returns( new CepDTO(request.Cep, "Fortaleza","Messeja","Ceara","5689",ProviderCep.Brazilapi, new LocationDTO()));
       
       // Action
       var result = await _queryCepHandle.HandleAsync(request);
       
       // Assert
       result.Value.ShouldBeNull();
    }
    
    [Theory]
    [InlineData("60874")]
    [InlineData("asadasd")]
    [InlineData("604sada-140")]
    public async Task WhenInformCepInvalid_ShouldReturnInvalidCepException(string cep)
    {
        // Arrange
        var request = new QueryCep(cep);
        
        // Action
        var result = await _queryCepHandle.HandleAsync(request);   
        
        // Assert
        result.Exception.ShouldBeOfType<InvalidCepException>();
    }

    [Theory]
    [InlineData("60000000")]
    public async Task WhenInformCepInvalid_ShouldReturnBusinessNotredameException(string cep)
    {
        // Arrange
        var request = new QueryCep(cep);
        _cepService.SearchCepAsync(request.Cep)!.Returns(Task.FromResult<CepDTO>(null!));
        
        // Action
        var result = await _queryCepHandle.HandleAsync(request);   
        
        // Assert
        result.Exception.ShouldBeOfType<BusinessNotredameException>();
    }

}