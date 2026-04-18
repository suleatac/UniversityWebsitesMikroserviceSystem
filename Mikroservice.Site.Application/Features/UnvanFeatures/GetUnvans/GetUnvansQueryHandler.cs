using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvans
{
    public class GetUnvanQueryHandler(
        IUnvanRepository repository,
        IRedisCacheService redis,
        ILogger<GetUnvanQueryHandler> logger
    ) : IRequestHandler<GetUnvansQuery, ServiceResult<List<Unvan>>>
    {
        public async Task<ServiceResult<List<Unvan>>> Handle(GetUnvansQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "unvans:list";

            var cached = await redis.GetListAsync<Unvan>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("Unvan cache'den alındı");
                return ServiceResult<List<Unvan>>.SuccessAsOK(cached);
            }

            var data = repository.GetAll()
                .OrderBy(x => x.Sira)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            return ServiceResult<List<Unvan>>.SuccessAsOK(data);
        }
    }
}
