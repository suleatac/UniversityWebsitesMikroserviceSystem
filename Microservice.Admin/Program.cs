using Microservice.Admin.Clients;
using Microservice.Admin.Configurations;
using Microservice.Admin.Filters;
using Microservice.Admin.HttpHandlers;
using Microservice.Admin.Middleware;
using Microservice.Admin.SeriLog;
using Microservice.Admin.Services.ServicesExtentions;
using Microservice.Admin.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddLoggingExt(builder.Configuration);
//Session Ayarları - Site ve Dil seçimi için
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "MikroserviceSiteSelection";
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
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.Name = "MikroserviceAuthWebCookie";
        options.AccessDeniedPath = "/Auth/AccessDenied";

    });

builder.Services.AddAuthorization();

//Log işlemi için eklenen kısım
builder.Host.UseSerilog(Logging.ConfigureLogging);










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
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Admin kullanıcılar için site seçimi zorunlu middleware
app.UseMiddleware<SiteSelectionMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();

app.Run();
