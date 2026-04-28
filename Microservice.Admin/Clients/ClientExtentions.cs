using Microservice.Admin.Clients.SiteClients;
using Microservice.Admin.Clients.TemplateClients;
using Microservice.Admin.HttpHandlers;
using Microservice.Admin.Settings;
using Refit;

namespace Microservice.Admin.Clients
{
    public static class ClientExtentions
    {
        public static IServiceCollection AddClientExtentions(this IServiceCollection services, IConfiguration configuration)
        {
            //Site Clients
            services.AddRefitClient<ISiteClientServices>()
            .ConfigureHttpClient(c => {

                var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
                c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            })
            .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
            .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Template Clients
             services.AddRefitClient<ITemplateClientService>()
            .ConfigureHttpClient(c => {

                var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
                c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            })
            .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
            .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için






            return services;
        }
    }
}
