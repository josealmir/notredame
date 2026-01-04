using System.Diagnostics;
using System.Reflection;

using OpenTelemetry.Resources;

namespace Notredame.Shared;

public static class NotredameResource
{
    public const string ServiceName = "notredame-api";
    public static readonly string ServiceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
    
    public static ActivitySource ActivitySource
        => new(ServiceName, ServiceVersion);

    public static ResourceBuilder Instance
        => ResourceBuilder.CreateDefault()
            .AddService(serviceName: ServiceName, serviceVersion: ServiceVersion);
}