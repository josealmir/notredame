using MapsterMapper;
using Microsoft.Extensions.Logging;
using Notredame.App.GetById;
using Notredame.Domain.DTOs;
using Notredame.Domain.Exceptions;
using Notredame.Domain.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test.GetById;

public sealed class QueryByIdCepHandlerTest
{
    private readonly ICepRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<QueryByIdCepHandler> _logger;
    private readonly QueryByIdCepHandler _handler;

    public QueryByIdCepHandlerTest()
    {
        _repository = Substitute.For<ICepRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<QueryByIdCepHandler>>();

        _handler = new QueryByIdCepHandler(_logger, _repository, _mapper);
    }

    [Fact]
    public async Task WhenCepExists_ShouldReturnSuccess()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var query = new QueryByIdCep(externalId);
        var entity = new Cep
        {
            ExternalId = externalId,
            ZipCode = "60872140",
            City = "Fortaleza",
            District = "Messejana",
            State = "CE"
        };
        var expectedDto = new CepDTO("60872140", "Fortaleza", "Messejana", "CE", "", ProviderCep.Brazilapi, null);

        _repository
            .GerByIdAsync(externalId, Arg.Any<CancellationToken>())
            .Returns(entity);

        _mapper.Map<CepDTO>(entity).Returns(expectedDto);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ZipCode.ShouldBe("60872140");
        result.Value.City.ShouldBe("Fortaleza");
    }

    [Fact]
    public async Task WhenCepNotFound_ShouldReturnBusinessException()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var query = new QueryByIdCep(externalId);

        _repository
            .GerByIdAsync(externalId, Arg.Any<CancellationToken>())
            .Returns((Cep?)null);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldBeOfType<BusinessNotredameException>();
    }
}
