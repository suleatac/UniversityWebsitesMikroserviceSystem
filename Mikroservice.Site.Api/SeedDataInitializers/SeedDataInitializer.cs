using Mikroservice.Site.Application.Contracts.Services;

namespace Mikroservice.Site.Api.SeedDataInitializers
{
    public static class SeedDataInitializer
    {
        public static async Task InitializeSeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            //var seedServices = scope.ServiceProvider.GetServices<ISeedService>();
            var seedServices = scope.ServiceProvider.GetServices<ISeedService>().OrderBy(x => x.Sira);

            foreach (var seedService in seedServices)
            {
                try
                {
                    if (await seedService.IsDatabaseSeededAsync())
                    {
                        logger.LogInformation($"{seedService.GetType().Name} için veritabanı boş değil veya zaten seed edilmiş. Atlanıyor.");
                        return;
                    }

                    logger.LogInformation($"Seed çalışıyor: {seedService.GetType().Name}");
                    await seedService.SeedInitialDataAsync();
                    logger.LogInformation($"✅ {seedService.GetType().Name} için seed data başarıyla tamamlandı!");
                }
                catch (Exception ex)
                {
                    var errorMsg = $"❌ {seedService.GetType().Name} için seed data sırasında kritik hata: {{Message}}";
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
}
