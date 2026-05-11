using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;
using System.Net;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyuruById
{
    public class GetYonetimDuyuruByIdQueryHandler(
    IYonetimDuyuruRepository yonetimDuyuruRepository,
    ILogger<GetYonetimDuyuruByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetYonetimDuyuruByIdQuery, ServiceResult<YonetimDuyuruDto>>
    {
        public async Task<ServiceResult<YonetimDuyuruDto>> Handle(GetYonetimDuyuruByIdQuery request, CancellationToken cancellationToken)
        {

            // ✔ DB'den TEK kayıt çek
            var entity = await yonetimDuyuruRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Yonetim duyurusu bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<YonetimDuyuruDto>.Error("Yonetim duyurusu bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<YonetimDuyuruDto>(entity);

            logger.LogInformation("Yonetim duyurusu DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<YonetimDuyuruDto>.SuccessAsOK(dto);
        }
    }
}
