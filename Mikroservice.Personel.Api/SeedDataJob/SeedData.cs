using Microsoft.EntityFrameworkCore;
using Mikroservice.Personel.Api.RecurringJob;
using Mikroservice.Personel.Persistence;

namespace Mikroservice.Personel.Api.SeedDataJob
{
    public static class SeedData
    {
        public static async Task AddSeedDataExt(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Seed sadece ilk çalışmada (db boşsa) çalışsın
            if (await dbContext.Personels.AnyAsync())
            {
                return;
            }

            // Mevcut job mantığını tekrar kullan
            var personelRecurringJob = scope.ServiceProvider.GetRequiredService<PersonelRecurringJob>();

            // Tüm personelleri çek
            const string requestJson = "{'serviceName': 'GetWorkers','serviceCriteria':{  'GetPersonEncryptedId':true} }";

            await personelRecurringJob.IstekGonder(requestJson);
        }
    }
}