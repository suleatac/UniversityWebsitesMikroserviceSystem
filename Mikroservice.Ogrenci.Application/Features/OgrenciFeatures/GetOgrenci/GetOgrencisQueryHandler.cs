using AutoMapper;
using MediatR;
using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microservice.Shared;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public class GetOgrencisQueryHandler(IOgrenciRepository ogrenciRepository, IRedisCacheService redisCacheService, ILogger<GetOgrencisQueryHandler> _logger) : IRequestHandler<GetOgrencisQuery, ServiceResult<List<Domain.Entities.Ogrenci>>>
    {
        public async Task<ServiceResult<List<Domain.Entities.Ogrenci>>> Handle(GetOgrencisQuery request, CancellationToken cancellationToken)
        {
  

            OpenTelemetryMetric.OgrenciGetOgrencisQueryExecuted.Add(1,
               new KeyValuePair<string, object?>("event", "add")); // Örnek Metric


            // Önce cache'e bak
            var cacheKey = "list:ogrencis";
            var cached = await redisCacheService.GetListAsync<Domain.Entities.Ogrenci>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                _logger.LogInformation("Öğrenciler verisi cacheden alındı. Öğrenci sayısı: {@personelCount}", cached.Count);
                return ServiceResult<List<Domain.Entities.Ogrenci>>.SuccessAsOK(cached);
            }



            // Yoksa veritabanından çek
            var data = ogrenciRepository.GetAll().ToList();
            //Örnek Loglama
            _logger.LogInformation("Öğrenciler verisi veritabanından alındı. Öğrenci sayısı:{@ogrenciCount}", data.Count);
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);
            return ServiceResult<List<Domain.Entities.Ogrenci>>.SuccessAsOK(data);

        }
    }
}
