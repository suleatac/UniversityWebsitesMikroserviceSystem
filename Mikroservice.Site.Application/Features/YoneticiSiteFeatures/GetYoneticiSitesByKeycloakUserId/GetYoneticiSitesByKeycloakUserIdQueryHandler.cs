using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSitesByKeycloakUserId
{
    public class GetYoneticiSitesByKeycloakUserIdQueryHandler(
        IYoneticiSiteRepository repository,
        ILogger<GetYoneticiSitesByKeycloakUserIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetYoneticiSitesByKeycloakUserIdQuery, ServiceResult<List<YoneticiSiteDetailDto>>>
    {
        public async Task<ServiceResult<List<YoneticiSiteDetailDto>>> Handle(
            GetYoneticiSitesByKeycloakUserIdQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.KeycloakUserId))
            {
                return ServiceResult<List<YoneticiSiteDetailDto>>.Error("KeycloakUserId boş olamaz", System.Net.HttpStatusCode.BadRequest);
            }

            var data = await repository.GetByKeycloakUserIdAsync(request.KeycloakUserId, cancellationToken);

            var mappedData = mapper.Map<List<YoneticiSiteDetailDto>>(data);

            logger.LogInformation("KeycloakUserId: {KeycloakUserId} için {Count} adet YoneticiSite bulundu", request.KeycloakUserId, mappedData.Count);

            return ServiceResult<List<YoneticiSiteDetailDto>>.SuccessAsOK(mappedData);
        }
    }
}