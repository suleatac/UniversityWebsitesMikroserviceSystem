using Microservice.Admin.Clients;
using Microservice.Admin.Configurations;
using Microservice.Admin.Filters;
using Microservice.Admin.HttpHandlers;
using Microservice.Admin.Middleware;
using Microservice.Admin.SeriLog;
using Microservice.Admin.Services;
using Microservice.Admin.Services.ServicesExtentions;
using Microservice.Admin.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<AuditLogFilter>();
});


//Configuration Ayarları
builder.Services.AddIdentityServerExtentions(builder.Configuration);
builder.Services.AddMicroservicesConfiguration(builder.Configuration);
builder.Services.AddMinioExtentions(builder.Configuration);
builder.Services.AddRedisExtentions(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Kimlik doğrulama cookie ayarları (appsettings -> AuthCookie)
var authCookieSettings = builder.Configuration
    .GetSection(AuthCookieSetting.Key)
    .Get<AuthCookieSetting>() ?? new AuthCookieSetting();

builder.Services.Configure<AuthCookieSetting>(
    builder.Configuration.GetSection(AuthCookieSetting.Key));

// nginx arkasında çalışıldığı için X-Forwarded-For / X-Forwarded-Proto işlenmeli.
// Aksi halde cookie SecurePolicy ve istemci IP'si yanlış hesaplanır.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

//LDAP Ayarları
builder.Services.Configure<LdapSetting>(
    builder.Configuration.GetSection("LdapSettings"));


//Http handler Ayarları
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();

//Services Ayarları
builder.Services.AddServicesExtentions(builder.Configuration);

//Client Extentions Ayarları
builder.Services.AddClientExtentions(builder.Configuration);

//Logging Extentions Ayarları
builder.Services.AddLoggingExt(builder.Configuration);

//Session Ayarları - Site ve Dil seçimi için
// Not: AddDistributedMemoryCache() yerine Redis kullanılır. Aksi halde uygulama
// yeniden başladığında veya çoklu instance çalıştığında site seçimi kaybolur.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Mikroservice.Admin:session:";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "MikroserviceSiteSelection";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

//Cookie Authentication Ayarları
builder.Services.AddAuthentication(configureOption => {
    configureOption.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    configureOption.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    configureOption.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options => {
        options.LoginPath = "/Auth/SignIn";
        options.LogoutPath = "/Auth/SignOutAsync";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromMinutes(Math.Max(authCookieSettings.ExpireMinutes, 1));
        options.SlidingExpiration = authCookieSettings.SlidingExpiration;

        options.Cookie.Name = authCookieSettings.CookieName;
        options.Cookie.Path = authCookieSettings.CookiePath;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;

        if (Enum.TryParse<SameSiteMode>(authCookieSettings.SameSite, true, out var sameSite))
        {
            options.Cookie.SameSite = sameSite;
        }

        if (Enum.TryParse<CookieSecurePolicy>(authCookieSettings.SecurePolicy, true, out var securePolicy))
        {
            options.Cookie.SecurePolicy = securePolicy;
        }

        // Cookie'yi parçalama yerine tek ve ölçülü tut. Ticket store aktifken cookie
        // içeriği zaten kısa bir anahtardan oluşur.
        options.CookieManager = new ChunkingCookieManager
        {
            ChunkSize = authCookieSettings.ChunkSize > 0 ? authCookieSettings.ChunkSize : 4096,
            ThrowForPartialCookies = false
        };
    });

// Ticket (claim + token) Redis'te saklanır. Böylece access/refresh token'lar cookie'ye
// yazılmaz, cookie 4 KB limitini aşmaz ve nginx 400 (büyük header) hatası oluşmaz.
if (authCookieSettings.EnableTicketStore)
{
    builder.Services.AddSingleton<ITicketStore, RedisTicketStore>();

    builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
        .Configure<ITicketStore>((options, ticketStore) =>
        {
            options.SessionStore = ticketStore;
        });
}

builder.Services.AddAuthorization();

//Log işlemi için eklenen kısım
builder.Host.UseSerilog(Logging.ConfigureLogging);



builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 100L * 1024 * 1024);
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
    o.MultipartBodyLengthLimit = 100L * 1024 * 1024);







var app = builder.Build();

app.UseExceptionMiddleware();
app.UseMiddleware<ObservabilityMiddleware>();

var cultueInfo = new CultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentCulture = cultueInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultueInfo;
app.UseRequestLocalization(new RequestLocalizationOptions {
    DefaultRequestCulture = new RequestCulture(cultueInfo),
    SupportedCultures = new List<CultureInfo> { cultueInfo },
    SupportedUICultures = new List<CultureInfo> { cultueInfo }
});





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();

// X-Forwarded-* header'ları UseAuthentication/UseSession'dan önce işlenmeli.
app.UseForwardedHeaders();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Admin kullanıcılar için site seçimi zorunlu middleware
app.UseMiddleware<SiteSelectionMiddleware>();

app.MapStaticAssets();

// Default route'tan önce eşleşmeli, yoksa /Error -> Error/SignIn'e düşer ve 404 verir
app.MapControllerRoute(
    name: "error",
    pattern: "/Error",
    defaults: new { controller = "Error", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();

app.Run();
