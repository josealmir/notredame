using Notredame.Shared;
using Notredame.Shared.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Notredame.Api.Settings;

public static class ApmNotredame
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApmNotredame(IConfiguration configuration)
        {
            var apmOptions = configuration.GetRequiredSection(NotredameApmOption.SectionName).Get<NotredameApmOption>() ?? null;
            services.AddOpenTelemetry()
                .ConfigureResource(b => b = NotredameResource.Instance)
                .WithTracing(tracingBuilder =>
                {
                    tracingBuilder
                        .AddSource(NotredameResource.ServiceName)
                        .SetResourceBuilder(NotredameResource.Instance)
                        .SetSampler(new AlwaysOnSampler())
                        .SetErrorStatusOnException()
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddOtlpExporter(exportBuilder =>
                        {
                            exportBuilder.Protocol = OtlpExportProtocol.Grpc;
                            exportBuilder.Endpoint = new Uri(apmOptions?.TraceUri ?? string.Empty);
                        });

                })
                .WithMetrics((metricsBuilder) =>
                {
                    metricsBuilder
                        .SetResourceBuilder(NotredameResource.Instance)
                        .AddMeter(NotredameResource.ServiceName)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(exportBuilder =>
                        {
                            exportBuilder.Protocol = OtlpExportProtocol.Grpc;
                            exportBuilder.Endpoint = new Uri(apmOptions?.MetricsUri ?? string.Empty);
                        });
                })
                .WithLogging((loggerBuilder) =>
                {
                    loggerBuilder
                        .SetResourceBuilder(NotredameResource.Instance)
                        .AddOtlpExporter(exportBuilder =>
                        {
                            exportBuilder.Protocol = OtlpExportProtocol.Grpc;
                            exportBuilder.Endpoint = new Uri(apmOptions?.LogUri ?? string.Empty);
                        });
                });
        
            services.AddLogging(logger =>
            {
                logger.AddOpenTelemetry(options => options
                    .SetResourceBuilder(NotredameResource.Instance)
                    .AddConsoleExporter());
            });

            return services;
        }
    }
}