using Microsoft.EntityFrameworkCore;
using Mikroservice.Ogrenci.Api.RecurringJob;
using Mikroservice.Ogrenci.Persistence;

namespace Mikroservice.Ogrenci.Api.SeedDataJob
{
    public static class SeedData
    {
        public static async Task AddSeedDataExt(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Seed sadece ilk çalışmada (db boşsa) çalışsın
            if (await dbContext.Ogrencis.AnyAsync())
            {
                return;
            }

            // Mevcut job mantığını tekrar kullan
            var ogrenciRecurringJob = scope.ServiceProvider.GetRequiredService<OgrenciRecurringJob>();

            // Tüm öğrencileri çek
            const string requestJson = "{'serviceName': 'GetPersonStudents','serviceCriteria':{ } }";

            await ogrenciRecurringJob.IstekGonder(requestJson);
        }
    }
}