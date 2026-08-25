using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSiteByHost
{
    public class GetSiteByHostQueryHandler(
        ISiteRepository siteRepository,
        IMapper mapper) : IRequestHandler<GetSiteByHostQuery, ServiceResult<SiteDetailDto>>
    {
        public Task<ServiceResult<SiteDetailDto>> Handle(GetSiteByHostQuery request, CancellationToken cancellationToken)
        {
            var host = request.Host.Trim().ToLowerInvariant();
         

            var site = siteRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .FirstOrDefault(x => x.SiteAlanAdi.ToLower()+".sivas.edu.tr" == host);

            if (site is null)
            {
                return Task.FromResult(ServiceResult<SiteDetailDto>.Error("Site not found", System.Net.HttpStatusCode.NotFound));
            }

            var dto = mapper.Map<SiteDetailDto>(site);
            return Task.FromResult(ServiceResult<SiteDetailDto>.SuccessAsOK(dto));
        }
    }
}