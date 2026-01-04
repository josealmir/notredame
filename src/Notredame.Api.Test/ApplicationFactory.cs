using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notredame.Api.Test.DataTest;
using Notredame.Domain.Services;

namespace Notredame.Api.Test;

public class ApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services=>
        {
            services.RemoveAll<ICepService>();
            services.AddSingleton(SetupCepService.CepService);
        });
    }
}