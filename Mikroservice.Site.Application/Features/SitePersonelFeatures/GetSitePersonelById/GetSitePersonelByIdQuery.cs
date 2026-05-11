using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelById
{
    public record GetSitePersonelByIdQuery(int Id) : IRequestByServiceResult<SitePersonel>;
}