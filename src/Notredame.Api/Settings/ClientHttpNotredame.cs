using Notredame.Api.Resiliences;
using Notredame.Shared.Options;

namespace Notredame.Api.Settings;

public static class ClientHttpNotredame
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddHttpClientViaCep(IConfiguration configuration)
        {
            var viacep = configuration
                .GetRequiredSection(ViaCepOptionNotredame.SectionName)
                .Get<ViaCepOptionNotredame>() ??
                         new ViaCepOptionNotredame();
            services.AddHttpClient(ViaCepOptionNotredame.SectionName, client =>
            {
                client.BaseAddress = new Uri(viacep.BaseUrl);
            }).AddResilienceHandler("pipeline-via-cep", (builder, context) =>
            {
                ResiliencyApi.Configure(context, builder);  
            });
            return services;
        }

        
        public IServiceCollection AddHttpClientBrazilCep(IConfiguration configuration)
        {
            var bazilcep = configuration
                .GetRequiredSection(BrasilCepOptionNotredame.SectionName)
                .Get<BrasilCepOptionNotredame>() ?? 
                         new BrasilCepOptionNotredame();
            
            services.AddHttpClient(ViaCepOptionNotredame.SectionName, client =>
            {
                client.BaseAddress = new Uri(bazilcep.BaseUrl);
            }).AddResilienceHandler("pipeline-brazil-cep", (builder, context) =>
            {
                ResiliencyApi.Configure(context, builder);  
            });
            return services;
        }
    }
}