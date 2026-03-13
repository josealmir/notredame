using Asp.Versioning;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Notredame.App.CepPaginated;
using Notredame.App.CreateCep;
using Notredame.App.DeleteCep;
using Notredame.App.GetById;
using Notredame.App.GetCep;
using Notredame.Domain.DTOs;
using Notredame.Shared.Models;

namespace Notredame.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class CepsController(
    IQueryMediator queryMediator, 
    ICommandMediator commandMediator) : BaseController(queryMediator, commandMediator)
{
    [HttpGet]
    [ProducesResponseType<PageResult<CepResult>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagineted([FromQuery] QueryCepPaginated query)
        => await HandleGetAsync(query);

    [HttpGet("{cep}")]
    [ProducesResponseType<CepDTO>(StatusCodes.Status200OK )]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(string cep)
        => await HandleGetAsync(new QueryCep(cep));
    
    [HttpGet("{externalId:Guid}")]
    [ProducesResponseType<CepDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid externalId)
        =>  await HandleGetAsync(new QueryByIdCep(externalId));
    
    [HttpPost]
    [ProducesResponseType<CepCreatedDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync(CepCommand cepCommand)
        => await HandleCreatedAsync(cepCommand);
    
    [HttpDelete("{externalId:Guid}")]
    [ProducesResponseType<CepCreatedDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(Guid externalId)
        => await HandleRequestAsync(new DeleteCepCommand(externalId));
}
