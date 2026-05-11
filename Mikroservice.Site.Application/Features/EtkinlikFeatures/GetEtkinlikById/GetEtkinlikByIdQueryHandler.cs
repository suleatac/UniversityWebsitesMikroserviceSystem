using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinlikById
{
    public class GetEtkinlikByIdQueryHandler(
        IEtkinlikRepository etkinlikRepository,
        ILogger<GetEtkinlikByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetEtkinlikByIdQuery, ServiceResult<EtkinlikDetailDto>>
    {
        public async Task<ServiceResult<EtkinlikDetailDto>> Handle(GetEtkinlikByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await etkinlikRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Etkinlik bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<EtkinlikDetailDto>.Error("Etkinlik bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<EtkinlikDetailDto>(entity);

            logger.LogInformation("Etkinlik DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<EtkinlikDetailDto>.SuccessAsOK(dto);
        }
    }
}