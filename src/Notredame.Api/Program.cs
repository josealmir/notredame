using System.Diagnostics;
using Notredame.Api.Settings;
using Notredame.Shared;
using Serilog;
using Scalar.AspNetCore;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Compact;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
        .Enrich.WithProperty("ApplicationName", Environment.GetEnvironmentVariable(NotredameResource.ServiceName))
        .Enrich.WithSpan()
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/hc")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/scalar")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/swagger")))
        .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().StartsWith("/metric")))
        .WriteTo.Console(new CompactJsonFormatter())
        .Enrich.FromLogContext();
});

// Add services to the container.

builder.Services
       .AddOpenApi()
       .AddLocalization()
       .AddControllers();

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
        .AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Extensions["traceId"] = Activity.Current?.TraceId.ToString();
                context.ProblemDetails.Instance = context.HttpContext.Request.Path;
            };
        });

builder.Services
       .AddApiVersionNotredame();

builder.Services
    .AddOptionsNotredame()
    .AddApmNotredame(builder.Configuration)
    .AddCorsNotredame(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsProduction())
{
    app.MapOpenApi()
       .AllowAnonymous();
    app.MapScalarApiReference();
    app.MapGet("/", () => "/scalar");
}

app.UseHttpsRedirection();

app.UseHeaderPropagation();

app.UseRequestLocalization();

app.UseRouting();
app.UseCors();

app.UseStatusCodePages();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/hc");

app.MapControllers();

app.Run();
