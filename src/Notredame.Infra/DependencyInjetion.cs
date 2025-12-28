using Microsoft.Extensions.DependencyInjection;

using Notredame.Infra.Services;
using Notredame.Domain.Services;

namespace Notredame.Data;

public static class DependencyInjection
{
    extension(IServiceCollection services) {
        
        public IServiceCollection AddDiNotredame()
        {
            services.AddScoped<ICepService , CepService>();
            return services;
        }
    }
}