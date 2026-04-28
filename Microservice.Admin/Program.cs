using Microservice.Admin.Clients;
using Microservice.Admin.Configurations;
using Microservice.Admin.HttpHandlers;
using Microservice.Admin.Services.ServicesExtentions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Configuration Ayarları
builder.Services.AddIdentityServerExtentions(builder.Configuration);
builder.Services.AddMicroservicesConfiguration(builder.Configuration); 
builder.Services.AddMinioExtentions(builder.Configuration);
builder.Services.AddRedisExtentions(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//Http handler Ayarları
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();

//Services Ayarları
builder.Services.AddServicesExtentions(builder.Configuration);

//Client Extentions Ayarları
builder.Services.AddClientExtentions(builder.Configuration);


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












var app = builder.Build();



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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();

app.Run();
