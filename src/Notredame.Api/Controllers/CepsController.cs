using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace Notredame.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class CepsController : ControllerBase
{
    [HttpGet("{cep}")]
    public async Task<IActionResult> GetAsync(string cep)
        => Ok(new { cep });
}
