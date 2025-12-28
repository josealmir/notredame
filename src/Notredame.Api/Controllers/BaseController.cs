using System.Net;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Notredame.Domain.Exceptions;
using Notredame.Shared.Infra;

namespace Notredame.Api.Controllers;

public abstract class BaseController: ControllerBase
{
    protected BaseController(IQueryMediator queryMediator, ICommandMediator commandMediator)
    {
        _queryMediator = queryMediator;
        _commandMediator = commandMediator;
    }

    private readonly IQueryMediator _queryMediator;
    private readonly ICommandMediator _commandMediator;
    
    protected async Task<IActionResult> HandleGetAsync<T>(IQueryBus<T> request)
        => await _queryMediator.QueryAsync(request) switch 
        {
            (true, var value ,_) => Ok(value),
            (false, _, var error) => TreatErrorQuery(error),
        };

    protected async Task<IActionResult> HandlePostAsync<T>(ICommandBus<T> request)
        => await _commandMediator.SendAsync(request) switch
        {
            (true, var result) => Created("", result),
            (false, _ ,var error) => TreatErrorCommand(error),
            _ => throw new ArgumentOutOfRangeException()
        };

    protected async Task<IActionResult> HandleRequestAsync<T>(ICommandBus<T> request)
        => await _commandMediator.SendAsync(request) switch
        {
            (true, var result) => Ok(result),
            (false, _, var error) => TreatErrorCommand(error),
            _ => throw new ArgumentOutOfRangeException()
        };

    [NonAction]
    private ObjectResult TreatErrorQuery(Exception? error)
        => error switch
        {   
            InvalidRequestException e => Problem(detail: e.Message, statusCode: (int)HttpStatusCode.BadRequest),
            DomainException e => Problem(detail: e.Message, statusCode: (int)HttpStatusCode.NotFound),
            not null => Problem(detail: error.Message),
            _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
        };
    
    [NonAction]
    public ObjectResult TreatErrorCommand(Exception? error)
        => error switch
        {   
            InvalidRequestException e => Problem(detail: error.Message, statusCode: (int)HttpStatusCode.BadRequest),
            DomainException e => Problem(detail: error.Message, statusCode: (int)HttpStatusCode.UnprocessableEntity),
            not null => Problem(detail: error.Message),
            _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
        };
}