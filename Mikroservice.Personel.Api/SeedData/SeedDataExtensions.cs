using Mikroservice.Personel.Application.Services;

namespace Mikroservice.Personel.Api.SeedData
{
    public static class SeedDataExtensions
    {
        public static async Task InitializeSeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var seedService = scope.ServiceProvider.GetRequiredService<PersonelSeedService>();

            try
            {
         
            
                logger.LogInformation("Seed data kontrol ediliyor...");

                if (await seedService.IsDatabaseSeededAsync())
                {
                    logger.LogInformation("Veritabanı zaten seed edilmiş. Atlanıyor.");
                    return;
                }

                logger.LogInformation("Seed data başlatılıyor...");
                await seedService.SeedInitialDataAsync();
                
                logger.LogInformation("✅ Seed data başarıyla tamamlandı!");
            }
            catch (Exception ex)
            {
                var errorMsg = "❌ Seed data sırasında kritik hata: {Message}";
                logger.LogError(ex, errorMsg, ex.Message);

                // Development'da exception throw et, Production'da logla ve devam et
                if (app.Environment.IsDevelopment())
                {
                    throw;
                }
            }
        }
    }
}
