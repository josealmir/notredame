namespace Notredame.Api.Settings;

public static class CorsNotredame
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCorsNotredame(IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var origins = configuration.GetRequiredSection("HostClient").Get<IEnumerable<string>>()?.ToArray() ?? Enumerable.Empty<string>().ToArray();
                    policy.WithOrigins(origins)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(_ => true)
                        .Build();
                });
            });    
            return services;
        }
    }
}