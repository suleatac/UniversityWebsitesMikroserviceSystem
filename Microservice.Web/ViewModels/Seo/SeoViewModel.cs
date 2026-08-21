namespace Microservice.Web.ViewModels.Seo
{
    public class SeoViewModel
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Keywords { get; set; }

        public string? CanonicalUrl { get; set; }

        public string? Robots { get; set; }

        public string? OgTitle { get; set; }

        public string? OgDescription { get; set; }

        public string? OgImage { get; set; }

        public bool IsIndexable { get; set; }

        public bool IsFollowable { get; set; }
    }
}
