namespace Microservice.Web.ViewModels.PageSection
{
    public class GetPageBlockMediaVm
    {
        public int Id { get; set; }
        public int PageBlockId { get; set; }
        public string ResimUrl { get; set; } = default!;
        public string? VideoUrl { get; set; }
        public int Sira { get; set; }
    }
}
