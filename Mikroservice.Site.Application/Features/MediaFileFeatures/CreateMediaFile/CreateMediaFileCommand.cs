using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.CreateMediaFile
{
    public record CreateMediaFileCommand : IRequestByServiceResult
    {
        public int SiteId { get; init; }
        public int DilId { get; init; }
        public string Path { get; set; } = default!;
        public string Url { get; set; } = default!;
    }
}
