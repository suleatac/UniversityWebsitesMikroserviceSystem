using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.MenuDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenuBySeoUrl
{
  
    public class GetMenuBySeoUrlQueryHandler(IMenuRepository repository, IMapper mapper)
       : IRequestHandler<GetMenuBySeoUrlQuery, ServiceResult<MenuDetailDto>>
    {
        public async Task<ServiceResult<MenuDetailDto>> Handle(GetMenuBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<MenuDetailDto>.Error("Menu bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<MenuDetailDto>.SuccessAsOK(mapper.Map<MenuDetailDto>(entity));
        }
    }
}
