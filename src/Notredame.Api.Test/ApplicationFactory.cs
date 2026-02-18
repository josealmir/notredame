using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Notredame.Api.Settings;
using Notredame.Api.Test.MockService;
using Notredame.Domain.Services;
using Notredame.Infra;

namespace Notredame.Api.Test;

public class ApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services=>
        {
            services.AddDiNotredame();
            services.RemoveAll<ICepService>();
            services.AddLitebusNotredame();
            
            services.AddScoped<ICepService, TestCepService>();
        });
    }
}