using Notredame.Domain.DTOs;
using Notredame.Shared.Infra;

namespace Notredame.App.GetById;

public record QueryByIdCep(Guid ExternalId) : IQueryBus<CepDTO>; 