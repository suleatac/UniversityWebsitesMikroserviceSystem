using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SiteDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSiteById
{
    public class GetSiteByIdQueryHandler(
    ISiteRepository siteRepository,
    ILogger<GetSiteByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetSiteByIdQuery, ServiceResult<SiteDetailDto>>
    {
        public async Task<ServiceResult<SiteDetailDto>> Handle(GetSiteByIdQuery request, CancellationToken cancellationToken)
        {

            // ✔ DB'den TEK kayıt çek
            var entity = await siteRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Site bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<SiteDetailDto>.Error("Site bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<SiteDetailDto>(entity);

            logger.LogInformation("Site DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<SiteDetailDto>.SuccessAsOK(dto);
        }
    }
}
