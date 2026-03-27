using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microservice.Personel.Application;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Personel.Api;
using Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt;
using Mikroservice.Personel.Api.RecurringJob;
using Mikroservice.Personel.Persistence;
using Mikroservice.Personel.Persistence.Extentions;

var builder = WebApplication.CreateBuilder(args);
//Authentication ve Authorization servisleri eklendi
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(PersonelApplicationAssembly));
builder.Services.AddPersistenceExtentions(builder.Configuration);
builder.Services.AddRedisCacheExt(builder.Configuration);
//Hangfire ayarları
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c => {
        var connectionToString = builder.Configuration.GetSection(HangFireConnectionToString.Key).Get<HangFireConnectionToString>();

        c.UseNpgsqlConnection(connectionToString!.HangfirePostgreSqlServer);

    }));

builder.Services.AddHangfireServer();

//Trace işlemi için eklenen extentionlar
builder.Services.AddOpenTelemetryExt(builder.Configuration);


//Versiyonlama eklendi
builder.Services.AddVersioningExt();



builder.Services.AddHttpClient();
builder.Services.AddScoped<PersonelRecurringJob>();

var app = builder.Build();

//çalıştığında otomatik migration yapması için
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.AddPersonelGroupEndpointExt(app.AddVersionSetExt());
app.UseHangfireDashboard("/hangfire", new DashboardOptions {
    Authorization = new[] { new AllowAll() }
});
PersonelRecurringJob.VeriTabaniGuncellemeJob();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


app.Run();


