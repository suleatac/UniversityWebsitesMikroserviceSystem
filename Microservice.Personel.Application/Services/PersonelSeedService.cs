using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Personel.Application.Contracts.Services;
using Microsoft.Extensions.Logging;
using Mikroservice.Personel.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Personel.Application.Services
{
    public class PersonelSeedService : IPersonelSeedService
    {
        private readonly IPersonelRepository _PersonelRepository;
        private readonly IPersonelSyncService _PersonelSyncService;
        private readonly ILogger<PersonelSeedService> _logger;

        public PersonelSeedService(
            IPersonelRepository PersonelRepository,
            IPersonelSyncService PersonelSyncService,
            ILogger<PersonelSeedService> logger)
        {
            _PersonelRepository = PersonelRepository;
            _PersonelSyncService = PersonelSyncService;
            _logger = logger;
        }
        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 öğrenci var mı kontrol et
            var hasStudents = await _PersonelRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database seeded kontrolü: {HasStudents}", hasStudents);
            return hasStudents;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Initial seed data başlatılıyor...");

            // Mevcut sync service'i kullanarak tüm verileri çek
            var response = await _PersonelSyncService.SyncPersonelsAsync(null, cancellationToken);

            _logger.LogInformation("Seed tamamlandı. Eklenen öğrenci sayısı: {Count}", response.TotalCount);

        }
    }
}
