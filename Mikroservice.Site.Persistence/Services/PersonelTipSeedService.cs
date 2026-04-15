using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    public class PersonelTipSeedService
    {
        private readonly IPersonelTipRepository _personelTipRepository;
        private readonly ILogger<PersonelTipSeedService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PersonelTipSeedService(
            IPersonelTipRepository personelTipRepository,
            IUnitOfWork unitOfWork,
            ILogger<PersonelTipSeedService> logger)
        {
            _personelTipRepository = personelTipRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 personel var mı kontrol et
            var personelTipiVarMı = await _personelTipRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasPersonelTipi}", personelTipiVarMı);
            return personelTipiVarMı;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _unitOfWork.GetExecutionStrategy();

            await strategy.ExecuteAsync(async () => {

                await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Personel tipleri seed işlemi başlatılıyor.");

                    var PersonelTipleri = PersonelTipSeedData.GetYoneticiTipiSeedDatas();

                    await ProcessPersonelTipleriAsync(PersonelTipleri, cancellationToken);
                    await _unitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Personel tipleri seed işlemi tamamlandı. İşlenen: {Count}", PersonelTipleri.Count);


                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessPersonelTipleriAsync(List<PersonelTip> PersonelTipleri, CancellationToken cancellationToken)
        {

            foreach (var PersonelTipi in PersonelTipleri)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _personelTipRepository.AddAsync(PersonelTipi);

            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
