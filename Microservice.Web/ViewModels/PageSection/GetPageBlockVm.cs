namespace Microservice.Web.ViewModels.PageSection
{
    /// <summary>
    /// Section icindeki container. Children nested satirlari, Medias carousel slaytlarini tasir.
    /// </summary>
    public class GetPageBlockVm
    {
        public int Id { get; set; }
        public int PageSectionId { get; set; }
        public int? ParentId { get; set; }

        /// <summary>text / video / image / carousel</summary>
        public string ContentType { get; set; } = default!;

        public string? Content { get; set; }
        public string? VideoUrl { get; set; }

        /// <summary>YouTube / Local</summary>
        public string? VideoType { get; set; }

        public string? BackgroundImageUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public int ColumnSize { get; set; }
        public int RowNumber { get; set; }
        public string? Animation { get; set; }

        public List<GetPageBlockMediaVm> Medias { get; set; } = [];
        public List<GetPageBlockVm> Children { get; set; } = [];
    }
}
