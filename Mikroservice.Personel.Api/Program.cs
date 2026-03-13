using Microservice.Personel.Application;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Shared.Extentions;
using Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt;
using Mikroservice.Personel.Persistence.Extentions;
using Mikroservice.Personel.Persistence.Repositories;
using Mikroservice.Personel.Persistence.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(PersonelApplicationAssembly));
builder.Services.AddPersistenceExtentions(builder.Configuration);




var app = builder.Build();
app.AddPersonelGroupEndpointExt();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


app.Run();


