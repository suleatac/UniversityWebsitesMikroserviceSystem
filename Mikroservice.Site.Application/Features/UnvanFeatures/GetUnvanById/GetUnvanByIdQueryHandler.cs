using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.UnvanDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvanById
{
    public class GetUnvanByIdQueryHandler(
    IUnvanRepository unvanRepository,
    ILogger<GetUnvanByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetUnvanByIdQuery, ServiceResult<UnvanDetailDto>>
    {
        public async Task<ServiceResult<UnvanDetailDto>> Handle(GetUnvanByIdQuery request, CancellationToken cancellationToken)
        {

            // ✔ DB'den TEK kayıt çek
            var entity = await unvanRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("Unvan bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<UnvanDetailDto>.Error("Unvan bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<UnvanDetailDto>(entity);

            logger.LogInformation("Unvan DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<UnvanDetailDto>.SuccessAsOK(dto);
        }
    }
}
