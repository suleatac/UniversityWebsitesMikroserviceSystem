using Microservice.Web.Clients;
using Microservice.Web.Configurations;
using Microservice.Web.HttpHandlers;
using Microservice.Web.Services.ServicesExtentions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Configuration Ayarları
builder.Services.AddIdentityServerExtentions(builder.Configuration);
builder.Services.AddMicroservicesConfiguration(builder.Configuration);
builder.Services.AddRedisExtentions(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


//Http handler Ayarları
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();

//Services Ayarları
builder.Services.AddServicesExtentions(builder.Configuration);

//Client Extentions Ayarları
builder.Services.AddClientExtentions(builder.Configuration);




var app = builder.Build();

//app.UseExceptionMiddleware();

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
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();



app.MapStaticAssets();

// catch-all'dan önce eşleşmeli, yoksa her zaman Template/Index'e düşer
app.MapControllerRoute(
    name: "error",
    pattern: "/Error",
    defaults: new {
        controller = "Template",
        action = "Error"
    });

app.MapControllerRoute(
    name: "template",
    pattern: "{*path}",
    defaults: new {
        controller = "Template",
        action = "Index"
    });

app.Run();
