using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinlikBySeoUrl
{
    public class GetEtkinlikBySeoUrlQueryHandler(IEtkinlikRepository repository, IMapper mapper)
        : IRequestHandler<GetEtkinlikBySeoUrlQuery, ServiceResult<EtkinlikDetailDto>>
    {
        public async Task<ServiceResult<EtkinlikDetailDto>> Handle(GetEtkinlikBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<EtkinlikDetailDto>.Error("Etkinlik bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<EtkinlikDetailDto>.SuccessAsOK(mapper.Map<EtkinlikDetailDto>(entity));
        }
    }
}
