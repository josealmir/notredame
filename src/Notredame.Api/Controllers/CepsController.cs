using Asp.Versioning;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Notredame.App.CreateCep;
using Notredame.App.GetCep;
using Notredame.Domain.DTOs;

namespace Notredame.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class CepsController(
    IQueryMediator queryMediator, 
    ICommandMediator commandMediator) : BaseController(queryMediator, commandMediator)
{
    
    [HttpGet("{cep}")]
    [ProducesResponseType<CepDTO>(StatusCodes.Status200OK )]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(string cep)
        => await HandleGetAsync(new QueryCep(cep));
    
    [HttpGet("{id:Guid}")]
    [ProducesResponseType<CepDTO>(StatusCodes.Status200OK )]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
        =>  await HandleGetAsync(new QueryCep("s"));
    
    [HttpPost]
    [ProducesResponseType<CepCreatedDTO>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync(CepCommand cepCommand)
        => await HandleCreatedAsync(cepCommand);
}
