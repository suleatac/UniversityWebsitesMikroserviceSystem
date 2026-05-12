using AutoMapper;
using MediatR;
using Microservice.Personel.Application.Contracts.DTOs;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Shared;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.Extensions.Logging;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public class GetPersonelsQueryHandler
        (
           IPersonelRepository personelRepository, 
           IRedisCacheService redisCacheService, 
           ILogger<GetPersonelsQueryHandler> _logger,
           IMapper mapper
        ) : IRequestHandler<GetPersonelsQuery, ServiceResult<List<PersonelDto>>>
    {
  
        public async Task<ServiceResult<List<PersonelDto>>> Handle(GetPersonelsQuery request, CancellationToken cancellationToken)
        {
      

            OpenTelemetryMetric.PersonelGetPersonelsQueryExecuted.Add(1, 
                new KeyValuePair<string, object?>("event", "add")); // Örnek Metric

            // Önce cache'e bak
            var cacheKey = "list:personels";
            var cached = await redisCacheService.GetListAsync<PersonelDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                _logger.LogInformation("Personeller verisi cacheden alındı. Personel sayısı: {@personelCount}", cached.Count);
                return ServiceResult<List<PersonelDto>>.SuccessAsOK(cached); 
            }
             

            // Yoksa veritabanından çek
            var data =  personelRepository.GetAll().ToList();

            var mappedData = mapper.Map<List<PersonelDto>>(data);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);

     
            //Örnek Loglama
            _logger.LogInformation("Personeller verisi veritabanından alındı. Personel sayısı:{@personelCount}", data.Count);

            return ServiceResult<List<PersonelDto>>.SuccessAsOK(mappedData);

        }
    }
}
