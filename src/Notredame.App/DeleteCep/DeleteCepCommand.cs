using Notredame.Domain.DTOs;
using Notredame.Shared.Infra;

namespace Notredame.App.DeleteCep;

public record DeleteCepCommand(Guid ExternalId): ICommandBus<CepDTO>;