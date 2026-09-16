using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelById
{
    public record GetSitePersonelByIdQuery(int Id) : IRequestByServiceResult<SitePersonelDetailDto>;
}