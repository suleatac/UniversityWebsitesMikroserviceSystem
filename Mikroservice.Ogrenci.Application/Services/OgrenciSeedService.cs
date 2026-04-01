using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Ogrenci.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Application.Services
{
    public class OgrenciSeedService : IOgrenciSeedService
    {
        private readonly IOgrenciRepository _ogrenciRepository;
        private readonly IOgrenciSyncService _ogrenciSyncService;
        private readonly ILogger<OgrenciSeedService> _logger;

        public OgrenciSeedService(
            IOgrenciRepository ogrenciRepository,
            IOgrenciSyncService ogrenciSyncService,
            ILogger<OgrenciSeedService> logger)
        {
            _ogrenciRepository = ogrenciRepository;
            _ogrenciSyncService = ogrenciSyncService;
            _logger = logger;
        }
        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 öğrenci var mı kontrol et
            var hasStudents = await _ogrenciRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database seeded kontrolü: {HasStudents}", hasStudents);
            return hasStudents;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Initial seed data başlatılıyor...");

            // Mevcut sync service'i kullanarak tüm verileri çek
            var response = await _ogrenciSyncService.SyncOgrencisAsync(null, cancellationToken);

            _logger.LogInformation("Seed tamamlandı. Eklenen öğrenci sayısı: {Count}", response.TotalCount);

        }
    }
}
