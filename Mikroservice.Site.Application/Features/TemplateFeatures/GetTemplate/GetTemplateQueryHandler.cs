using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplate
{
    public class GetTemplateQueryHandler(
        ITemplateRepository templateRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetTemplateQueryHandler> logger
      )
      : IRequestHandler<GetTemplateQuery, ServiceResult<List<Template>>>
    {
        public async Task<ServiceResult<List<Template>>> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "template:list";
            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<Template>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Template cache'den alındı. Count:{count}", cached.Count);
                return ServiceResult<List<Template>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = templateRepository.GetAll().ToList();
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);


            logger.LogInformation("Template DB'den alındı. Count:{count}", data.Count);

            return ServiceResult<List<Template>>.SuccessAsOK(data);
        }

    }
}
