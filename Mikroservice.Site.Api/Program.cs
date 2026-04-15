using Logging.Shared;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.SeriLog;
using Microservice.Site.Api.Endpoints.YoneticiDuyuruEndPoints;
using Microservice.Site.Api.Endpoints.YoneticiTipiEndPoints;
using Microservice.Site.Persistence;
using Microservice.Site.Persistence.Extentions;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Api.SeedDataInitializers;
using Mikroservice.Site.Application;
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
app.AddYoneticiDuyuruGroupsEndpointExt(apiVersionSet);
app.AddYoneticiTipiGroupsEndpointExt(apiVersionSet);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}



app.Run();




