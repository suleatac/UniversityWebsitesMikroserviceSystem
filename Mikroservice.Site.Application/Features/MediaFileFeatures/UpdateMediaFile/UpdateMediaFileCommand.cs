using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.UpdateMediaFile
{
    public record UpdateMediaFileCommand : IRequestByServiceResult
    {
        public int Id { get; set; }
        public int SiteId { get; init; }
        public int DilId { get; init; }
        public string Path { get; set; } = default!;
        public string Url { get; set; } = default!;
    }
}
