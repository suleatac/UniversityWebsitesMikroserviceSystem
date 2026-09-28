namespace Microservice.Admin.ViewModels.PageBuilder
{
    /// <summary>
    /// Site API'deki PageSectionDto karsiligi (agac: blocks + children + medias).
    /// </summary>
    public class GetPageSectionVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public int Sira { get; set; }
        public bool Yayinda { get; set; }

        public List<GetPageBlockVm> Blocks { get; set; } = [];
    }

    public class GetPageBlockVm
    {
        public int Id { get; set; }
        public int PageSectionId { get; set; }
        public int? ParentId { get; set; }
        public string ContentType { get; set; } = default!;
        public string? Content { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoType { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public int ColumnSize { get; set; }
        public int RowNumber { get; set; }
        public string? Animation { get; set; }

        public List<GetPageBlockMediaVm> Medias { get; set; } = [];
        public List<GetPageBlockVm> Children { get; set; } = [];
    }

    public class GetPageBlockMediaVm
    {
        public int Id { get; set; }
        public int PageBlockId { get; set; }
        public string ResimUrl { get; set; } = default!;
        public string? VideoUrl { get; set; }
        public int Sira { get; set; }
    }
}
