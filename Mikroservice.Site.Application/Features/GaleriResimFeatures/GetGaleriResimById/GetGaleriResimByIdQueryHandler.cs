using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimById
{
    public class GetGaleriResimByIdQueryHandler(
        IGaleriResimRepository galeriResimRepository,
        ILogger<GetGaleriResimByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetGaleriResimByIdQuery, ServiceResult<GaleriResimDetailDto>>
    {
        public async Task<ServiceResult<GaleriResimDetailDto>> Handle(GetGaleriResimByIdQuery request, CancellationToken cancellationToken)
        {
            // ✔ DB'den TEK kayıt çek (PageType eager loading ile)
            var entity = await galeriResimRepository.GetByIdWithPageTypeAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                logger.LogWarning("GaleriResim bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<GaleriResimDetailDto>.Error("Galeri resmi bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<GaleriResimDetailDto>(entity);

            logger.LogInformation("GaleriResim DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<GaleriResimDetailDto>.SuccessAsOK(dto);
        }
    }
}
