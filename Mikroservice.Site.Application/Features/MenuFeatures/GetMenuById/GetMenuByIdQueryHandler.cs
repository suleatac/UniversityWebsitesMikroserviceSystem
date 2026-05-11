using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.MenuDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenuById
{
    public class GetMenuByIdQueryHandler(
        IMenuRepository menuRepository,
        ILogger<GetMenuByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetMenuByIdQuery, ServiceResult<MenuDetailDto>>
    {
        public async Task<ServiceResult<MenuDetailDto>> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            // ✔ DB'den TEK kayıt çek
            var entity = await menuRepository.GetByIdAsync(request.Id);

            if (entity is null || entity.IsDeleted)
            {
                logger.LogWarning("Menu bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<MenuDetailDto>.Error("Menu bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<MenuDetailDto>(entity);

            logger.LogInformation("Menu DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<MenuDetailDto>.SuccessAsOK(dto);
        }
    }
}