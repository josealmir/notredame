using MapsterMapper;
using Microsoft.Extensions.Logging;
using Notredame.App.CreateCep;
using Notredame.Domain.DTOs;
using Notredame.Domain.Commons;
using Notredame.Domain.Services;
using Notredame.Domain.Exceptions;
using Notredame.Domain.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test.CreateCep;

public sealed class CepCommandHandlerTest
{
    private readonly ICepService _cepService;
    private readonly IMapper _mapper;
    private readonly ICepRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CepCommandHandler> _logger;
    private readonly IEnvironmentExecution<Exception> _environment;
    private readonly CepCommandHandler _handler;

    public CepCommandHandlerTest()
    {
        _cepService = Substitute.For<ICepService>();
        _mapper = Substitute.For<IMapper>();
        _repository = Substitute.For<ICepRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _environment = Substitute.For<IEnvironmentExecution<Exception>>();
        _logger = Substitute.For<ILogger<CepCommandHandler>>();

        _handler = new CepCommandHandler(
            _cepService,
            _mapper,
            _repository,
            _unitOfWork,
            _environment,
            _logger
        );
    }
    
    [Fact]
    public async Task WhenCepNotFound_ShouldReturnError()
    {
        // Arrange
        var command = new CepCommand
        {
            ZipCode = "01001000"
        };

        _cepService
            .SearchCepAsync(command.ZipCode)
            .Returns((CepDTO?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldBeOfType<BusinessNotredameException>();

        await _repository.DidNotReceive()
            .AddAsync(Arg.Any<Domain.Cep>());

        await _unitOfWork.DidNotReceive()
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
     public async Task ShouldPersistCepAndReturnSuccess()
    {
        // Arrange
        var command = new CepCommand
        {
            ZipCode = "01001000"
        };

        var cepDto = new CepDTO("01001-000", "São pualo", "São Paulo", "SP", string.Empty, ProviderCep.Viacep, null);

        var cepEntity = new Domain.Cep 
        {            
            ExternalId = Guid.NewGuid()
        };

        _cepService
            .SearchCepAsync(command.ZipCode)
            .Returns(cepDto);

        _mapper
            .Map<Domain.Cep>(cepDto)
            .Returns(cepEntity);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var (uri, payload) = result.Value;

        uri.ShouldBe($"/api/v1/ceps/{cepEntity.ExternalId}");
        payload.ShouldNotBeNull();

        await _repository.Received(1)
            .AddAsync(cepEntity);

        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }
}