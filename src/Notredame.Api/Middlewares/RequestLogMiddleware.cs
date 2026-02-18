using System.Text;
using Notredame.Api.Extensions;

namespace Notredame.Api.Middlewares;

internal class RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var originalBodyStream = httpContext.Response.Body;
        var responseBody = new MemoryStream();

        var requestBody = await ReadRequestAsync(httpContext.Request);
        httpContext.Response.Body = responseBody;

        using (logger.BeginScope(new Dictionary<string, object>(StringComparer.Ordinal) { ["BodyRequest"] = requestBody }))
        {
            await next(httpContext);

            await ReadResponseAsync(httpContext.Response);
            await responseBody.CopyToAsync(originalBodyStream, httpContext.RequestAborted);
        }
    }

    private static async Task<string> ReadRequestAsync(HttpRequest httpRequest)
    {
        httpRequest.EnableBuffering();
        httpRequest.Body.Position = 0;

        var requestBody = await new StreamReader(httpRequest.Body, encoding: Encoding.UTF8).ReadToEndAsync();
        httpRequest.Body.Position = 0;

        httpRequest.AddTagRequest(requestBody);
        return requestBody;
    }

    private static async Task<string> ReadResponseAsync(HttpResponse response)
    {
        response.Body.Position = 0;
        var responseBody = await new StreamReader(response.Body).ReadToEndAsync();

        response.Body.Position = 0;
        response.AddTagResponse(responseBody);
        return responseBody;
    }
}