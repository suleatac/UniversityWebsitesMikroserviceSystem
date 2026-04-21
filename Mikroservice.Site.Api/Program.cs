using Logging.Shared;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.SeriLog;
using Microservice.Site.Api.Endpoints.YoneticiTipiEndPoints;
using Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints;
using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Extentions;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Api.Endpoints.BandLogoEndPoints;
using Mikroservice.Site.Api.Endpoints.BannerEndPoints;
using Mikroservice.Site.Api.Endpoints.BilgiEndPoints;
using Mikroservice.Site.Api.Endpoints.BirimEndPoints;
using Mikroservice.Site.Api.Endpoints.DilEndPoints;
using Mikroservice.Site.Api.Endpoints.DuyuruEndPoints;
using Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints;
using Mikroservice.Site.Api.Endpoints.HaberEndPoints;
using Mikroservice.Site.Api.Endpoints.HedefEndPoints;
using Mikroservice.Site.Api.Endpoints.MediaFileEndPoints;
using Mikroservice.Site.Api.Endpoints.MenuEndPoints;
using Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints;
using Mikroservice.Site.Api.Endpoints.PopupEndPoints;
using Mikroservice.Site.Api.Endpoints.SertifikaParmakIziEndPoints;
using Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints;
using Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints;
using Mikroservice.Site.Api.Endpoints.SiteEndPoints;
using Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints;
using Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints;
using Mikroservice.Site.Api.Endpoints.TemplateEndPoints;
using Mikroservice.Site.Api.Endpoints.UnvanEndPoints;
using Mikroservice.Site.Api.Endpoints.VideoEndPoints;
using Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints;
using Mikroservice.Site.Api.SeedDataInitializers;
using Mikroservice.Site.Application;
using Mikroservice.Site.Persistence.Messaging.RabbitmqExtentions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Swagger ayarları
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Authentication ve Authorization servisleri eklendi
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

//Trace işlemi için eklenen extentionlar
builder.Services.AddOpenTelemetryTraceExt(builder.Configuration);

//Log işlemi için eklenen kısım
builder.Host.UseSerilog(Microservice.Shared.SeriLog.Logging.ConfigureLogging);

//Persistence işlemleri için eklenen extention
builder.Services.AddPersistenceExtentions(builder.Configuration);

//MediatR, AutoMapper, FluentValidation işlemleri için eklenen common extention
builder.Services.AddCommonServiceExt(typeof(ApplicationAssembly));

//Redis cache ekleme işlemi için eklenen extention
builder.Services.AddRedisCacheExt(builder.Configuration);

//Rabbitmq message sistemi için eklenen extention
builder.Services.AddRabbitmqExtentions(builder.Configuration);

//Versiyonlama eklendi
builder.Services.AddVersioningExt();





var app = builder.Build();

//Çalıştığında otomatik migration yapması için
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}


//Authentication ve Authorization middleware'leri eklendi
app.UseAuthentication();
app.UseAuthorization();


app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseExceptionMiddleware();
//Metric işlemi için eklenen middleware
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

//Seed işlemi için eklenen middleware
await app.InitializeSeedDataAsync();

//Endpointler eklendi
var apiVersionSet = app.AddVersionSetExt();

app.AddBandLogoGroupsEndpointExt(apiVersionSet);
app.AddBannerGroupsEndpointExt(apiVersionSet);
app.AddBilgiGroupsEndpointExt(apiVersionSet);
app.AddBirimGroupsEndpointExt(apiVersionSet);
app.AddDilGroupsEndpointExt(apiVersionSet);
app.AddDuyuruGroupsEndpointExt(apiVersionSet);
app.AddEtkinlikGroupsEndpointExt(apiVersionSet);
app.AddHaberGroupsEndpointExt(apiVersionSet);
app.AddHedefGroupsEndpointExt(apiVersionSet);
app.AddMediaFileGroupsEndpointExt(apiVersionSet);
app.AddMenuGroupsEndpointExt(apiVersionSet);
app.AddPersonelTipGroupsEndpointExt(apiVersionSet);
app.AddPopupGroupsEndpointExt(apiVersionSet);
app.AddSertifikaParmakIziGroupsEndpointExt(apiVersionSet);
app.AddSikcaSorulanSoruGroupsEndpointExt(apiVersionSet);
app.AddSikcaSorulanSoruKategoriGroupsEndpointExt(apiVersionSet);
app.AddSiteGroupsEndpointExt(apiVersionSet);
app.AddSiteOzellikleriGroupsEndpointExt(apiVersionSet);
app.AddSitePersonelGroupsEndpointExt(apiVersionSet);
app.AddTemplateGroupsEndpointExt(apiVersionSet);
app.AddUnvanGroupsEndpointExt(apiVersionSet);
app.AddVideoGroupsEndpointExt(apiVersionSet);
app.AddYoneticiSiteGroupsEndpointExt(apiVersionSet);
app.AddYoneticiTipiGroupsEndpointExt(apiVersionSet);
app.AddYonetimDuyuruGroupsEndpointExt(apiVersionSet);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}



app.Run();




