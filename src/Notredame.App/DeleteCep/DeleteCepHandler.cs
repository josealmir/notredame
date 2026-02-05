using MapsterMapper;
using OperationResult;
using Notredame.Domain.DTOs;
using Notredame.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using LiteBus.Commands.Abstractions;
using Notredame.Domain.Repositories;

namespace Notredame.App.DeleteCep;

public sealed class DeleteCepHandler(
        ILogger<DeleteCepHandler> logger,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ICepRepository repository): ICommandHandler<DeleteCepCommand, Result<CepDTO>>
{
    public async Task<Result<CepDTO>> HandleAsync(DeleteCepCommand message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "externalId", message.ExternalId } }))
        {
            ArgumentNullException.ThrowIfNull(message, nameof(DeleteCepCommand));
            
            var entity = await repository.GerByIdAsync(message.ExternalId, cancellationToken);
            if (entity is null)
               return Result.Error<CepDTO>( new BusinessNotredameException("Cep not found"));
            
            repository.Delete(entity);
            await unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(mapper.Map<CepDTO>(entity));
        }
    }
}