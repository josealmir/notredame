using Microsoft.EntityFrameworkCore;

using Notredame.Infra.Data;

namespace Notredame.Api.Builders;

public static class MigrationBuilder
{
    extension(IApplicationBuilder app)
    {
        public void UseApplyMigrations()
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            ctx.Database.Migrate();
        }
    }
}