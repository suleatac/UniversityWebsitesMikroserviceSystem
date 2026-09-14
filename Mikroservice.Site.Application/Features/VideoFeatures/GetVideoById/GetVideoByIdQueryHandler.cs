using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.VideoDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideoById
{
    public class GetVideoByIdQueryHandler(
        IVideoRepository videoRepository,
        ILogger<GetVideoByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetVideoByIdQuery, ServiceResult<VideoDetailDto>>
    {
        public async Task<ServiceResult<VideoDetailDto>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
        {
            // ✔ DB'den TEK kayıt çek (PageType eager loading ile)
            var entity = await videoRepository.GetByIdWithPageTypeAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                logger.LogWarning("Video bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<VideoDetailDto>.Error("Video bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<VideoDetailDto>(entity);

            logger.LogInformation("Video DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<VideoDetailDto>.SuccessAsOK(dto);
        }
    }
}