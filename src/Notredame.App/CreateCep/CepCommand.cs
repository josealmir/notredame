using LiteBus.Commands.Abstractions;
using Notredame.Domain.DTOs;
using Notredame.Domain.VOs;
using Notredame.Shared.Infra;

namespace Notredame.App.CreateCep;

public class CepCommand : ICommandBus<(string, object)> 
{
    public string ZipCode { get; set; } = string.Empty;
}