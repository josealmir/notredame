using Notredame.Shared.Options;

namespace Notredame.Api.Settings;

public static class OptionsNotredame
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddOptionsNotredame()
        {            
            services.AddOptions<BrasilCepOptionNotredame>()
                .BindConfiguration(BrasilCepOptionNotredame.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.AddOptions<ViaCepOptionNotredame>()
                .BindConfiguration(ViaCepOptionNotredame.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            return services;
        }
    }
}
