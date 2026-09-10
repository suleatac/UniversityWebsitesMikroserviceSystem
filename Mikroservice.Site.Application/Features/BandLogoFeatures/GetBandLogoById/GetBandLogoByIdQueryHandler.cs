using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogoById
{
    public class GetBandLogoByIdQueryHandler(
        IBandLogoRepository bandLogoRepository,
        ILogger<GetBandLogoByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetBandLogoByIdQuery, ServiceResult<BandLogoDetailDto>>
    {
        public async Task<ServiceResult<BandLogoDetailDto>> Handle(GetBandLogoByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await bandLogoRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("BandLogo bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<BandLogoDetailDto>.Error("BandLogo bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<BandLogoDetailDto>(entity);

            logger.LogInformation("BandLogo DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<BandLogoDetailDto>.SuccessAsOK(dto);
        }
    }
}
