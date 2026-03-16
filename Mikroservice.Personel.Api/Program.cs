using Hangfire;
using Hangfire.PostgreSql;
using Microservice.Personel.Application;
using Microservice.Shared.Extentions;
using Mikroservice.Personel.Api;
using Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt;
using Mikroservice.Personel.Persistence.Extentions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(PersonelApplicationAssembly));
builder.Services.AddPersistenceExtentions(builder.Configuration);

//Hangfire ayarları
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c => {
        var connectionToString = builder.Configuration.GetSection(HangFireConnectionToString.Key).Get<HangFireConnectionToString>();

        c.UseNpgsqlConnection(connectionToString!.HangfirePostgreSqlServer);
       
    }));

builder.Services.AddHangfireServer();

builder.Services.AddHttpClient();
builder.Services.AddScoped<PersonelRecurringJob>();

var app = builder.Build();
app.AddPersonelGroupEndpointExt();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire");
    PersonelRecurringJob.VeriTabaniGuncellemeJob();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


app.Run();


