using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.GetShortcutButtonById
{
    public class GetShortcutButtonByIdQueryHandler(
        IShortcutButtonRepository shortcutButtonRepository,
        ILogger<GetShortcutButtonByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetShortcutButtonByIdQuery, ServiceResult<ShortcutButtonDetailDto>>
    {
        public async Task<ServiceResult<ShortcutButtonDetailDto>> Handle(GetShortcutButtonByIdQuery request, CancellationToken cancellationToken)
        {
            // ✔ DB'den TEK kayıt çek
            var entity = await shortcutButtonRepository.GetByIdAsync(request.Id);

            if (entity is null || entity.IsDeleted)
            {
                logger.LogWarning("ShortcutButton bulunamadı. Id: {Id}", request.Id);

                return ServiceResult<ShortcutButtonDetailDto>.Error("ShortcutButton bulunamadı", HttpStatusCode.NotFound);
            }

            // ✔ map
            var dto = mapper.Map<ShortcutButtonDetailDto>(entity);

            logger.LogInformation("ShortcutButton DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<ShortcutButtonDetailDto>.SuccessAsOK(dto);
        }
    }
}
