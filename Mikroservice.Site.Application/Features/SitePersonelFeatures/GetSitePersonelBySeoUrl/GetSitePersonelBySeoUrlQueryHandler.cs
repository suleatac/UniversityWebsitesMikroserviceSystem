using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelBySeoUrl
{
    public class GetSitePersonelBySeoUrlQueryHandler(ISitePersonelRepository repository, IMapper mapper)
        : IRequestHandler<GetSitePersonelBySeoUrlQuery, ServiceResult<SitePersonelDetailDto>>
    {
        public async Task<ServiceResult<SitePersonelDetailDto>> Handle(GetSitePersonelBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<SitePersonelDetailDto>.Error("SitePersonel bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<SitePersonelDetailDto>.SuccessAsOK(mapper.Map<SitePersonelDetailDto>(entity));
        }
    }
}
