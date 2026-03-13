using MapsterMapper;
using Microsoft.Extensions.Logging;
using Notredame.App.DeleteCep;
using Notredame.Domain.DTOs;
using Notredame.Domain.Exceptions;
using Notredame.Domain.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Notredame.Domain.Test.DeleteCep;

public sealed class DeleteCepHandlerTest
{
    private readonly ICepRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCepHandler> _logger;
    private readonly DeleteCepHandler _handler;

    public DeleteCepHandlerTest()
    {
        _repository = Substitute.For<ICepRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<DeleteCepHandler>>();

        _handler = new DeleteCepHandler(_logger, _mapper, _unitOfWork, _repository);
    }

    [Fact]
    public async Task WhenCepExists_ShouldDeleteAndReturnSuccess()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var command = new DeleteCepCommand(externalId);
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
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ZipCode.ShouldBe("60872140");

        _repository.Received(1).Delete(entity);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task WhenCepNotFound_ShouldReturnBusinessException()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var command = new DeleteCepCommand(externalId);

        _repository
            .GerByIdAsync(externalId, Arg.Any<CancellationToken>())
            .Returns((Cep?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldBeOfType<BusinessNotredameException>();

        _repository.DidNotReceive().Delete(Arg.Any<Cep>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task WhenCommandIsNull_ShouldThrowException()
    {
        // Act & Assert
        await Should.ThrowAsync<NullReferenceException>(
            () => _handler.HandleAsync(null!));
    }
}
