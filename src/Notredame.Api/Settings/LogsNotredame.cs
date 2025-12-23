using Notredame.Shared;

using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Compact;

namespace Notredame.Api.Settings;

public static class LogsNotredame
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLogsNotredame(IConfiguration configuration)
        {
            services.AddSerilog((services, logger) =>
            {
                logger.ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                    .Enrich.WithProperty("ApplicationName", Environment.GetEnvironmentVariable(NotredameResource.ServiceName))
                    .Enrich.WithSpan()
                    .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/hc")))
                    .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/scalar")))
                    .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/swagger")))
                    .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/metric")))
                    .WriteTo.Console(new CompactJsonFormatter())
                    .CreateBootstrapLogger();
            });
            return services;
        }
    }
}