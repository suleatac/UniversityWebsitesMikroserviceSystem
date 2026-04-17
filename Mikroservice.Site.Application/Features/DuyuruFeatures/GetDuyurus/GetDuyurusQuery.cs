using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyurus
{
    public record GetDuyurusQuery(int SiteId, int DilId) : IRequestByServiceResult<List<Duyuru>>;
}
