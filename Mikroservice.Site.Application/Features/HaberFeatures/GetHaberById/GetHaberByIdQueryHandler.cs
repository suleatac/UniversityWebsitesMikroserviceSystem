using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHaberById
{
    public class GetHaberByIdQueryHandler(
    IHaberRepository haberRepository,
    ILogger<GetHaberByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetHaberByIdQuery, ServiceResult<HaberDetailDto>>
    {
        public async Task<ServiceResult<HaberDetailDto>> Handle(GetHaberByIdQuery request, CancellationToken cancellationToken)
        {

            // ✔ DB'den TEK kayıt çek
            var entity = await haberRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Haber bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<HaberDetailDto>.Error("Haber bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<HaberDetailDto>(entity);

            logger.LogInformation("Haber DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<HaberDetailDto>.SuccessAsOK(dto);
        }
    }
}
