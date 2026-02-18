using System.Diagnostics;

namespace Notredame.Api.Extensions;

public static class HttpRequestExtension
{
    /// <summary>
    /// Adds a tag to the current activity containing the response body and location.
    /// </summary>
    /// <param name="response">The response to add tags to.</param>
    /// <param name="body">The response body to add to the tag. If null, the tag will not be added.</param>
    extension(HttpResponse httpResponse)
    {
        public void AddTagResponse(string? body = null)
        {
            Activity.Current?.SetTag("http.response_body", body);
            Activity.Current?.SetTag("http.location", httpResponse.Headers.Location);
            Activity.Current?.SetTag("http.content_language", httpResponse.Headers.ContentLanguage);
        }
    }


    /// <summary>
    /// Adds tags to the current activity containing the request body and Accept-Language header.
    /// </summary>
    /// <param name="body">The request body to add to the tag. If null, the tag will not be added.</param>
    extension(HttpRequest httpRequest)
    {
        public void AddTagRequest(string? body = null)
        {
            Activity.Current?.SetTag("http.request_body", body);
            Activity.Current?.SetTag("http.request_accept_language", httpRequest.Headers.AcceptLanguage);
        }
    }
}