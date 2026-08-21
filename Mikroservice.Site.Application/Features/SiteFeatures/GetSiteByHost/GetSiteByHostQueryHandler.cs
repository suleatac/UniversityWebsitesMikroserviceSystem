using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSiteByHost
{
    public class GetSiteByHostQueryHandler(
        ISiteRepository siteRepository,
        IMapper mapper) : IRequestHandler<GetSiteByHostQuery, ServiceResult<PageRouteDto>>
    {
        public Task<ServiceResult<PageRouteDto>> Handle(GetSiteByHostQuery request, CancellationToken cancellationToken)
        {
            var host = request.Host.Trim().ToLowerInvariant();
            if (host.StartsWith("www."))
            {
                host = host[4..];
            }

            var site = siteRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .FirstOrDefault(x => x.SiteAlanAdi.ToLower() == host || x.SiteAlanAdi.ToLower() == "www." + host);

            if (site is null)
            {
                return Task.FromResult(ServiceResult<PageRouteDto>.Error("Site not found", System.Net.HttpStatusCode.NotFound));
            }

            var dto = mapper.Map<PageRouteDto>(site);
            return Task.FromResult(ServiceResult<PageRouteDto>.SuccessAsOK(dto));
        }
    }
}