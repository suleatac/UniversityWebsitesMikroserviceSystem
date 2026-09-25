using Microservice.Web.Clients.BandLogoClients;
using Microservice.Web.Clients.BannerClients;
using Microservice.Web.Clients.BilgiClients;
using Microservice.Web.Clients.DilClients;
using Microservice.Web.Clients.GaleriResimClients;
using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Clients.EtkinlikClients;
using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Clients.IcerikClients;
using Microservice.Web.Clients.IletisimClients;
using Microservice.Web.Clients.MenuClients;
using Microservice.Web.Clients.OgrenciClients;
using Microservice.Web.Clients.PageTypeClients;
using Microservice.Web.Clients.PopupClients;
using Microservice.Web.Clients.ShortcutButtonClients;
using Microservice.Web.Clients.SikcaSorulanSoruClients;
using Microservice.Web.Clients.SiteClients;
using Microservice.Web.Clients.SitePersonelClients;
using Microservice.Web.Clients.VideoClients;
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
            services.AddRefitClient<IHaberClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Banner Clients
            services.AddRefitClient<IBannerClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
            
            //BandLogo Clients
            services.AddRefitClient<IBandLogoClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
            //BandLogo Clients
            services.AddRefitClient<ISitePersonelClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Bilgi Clients
            services.AddRefitClient<IBilgiClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Duyuru Clients
            services.AddRefitClient<IDuyuruClientServices>()
               .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
               .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Etkinlik Clients
            services.AddRefitClient<IEtkinlikClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Video Clients
            services.AddRefitClient<IVideoClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //ShortcutButton Clients
            services.AddRefitClient<IShortcutButtonClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //GaleriResim Clients
            services.AddRefitClient<IGaleriResimClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Dil Clients
            services.AddRefitClient<IDilClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Menu Clients
            services.AddRefitClient<IMenuClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //PageType Clients
            services.AddRefitClient<IPageTypeClientServices>()
           .ConfigureHttpClient(c => {

               var microserviceOption = configuration.GetSection(MicroservicesSetting.SectionName).Get<MicroservicesSetting>();
               c.BaseAddress = new Uri(microserviceOption!.Site.BaseUrl);
           })
           .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();//bu clientcredential için token alıp istek göndermek için

            //Icerik (arama) Clients
            services.AddRefitClient<IIcerikClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Iletisim Clients
            services.AddRefitClient<IIletisimClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Popup Clients (Site API)
            services.AddRefitClient<IPopupClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //Ogrenci Clients (Ogrenci API; sadece sayaclar icin kullanilir)
            services.AddRefitClient<IOgrenciClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetOgrenciBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

            //SikcaSorulanSoru Clients (Site API; SSS sayfasi icin)
            services.AddRefitClient<ISikcaSorulanSoruClientServices>()
                .ConfigureHttpClient(c => c.BaseAddress = GetSiteBaseAddress(configuration))
                .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();



            return services;
        }

        private static Uri GetSiteBaseAddress(IConfiguration configuration)
        {
            var options = configuration.GetSection(MicroservicesSetting.SectionName)
                .Get<MicroservicesSetting>();

            return new Uri(options!.Site.BaseUrl);
        }

        private static Uri GetOgrenciBaseAddress(IConfiguration configuration)
        {
            var options = configuration.GetSection(MicroservicesSetting.SectionName)
                .Get<MicroservicesSetting>();

            return new Uri(options!.Ogrenci.BaseUrl);
        }

    }
}
