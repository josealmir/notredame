using Notredame.Infra.Data;
using Notredame.Infra.Services;
using Notredame.Domain.Services;
using Notredame.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Notredame.Infra;

public static class DependencyInjection
{
    extension(IServiceCollection services) {
        
        public IServiceCollection AddDiNotredame()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICepService , CepService>();
            services.AddScoped<ICepRepository, CepRepository>();
            return services;
        }
    }
}