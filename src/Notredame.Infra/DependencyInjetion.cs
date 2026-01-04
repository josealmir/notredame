using Microsoft.Extensions.DependencyInjection;

using Notredame.App.Common;
using Notredame.Domain.Commons;
using Notredame.Domain.Repositories;
using Notredame.Infra.Services;
using Notredame.Domain.Services;
using Notredame.Infra.Data;

namespace Notredame.Infra;

public static class DependencyInjection
{
    extension(IServiceCollection services) {
        
        public IServiceCollection AddDiNotredame()
        {
            services.AddScoped<IUnitOfWork, AppDbContext>();
            services.AddScoped<ICepRepository, CepRepository>();
            services.AddScoped<ICepService , CepService>();
            services.AddScoped(typeof(IEnvironmentExecution<>), typeof(EnvironmentLitebus<>));
            return services;
        }
    }
}