using Microservice.Admin.Clients.BirimClients;
using Microservice.Admin.Clients.DilClients;
using Microservice.Admin.Clients.HaberClients;
using Microservice.Admin.Clients.HedefClients;
using Microservice.Admin.Clients.SiteClients;
using Microservice.Admin.Clients.TemplateClients;
using Microservice.Admin.Clients.MenuClients;
using Microservice.Admin.Clients.UnvanClients;
using Microservice.Admin.Clients.YonetimDuyuruClients;
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



            //Birim Clients
            services.AddRefitClient<IBirimClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Dil Clients
            services.AddRefitClient<IDilClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Hedef Clients
            services.AddRefitClient<IHedefClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Haber Clients
            services.AddRefitClient<IHaberClientService>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için
           
            //Yönetim Duyuru Clients
            services.AddRefitClient<IYonetimDuyuruClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

           //Unvan Clients
           services.AddRefitClient<IUnvanClientServices>()
          .ConfigureHttpClient(c => {

              var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
              c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
          })
          .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()//bu usertoken için istek atarken kullanmak için
          .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

          //Menu Clients
          services.AddRefitClient<IMenuClientServices>()
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
