using System.Text.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

using FluentValidation;

using Scalar.AspNetCore;
using Notredame.Infra;
using Notredame.Shared;
using Notredame.Api.Settings;
using Serilog;
using Mapster;
using MapsterMapper;

using Notredame.App;

using Prometheus;

using Serilog.Enrichers.Span;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);
/*
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
        .Enrich.WithProperty("ApplicationName", NotredameResource.ServiceName)
        .Enrich.WithSpan()
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/openapi")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/hc")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/scalar")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/swagger")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/metric")))
        .WriteTo.Console(new CompactJsonFormatter())
        .Enrich.FromLogContext() ;
});
*/
// Add services to the container.

builder.Services
       .AddOpenApi()
       .AddLocalization()
       .AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.WriteIndented = true;
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
       });

builder.Services
     .AddEndpointsApiExplorer();

builder.Services
     .AddHealthChecks();

builder.Services
    .AddHeaderPropagation()
    .AddHttpContextAccessor()
    .AddHttpClientViaCep(builder.Configuration)
    .AddHttpClientBrazilCep(builder.Configuration);

builder.Services
    .AddMapster();

builder.Services
    .AddValidatorsFromAssemblyContaining<AssemblyScanApp>();

builder.Services
        .AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Extensions["traceId"] = Activity.Current?.TraceId.ToString();
                context.ProblemDetails.Instance = context.HttpContext.Request.Path;
            };
        });

builder.Services.AddDiNotredame();

builder.Services
    .AddApiVersionNotredame()
    .AddDbContextNotredame()
    .AddLitebusNotredame()
    .AddOptionsNotredame()
    .AddCorsNotredame(builder.Configuration)
    .UseHttpClientMetrics();

builder.UseApmNotredame();
var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsProduction())
{
    app.MapOpenApi()
       .AllowAnonymous();
    app.MapScalarApiReference();
    app.MapGet("/", () => "/scalar");
}
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseHeaderPropagation();

app.UseRequestLocalization();

app.UseRouting();
app.UseCors();

app.UseStatusCodePages();

// app.UseSerilogRequestLogging();
app.UseMetricServer();
app.MapPrometheusScrapingEndpoint();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/hc");

app.MapControllers();
app.MapMetrics();

app.Run();
