using Notredame.Shared;
using Notredame.Shared.Options;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Notredame.Api.Settings;

public static class ApmNotredame
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder UseApmNotredame()
        {
            // var apmOptions = builder.Configuration.GetRequiredSection(NotredameApmOption.SectionName).Get<NotredameApmOption>() ?? null;
            builder.Services.AddOpenTelemetry()
                .WithTracing(tracingBuilder =>
                {
                    tracingBuilder
                        .SetResourceBuilder(NotredameResource.Instance.AddService(NotredameResource.ServiceName))
                        .SetErrorStatusOnException()
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation();
                })
                .WithMetrics((metricsBuilder) =>
                {
                    metricsBuilder
                        .SetResourceBuilder(NotredameResource.Instance.AddService(NotredameResource.ServiceName))
                        .AddMeter(NotredameResource.ServiceName)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddPrometheusExporter();
                }).UseOtlpExporter();
            builder.Logging.ClearProviders();
            builder.Services.AddLogging(logger =>
            {
                logger.AddOpenTelemetry(options =>
                    {
                        options.SetResourceBuilder(
                            NotredameResource.Instance.AddService(NotredameResource.ServiceName));
                        options.IncludeScopes = true; 
                        options.IncludeFormattedMessage = true;
                    })
                    .AddJsonConsole();
            });

            return builder;
        }
    }
}
