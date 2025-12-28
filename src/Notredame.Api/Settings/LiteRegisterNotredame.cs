using LiteBus.Commands;
using LiteBus.Events;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Notredame.App;

namespace Notredame.Api.Settings;

public static class LiteRegisterNotredame
{
    extension(IServiceCollection services)
    {

        public IServiceCollection AddLitebusNotredame()
        {
            services.AddLiteBus(liteBus =>
            {
                var assemblyScanApp = typeof(AssemblyScanApp).Assembly;
                liteBus.AddCommandModule(module => module.RegisterFromAssembly(assemblyScanApp));
                liteBus.AddEventModule(module => module.RegisterFromAssembly(assemblyScanApp));
                liteBus.AddQueryModule(module => module.RegisterFromAssembly(assemblyScanApp));
            });
            return services;
        }
    }
}