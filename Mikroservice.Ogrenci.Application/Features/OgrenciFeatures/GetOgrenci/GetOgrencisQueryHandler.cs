using AutoMapper;
using MediatR;
using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public class GetOgrencisQueryHandler(IOgrenciRepository ogrenciRepository, IRedisCacheService redisCacheService) : IRequestHandler<GetOgrencisQuery, ServiceResult<List<Domain.Entities.Ogrenci>> >
    {
        public async Task<ServiceResult<List<Domain.Entities.Ogrenci>>> Handle(GetOgrencisQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = "list:ogrencis";
            var cached = await redisCacheService.GetListAsync<Domain.Entities.Ogrenci>(cacheKey, cancellationToken);
            if (cached is not null)
                return ServiceResult<List<Domain.Entities.Ogrenci>>.SuccessAsOK(cached);

            // Yoksa veritabanından çek
            var data = ogrenciRepository.GetAll().ToList();
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);
            return ServiceResult<List<Domain.Entities.Ogrenci>>.SuccessAsOK(data);
 
        }
    }
}
