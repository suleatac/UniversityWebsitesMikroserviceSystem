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
        private readonly IHaberClientServices _haberClient;
        private readonly IDuyuruClientServices _duyuruClient;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<RouteService> _logger;

        public RouteService(
            IPageTypeClientServices pageClient,
            ISiteService siteService,
            IHaberClientServices haberClient,
            IDuyuruClientServices duyuruClient,
            IRedisCacheService redisCacheService,
            ILogger<RouteService> logger)
        {
            _pageClient = pageClient
                ?? throw new ArgumentNullException(nameof(pageClient));

            _siteService = siteService
                ?? throw new ArgumentNullException(nameof(siteService));

            _haberClient = haberClient
                ?? throw new ArgumentNullException(nameof(haberClient));

            _duyuruClient = duyuruClient
                ?? throw new ArgumentNullException(nameof(duyuruClient));

            _redisCacheService = redisCacheService
                ?? throw new ArgumentNullException(nameof(redisCacheService));

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

            var siteResult = await _siteService
                .GetSiteByHostAsync(host);

            if (siteResult.IsFail || siteResult.Data is null)
            {
                _logger.LogWarning(
                    "Host için site bulunamadı. Host: {Host}",
                    host);

                //return null;
                var result2 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "Host için site bulunamadı."
                };
                return result2;
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

                //return null;
                var result3 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "Desteklenmeyen dil kodu.."
                };
                return result3;
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
                    site.Id,
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
                site.Id,
                languageId.Value,
                pageSlug);

            if (page is null)
            {
                var result4 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "Page null geldi"
                };
                return result4;
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
                return result;
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
                var result4 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "Geçersiz URL yapısı."
                };
                return result4;
                //return null;
            }

            var detailSlug = Normalize(segments[2]);

            if (string.IsNullOrWhiteSpace(detailSlug))
            {
                var result5 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "detailSlug hatası."
                };
                return result5;

                //return null;
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

            //var dilResult = await _dilService
            //   .GetDilByIdAsync(defaultLanguageId);

            //if (!dilResult.IsSuccess || dilResult.Data is null)
            //{
            //    _logger.LogWarning(
            //        "Dil bulunamadı. defaultLanguageId: {DefaultLanguageId}",
            //        defaultLanguageId);

            //    return null;
            //}

            string defaultLanguageCode = site.DefaultLanguage.Kod.ToLower();

            var response = await _pageClient
                .HomePageControlAsync(
                    site.Id,
                    defaultLanguageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Ana sayfa bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    (int)site.Id,
                    defaultLanguageId);


                var result5 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "Default Ana sayfa bulunamadı."
                };
                return result5;

                //return null;
            }

            return new RouteResolveResult {
                Page = response.Content,
                LanguageId = defaultLanguageId,
                LanguageCode = defaultLanguageCode
            };
        }
        private async Task<RouteResolveResult?> ResolveHomePageAsync(
            int siteId,
            int languageId,
            string languageCode)
        {
            var response = await _pageClient
                .HomePageControlAsync(
                    siteId,
                    languageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Ana sayfa bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    siteId,
                    languageId);
                var result5 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "Ana sayfa bulunamadı."
                };
                return result5;

                //return null;
           
            }

            return new RouteResolveResult {
                Page = response.Content,
                LanguageId = languageId,
                LanguageCode = languageCode
            };
        }

        // =============================================================
        // PAGE
        // =============================================================

        private async Task<PagesDetailVm?> GetPageAsync(
            int siteId,
            int languageId,
            string slug)
        {
            var response = await _pageClient
                .GetPagesBySlugAsync(
                    siteId,
                    languageId,
                    slug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Page bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, Slug: {Slug}",
                    siteId,
                    languageId,
                    slug);

           

                //return null;
                return null;
            }

            return response.Content;
        }

        // =============================================================
        // DETAIL
        // =============================================================

        private async Task<RouteResolveResult?> ResolveDetailAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var pageType = (PageType)result.Page.PageTypeId;

            return pageType switch {
                PageType.NewsList =>
                    await ResolveNewDetailAsync(
                        result,
                        detailSlug),

                PageType.AnnouncementList =>
                    await ResolveAnnouncementDetailAsync(
                        result,
                        detailSlug),

                _ => null
            };
        }

        // =============================================================
        // NEWS
        // =============================================================

        private async Task<RouteResolveResult?> ResolveNewDetailAsync(RouteResolveResult result,string detailSlug)
        {
            var response = await _haberClient
                .GetHaberBySeoUrlAsync(
                    result.Page.SiteId,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Haber bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Page.SiteId,
                    result.LanguageId,
                    detailSlug);

                var result5 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "haber bulunamadı."
                };
                return result5;
            }

            result.New = response.Content;

            return result;
        }

        // =============================================================
        // ANNOUNCEMENT
        // =============================================================

        private async Task<RouteResolveResult?> ResolveAnnouncementDetailAsync(RouteResolveResult result,string detailSlug)
        {
            var response = await _duyuruClient.GetDuyuruBySeoUrlAsync(
                    result.Page.SiteId,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Duyuru bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Page.SiteId,
                    result.LanguageId,
                    detailSlug);

                var result5 = new RouteResolveResult {
                    Page = null,
                    LanguageId = 1,
                    LanguageCode = "duyuru bulunamadı."
                };
                return result5;
            }

            result.Announcement = response.Content;

            return result;
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