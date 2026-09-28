using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;

namespace Mikroservice.Site.Application.Features.PageBlockFeatures.CreatePageBlock
{
    public record CreatePageBlockCommand : IRequestByServiceResult<CreatePageBlockResponse>
    {
        public int PageSectionId { get; set; }
        public int? ParentId { get; set; }

        /// <summary>BlockContentType: text / video / image / carousel.</summary>
        public string ContentType { get; set; } = default!;

        public string? Content { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoType { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public int ColumnSize { get; set; } = 12;
        public int RowNumber { get; set; }
        public string? Animation { get; set; }

        /// <summary>Carousel tipi icin slaytlar.</summary>
        public List<PageBlockMediaInputDto> Medias { get; set; } = [];
    }

    public record CreatePageBlockResponse(int Id);
}
