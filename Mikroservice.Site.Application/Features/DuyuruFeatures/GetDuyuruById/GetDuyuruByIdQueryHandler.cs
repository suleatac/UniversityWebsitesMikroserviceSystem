using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyuruById
{
    public class GetDuyuruByIdQueryHandler(
        IDuyuruRepository duyuruRepository,
        ILogger<GetDuyuruByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetDuyuruByIdQuery, ServiceResult<DuyuruDetailDto>>
    {
        public async Task<ServiceResult<DuyuruDetailDto>> Handle(GetDuyuruByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await duyuruRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Duyuru bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<DuyuruDetailDto>.Error("Duyuru bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<DuyuruDetailDto>(entity);

            logger.LogInformation("Duyuru DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<DuyuruDetailDto>.SuccessAsOK(dto);
        }
    }
}