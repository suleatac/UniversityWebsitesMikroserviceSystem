using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.GetMediaFile
{
    public record GetMediaFilesQuery(int SiteId, int DilId) : IRequestByServiceResult<List<MediaFile>>;

}
