using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Notredame.Api.Settings;
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
            services.AddLitebusNotredame();
        });
    }
}