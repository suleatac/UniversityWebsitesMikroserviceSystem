using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.PageRouteFeatures.GetBySlug
{
    public class GetBySlugQueryHandler(
          IPageRouteRepository pageRouteRepository,
          ILogger<GetBySlugQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetBySlugQuery, ServiceResult<PageRouteDto>>
    {
        // Impl. of IRequestHandler (implicit)
        public async Task<ServiceResult<PageRouteDto>> Handle(GetBySlugQuery request, CancellationToken cancellationToken)
        {
            // Veritabanından slug'a göre çek
            var data = await pageRouteRepository.GetBySlugAsync(request.SiteId, request.Slug, cancellationToken);

            // Örnek log
            logger.LogInformation(
                "Page route verisi veritabanından alındı. SiteId:{siteId}, Slug:{slug}",
                request.SiteId,
                request.Slug);

            var mappedData = mapper.Map<PageRouteDto>(data);

            return ServiceResult<PageRouteDto>.SuccessAsOK(mappedData);
        }
    }
}