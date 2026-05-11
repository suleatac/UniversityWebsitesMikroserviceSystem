using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.BannerDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetBannerById
{
    public class GetBannerByIdQueryHandler(
        IBannerRepository bannerRepository,
        ILogger<GetBannerByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetBannerByIdQuery, ServiceResult<BannerDetailDto>>
    {
        public async Task<ServiceResult<BannerDetailDto>> Handle(GetBannerByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await bannerRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Banner bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<BannerDetailDto>.Error("Banner bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<BannerDetailDto>(entity);

            logger.LogInformation("Banner DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<BannerDetailDto>.SuccessAsOK(dto);
        }
    }
}