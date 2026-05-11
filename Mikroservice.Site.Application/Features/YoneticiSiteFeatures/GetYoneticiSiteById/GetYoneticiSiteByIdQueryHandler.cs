using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSiteById
{
    public class GetYoneticiSiteByIdQueryHandler(
        IYoneticiSiteRepository repository,
        ILogger<GetYoneticiSiteByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetYoneticiSiteByIdQuery, ServiceResult<YoneticiSiteDetailDto>>
    {
        public async Task<ServiceResult<YoneticiSiteDetailDto>> Handle(GetYoneticiSiteByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity is null || entity.IsDeleted)
            {
                logger.LogWarning("YoneticiSite bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<YoneticiSiteDetailDto>.Error("YoneticiSite bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<YoneticiSiteDetailDto>(entity);

            logger.LogInformation("YoneticiSite DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<YoneticiSiteDetailDto>.SuccessAsOK(dto);
        }
    }
}