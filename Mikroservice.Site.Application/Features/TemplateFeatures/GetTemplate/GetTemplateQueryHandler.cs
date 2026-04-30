using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.TemplateDtos;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplate
{
    public class GetTemplateQueryHandler(
        ITemplateRepository templateRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetTemplateQueryHandler> logger,
        IMapper mapper
      )
      : IRequestHandler<GetTemplateQuery, ServiceResult<List<TemplateListDto>>>
    {
        public async Task<ServiceResult<List<TemplateListDto>>> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "template:list";
            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<TemplateListDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Template cache'den alındı. Count:{count}", cached.Count);


                return ServiceResult<List<TemplateListDto>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var data = templateRepository.GetAll().ToList();
           

            logger.LogInformation("Template DB'den alındı. Count:{count}", data.Count);

            var mappedData = mapper.Map<List<TemplateListDto>>(data);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);


            return ServiceResult<List<TemplateListDto>>.SuccessAsOK(mappedData);
        }

    }
}
