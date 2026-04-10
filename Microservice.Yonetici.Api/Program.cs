using Microservice.Yonetici.Api.SeedDataInitializers;
using Microservice.Yonetici.Application.Contracts.Services;
using Microservice.Yonetici.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IYoneticiTipiSeedService, YoneticiTipiSeedService>();
var app = builder.Build();
await app.InitializeYoneticiTipiSeedDataAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

