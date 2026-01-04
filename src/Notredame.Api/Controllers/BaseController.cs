using System.Net;
using FluentValidation;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Notredame.Api.Extensions;
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

    protected async Task<IActionResult> HandleCreatedAsync(ICommandBus<(string Location, object Model)> request)
        => await _commandMediator.SendAsync(request) switch
        {
            (true, var result) => Created(result.Location, result.Model),
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
    private IActionResult TreatErrorQuery(Exception? error)
        => error switch
        {   
            InvalidRequestException e => Problem(detail: e.Message, statusCode: (int)HttpStatusCode.BadRequest),
            DomainException e => Problem(detail: e.Message, statusCode: (int)HttpStatusCode.NotFound),
            not null => Problem(detail: error.Message),
            _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
        };
    
    [NonAction]
    private IActionResult TreatErrorCommand(Exception? error)
        => error switch
        {   
            ValidationException e => ValidationProblem(modelStateDictionary: e.Errors.ToModalState(ModelState)),
            DomainException e => Problem(detail: error.Message, statusCode: (int)HttpStatusCode.UnprocessableEntity),
            not null => Problem(detail: error.Message),
            _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
        };
}