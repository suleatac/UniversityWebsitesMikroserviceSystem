using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Clients.PageTypeClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;
using Microservice.Web.ViewModels.Pages;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.Services
{
    public class RouteService : IRouteService
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        private readonly IPageTypeClientServices _pageClient;
        private readonly ISiteService _siteService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<RouteService> _logger;

        private readonly IEnumerable<IPageDetailResolver> _detailResolvers;

        private readonly IEnumerable<IPageResolver> _pageResolvers;




        public RouteService(
            IPageTypeClientServices pageClient,
            ISiteService siteService,
            IHaberClientServices haberClient,
            IDuyuruClientServices duyuruClient,
            IRedisCacheService redisCacheService,
            IEnumerable<IPageDetailResolver> detailResolvers,
            IEnumerable<IPageResolver> pageResolvers,
            ILogger<RouteService> logger)
        {
            _pageClient = pageClient
                ?? throw new ArgumentNullException(nameof(pageClient));

            _siteService = siteService
                ?? throw new ArgumentNullException(nameof(siteService));
            _pageResolvers = pageResolvers
                            ?? throw new ArgumentNullException(nameof(pageResolvers));

            _redisCacheService = redisCacheService
                ?? throw new ArgumentNullException(nameof(redisCacheService));

            _detailResolvers = detailResolvers
                ?? throw new ArgumentNullException(nameof(detailResolvers));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            string host,
            string path)
        {
            var cacheKey = $"route:{host.ToLowerInvariant()}:{path.Trim('/').ToLowerInvariant()}";

            var cached = await _redisCacheService.GetAsync<RouteResolveResult>(cacheKey);
            if (cached is not null)
            {
                _logger.LogInformation("Route cache'den alındı. Host: {Host}, Path: {Path}", host, path);
                return cached;
            }

            var result = await ResolveInternalAsync(host, path);

            if (result is not null)
            {
                await _redisCacheService.SetAsync(cacheKey, result, CacheDuration);
            }

            return result;
        }

        private async Task<RouteResolveResult?> ResolveInternalAsync(
            string host,
            string path)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return null;
            }

            // =========================================================
            // 1. SITE
            // =========================================================

            var siteResult = await _siteService.GetSiteByHostAsync(host);

            if (siteResult.IsFail || siteResult.Data is null)
            {
                _logger.LogWarning(
                    "Host için site bulunamadı. Host: {Host}",
                    host);

                return null;
            }

            var site = siteResult.Data;

            // =========================================================
            // 2. URL SEGMENTLERİ
            // =========================================================

            var segments = GetPathSegments(path);

            if (segments.Length == 0)
            {
                return await ResolveDefaultHomePageAsync(site);
            }

            // =========================================================
            // 3. LANGUAGE
            //
            // /tr
            // /en
            // =========================================================

            var languageCode = Normalize(segments[0]);

            var languageId = GetLanguageId(languageCode);

            if (languageId is null)
            {
                _logger.LogWarning(
                    "Desteklenmeyen dil kodu. Host: {Host}, Language: {Language}",
                    host,
                    languageCode);

                return null;
            }

            // =========================================================
            // 4. SADECE /tr veya /en
            //
            // /tr
            // /en
            //
            // Ana sayfa
            // =========================================================

            if (segments.Length == 1)
            {
                return await ResolveHomePageAsync(
                    site,
                    site.TemplateId,
                    languageId.Value,
                    languageCode);
            }

            // =========================================================
            // 5. PAGE SLUG
            //
            // /tr/haberler
            // /en/news
            // =========================================================

            var pageSlug = Normalize(segments[1]);

            var page = await GetPageAsync(
                site.TemplateId,
                languageId.Value,
                pageSlug);

            if (page is null)
            {
                return null;
            }

            var result = new RouteResolveResult {
                Page = page,
                LanguageId = languageId.Value,
                LanguageCode = languageCode
            };

            // =========================================================
            // 6. SADECE PAGE
            //
            // /tr/haberler
            // /en/news
            // =========================================================

            if (segments.Length == 2)
            {
                return await ResolvePageAsync(result);
            }

            // =========================================================
            // 7. DETAIL
            //
            // /tr/haberler/haber-slug
            // /en/news/news-slug
            // =========================================================

            if (segments.Length != 3)
            {
                _logger.LogWarning(
                    "Geçersiz URL yapısı. Host: {Host}, Path: {Path}",
                    host,
                    path);

                return null;
            }

            var detailSlug = Normalize(segments[2]);

            if (string.IsNullOrWhiteSpace(detailSlug))
            {
                return null;
            }

            result.DetailSlug = detailSlug;

            // =========================================================
            // 8. DETAIL CONTENT
            // =========================================================

            return await ResolveDetailAsync(
                result,
                detailSlug);
        }

        // =============================================================
        // HOME PAGE
        // =============================================================
        private async Task<RouteResolveResult?> ResolveDefaultHomePageAsync(SiteDetailGetVm site)
        {
            var defaultLanguageId = site.DefaultLanguageId;

            string defaultLanguageCode = site.DefaultLanguage.Kod.ToLower();

            var response = await _pageClient.HomePageControlAsync(site.TemplateId,defaultLanguageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Ana sayfa bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    (int)site.Id,
                    defaultLanguageId);

                return null;
            }

            return new RouteResolveResult {
                Site = site,
                Page = response.Content,
                LanguageId = defaultLanguageId,
                LanguageCode = defaultLanguageCode
            };
        }
        private async Task<RouteResolveResult?> ResolveHomePageAsync(
            SiteDetailGetVm site,
            int siteTemplateId,
            int languageId,
            string languageCode)
        {
            var response = await _pageClient.HomePageControlAsync(siteTemplateId,languageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Ana sayfa bulunamadı. SiteTemplateId: {SiteTemplateId}, LanguageId: {LanguageId}",
                    siteTemplateId,
                    languageId);

                return null;
            }

            return new RouteResolveResult {
                Site = site,
                Page = response.Content,
                LanguageId = languageId,
                LanguageCode = languageCode
            };
        }

        // =============================================================
        // PAGE
        // =============================================================

        private async Task<PagesDetailVm?> GetPageAsync(
            int siteTemplateId,
            int languageId,
            string slug)
        {
            var response = await _pageClient
                .GetPagesBySlugAsync(
                    siteTemplateId,
                    languageId,
                    slug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Page bulunamadı. SiteTemplateId: {SiteTemplateId}, LanguageId: {LanguageId}, Slug: {Slug}",
                    siteTemplateId,
                    languageId,
                    slug);

                return null;
            }

            return response.Content;
        }


        private async Task<RouteResolveResult?> ResolvePageAsync(
    RouteResolveResult result)
        {
            var pageType = result.Page.PageTypeKind;

            var resolver = _pageResolvers
                .FirstOrDefault(x => x.CanResolve(pageType));

            if (resolver is null)
            {
                // Resolver gerektirmeyen normal sayfa olabilir.
                return result;
            }

            await resolver.ResolveAsync(result);

            return result;
        }




        // =============================================================
        // DETAIL
        // =============================================================

        private async Task<RouteResolveResult?> ResolveDetailAsync(
     RouteResolveResult result,
     string detailSlug)
        {
            var pageType = result.Page.PageTypeKind;

            var resolver = _detailResolvers
                .FirstOrDefault(x => x.CanResolve(pageType));

            if (resolver is null)
            {
                _logger.LogWarning(
                    "Detail resolver bulunamadı. PageType: {PageType}, DetailSlug: {DetailSlug}",
                    pageType,
                    detailSlug);

                return null;
            }

            return await resolver.ResolveAsync(
                result,
                detailSlug);
        }

       

        // =============================================================
        // LANGUAGE
        // =============================================================

        private static int? GetLanguageId(string languageCode)
        {
            return languageCode switch {
                "tr" => 1,
                "en" => 2,
                _ => null
            };
        }

        // =============================================================
        // URL
        // =============================================================

        private static string[] GetPathSegments(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Array.Empty<string>();
            }

            return path
                .Trim('/')
                .Split(
                    '/',
                    StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return Uri.UnescapeDataString(
                value
                    .Trim()
                    .Trim('/')
                    .ToLowerInvariant());
        }
    }
}