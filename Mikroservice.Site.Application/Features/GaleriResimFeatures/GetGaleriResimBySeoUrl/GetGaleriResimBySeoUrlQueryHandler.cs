using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimBySeoUrl
{
    public class GetGaleriResimBySeoUrlQueryHandler(IGaleriResimRepository repository, IMapper mapper)
        : IRequestHandler<GetGaleriResimBySeoUrlQuery, ServiceResult<GaleriResimDetailDto>>
    {
        public async Task<ServiceResult<GaleriResimDetailDto>> Handle(GetGaleriResimBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<GaleriResimDetailDto>.Error("Galeri resmi bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<GaleriResimDetailDto>.SuccessAsOK(mapper.Map<GaleriResimDetailDto>(entity));
        }
    }
}
