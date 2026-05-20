namespace Microservice.Admin.Middleware
{
    /// <summary>
    /// Tüm kimliği doğrulanmış kullanıcılar için site seçimini zorunlu kılan middleware.
    /// Admin kullanıcılar: Site seçilmemişse SelectSite sayfasına yönlendirir.
    /// Admin olmayan kullanıcılar: Site seçilmemişse SelectSite sayfasına yönlendirir.
    /// Admin yetkili işlemler (Template, Birim, Unvan vb.) site seçiminden muaftır.
    /// </summary>
    public class SiteSelectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SiteSelectionMiddleware> _logger;

        public SiteSelectionMiddleware(RequestDelegate next, ILogger<SiteSelectionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Kimliği doğrulanmamış kullanıcılar için kontrolü atla
            if (context.User.Identity?.IsAuthenticated != true)
            {
                await _next(context);
                return;
            }

            // Belirli yolları kontrol dışı tut
            if (IsExcludedPath(path))
            {
                await _next(context);
                return;
            }

            // Statik dosya isteklerini atla
            if (IsStaticFileRequest(path))
            {
                await _next(context);
                return;
            }

            // Admin rolüne sahip kullanıcılar için admin yetkili işlemleri muaf tut
            // Bu işlemler site seçiminden bağımsızdır (site ekleme, birim, kullanıcı yönetimi vb.)
            if (context.User.IsInRole("Admin") && IsAdminOperationPath(path))
            {
                await _next(context);
                return;
            }

            // Session'da site seçimi yapılmış mı kontrol et
            var currentSiteId = context.Session.GetInt32("CurrentSiteId");

            // Site seçimi yapılmamışsa yönlendir
            if (!currentSiteId.HasValue || currentSiteId.Value <= 0)
            {
                // AJAX istekleri için JSON hata döndür
                if (IsAjaxRequest(context.Request))
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"error\": \"Site seçimi gerekli. Lütfen bir site seçin.\", \"redirectUrl\": \"/SiteSelection/SelectSite\"}");
                    return;
                }

                _logger.LogInformation("Kullanıcı site seçimi yapılmadan sayfaya erişmeye çalıştı. Yönlendirme: SiteSelection/SelectSite. Path: {Path}, User: {User}", path, context.User.Identity?.Name ?? "Bilinmeyen");
                context.Response.Redirect("/SiteSelection/SelectSite");
                return;
            }

            await _next(context);
        }

        /// <summary>
        /// Her durumda muaf tutulan yollar (kimlik doğrulama, site seçimi, site yönetimi)
        /// </summary>
        private static bool IsExcludedPath(string path)
        {
            var excludedPrefixes = new[]
            {
                "/Auth",
                "/SiteSelection/",
                "/Site"
            };

            return excludedPrefixes.Any(prefix => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Admin rolüne sahip kullanıcıların site seçiminden muaf olduğu yollar.
        /// Bu işlemler belirli bir siteye bağlı değildir.
        /// </summary>
        private static bool IsAdminOperationPath(string path)
        {
            var adminPaths = new[]
            {
                "/Auth",
                "/Birim",
                "/YonetimDuyuru",
                "/Unvan",
                "/PersonelTip",
                "/User",
                "/YoneticiSite/",
                "/Template/",
                "/Profile"
            };

            return adminPaths.Any(prefix => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsStaticFileRequest(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            var staticFileExtensions = new[]
            {
                ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg",
                ".ico", ".woff", ".woff2", ".ttf", ".eot", ".map",
                ".json", ".xml", ".txt", ".pdf", ".doc", ".docx",
                ".min.css", ".min.js"
            };

            return staticFileExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   request.Headers["Accept"].ToString().Contains("application/json");
        }
    }
}