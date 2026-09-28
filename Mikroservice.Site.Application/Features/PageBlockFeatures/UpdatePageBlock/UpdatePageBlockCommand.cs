using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.UpdatePageBlock
{
    public record UpdatePageBlockCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public int PageSectionId { get; set; }
        public int? ParentId { get; set; }
        public string ContentType { get; set; } = default!;
        public string? Content { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoType { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public int ColumnSize { get; set; } = 12;
        public int RowNumber { get; set; }
        public string? Animation { get; set; }

        /// <summary>Carousel tipi icin slaytlar (tam liste gönderilir; senkronize edilir).</summary>
        public List<PageBlockMediaInputDto> Medias { get; set; } = [];
    }
}
