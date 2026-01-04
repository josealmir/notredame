using Microsoft.EntityFrameworkCore;
using Notredame.Infra.Data;

namespace Notredame.Api.Settings;

public static class DbContextNotredame
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDbContextNotredame()
        {
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
            });
            return services;
        }
    }
}