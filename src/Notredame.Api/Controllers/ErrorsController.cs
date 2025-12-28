using System.Net;

using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Polly.Timeout;

namespace Notredame.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Errors")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class ErrorsController : ControllerBase
{

    [Route("/error")]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        return context?.Error switch
        {
            TimeoutRejectedException or
            TimeoutException => Problem(detail: context.Error.Message, statusCode: (int)HttpStatusCode.GatewayTimeout),
            _ => Problem(detail: context?.Error.Message),
        };
    }
}