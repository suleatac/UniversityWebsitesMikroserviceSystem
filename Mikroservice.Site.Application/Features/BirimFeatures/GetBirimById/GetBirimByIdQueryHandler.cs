using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.BirimDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.BirimFeatures.GetBirimById
{
    public class GetBirimByIdQueryHandler(
    IBirimRepository birimRepository,
    ILogger<GetBirimByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetBirimByIdQuery, ServiceResult<BirimDetailDto>>
    {
        public async Task<ServiceResult<BirimDetailDto>> Handle(GetBirimByIdQuery request, CancellationToken cancellationToken)
        {

            // ✔ DB'den TEK kayıt çek
            var entity = await birimRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Birim bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<BirimDetailDto>.Error("Birim bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<BirimDetailDto>(entity);

            logger.LogInformation("Birim DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<BirimDetailDto>.SuccessAsOK(dto);

        }
    }
}
