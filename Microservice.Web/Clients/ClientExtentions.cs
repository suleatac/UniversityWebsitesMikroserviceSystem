using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Clients.SiteClients;
using Microservice.Web.HttpHandlers;
using Microservice.Web.Settings;
using Refit;

namespace Microservice.Web.Clients
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
            .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Haber Clients
            services.AddRefitClient<IHaberClientService>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için


            // //Template Clients
            // services.AddRefitClient<ITemplateClientService>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için



            // //Birim Clients
            // services.AddRefitClient<IBirimClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Dil Clients
            // services.AddRefitClient<IDilClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Hedef Clients
            // services.AddRefitClient<IHedefClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Yönetim Duyuru Clients
            // services.AddRefitClient<IYonetimDuyuruClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Unvan Clients
            // services.AddRefitClient<IUnvanClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Menu Clients
            // services.AddRefitClient<IMenuClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //PersonelTip Clients
            // services.AddRefitClient<IPersonelTipClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Duyuru Clients
            // services.AddRefitClient<IDuyuruClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Etkinlik Clients
            // services.AddRefitClient<IEtkinlikClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Video Clients
            // services.AddRefitClient<IVideoClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //SikcaSorulanSoru Clients
            // services.AddRefitClient<ISikcaSorulanSoruClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için


            // //SitePersonel Clients
            // services.AddRefitClient<ISitePersonelClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Bilgi Clients
            // services.AddRefitClient<IBilgiClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Banner Clients
            // services.AddRefitClient<IBannerClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //Popup Clients
            // services.AddRefitClient<IPopupClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            // //SiteOzellikleri Clients
            // services.AddRefitClient<ISiteOzellikleriClientServices>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için


            // //TümPersonel Clients
            // services.AddRefitClient<ITumPersonelClientService>()
            //.ConfigureHttpClient(c => {

            //    var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
            //    c.BaseAddress = new Uri(microserviceOption!.Personel.BaseUrl);
            //})
            //.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için



            return services;
        }

    }
}
