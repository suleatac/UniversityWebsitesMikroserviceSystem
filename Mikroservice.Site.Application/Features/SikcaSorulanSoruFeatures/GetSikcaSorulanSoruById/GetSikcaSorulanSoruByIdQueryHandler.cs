using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SikcaSorulanSoruDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoruById
{
    public class GetSikcaSorulanSoruByIdQueryHandler(
        ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
        ILogger<GetSikcaSorulanSoruByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetSikcaSorulanSoruByIdQuery, ServiceResult<SikcaSorulanSoruDetailDto>>
    {
        public async Task<ServiceResult<SikcaSorulanSoruDetailDto>> Handle(GetSikcaSorulanSoruByIdQuery request, CancellationToken cancellationToken)
        {
            // ✔ DB'den TEK kayıt çek
            var entity = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);

            if (entity is null || entity.IsDeleted)
            {
                logger.LogWarning("SikcaSorulanSoru bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<SikcaSorulanSoruDetailDto>.Error("SikcaSorulanSoru bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<SikcaSorulanSoruDetailDto>(entity);

            logger.LogInformation("SikcaSorulanSoru DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<SikcaSorulanSoruDetailDto>.SuccessAsOK(dto);
        }
    }
}