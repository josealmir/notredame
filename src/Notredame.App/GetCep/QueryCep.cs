using Notredame.Domain.DTOs;
using Notredame.Shared.Infra;

namespace Notredame.App.GetCep;

public record QueryCep(string Cep) : IQueryBus<CepDTO> ;