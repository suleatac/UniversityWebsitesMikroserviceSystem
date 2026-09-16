using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.SitePersonel
{
    public class PersonelDetailVm
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PersonelTipId { get; set; }
        public int PageTypeId { get; set; }

        public string? Adi { get; set; }
        public string? Soyadi { get; set; }
        public string? Username { get; set; }

        public string? ResimUrl { get; set; } = default!;
        public string? IlgiAlanlari { get; set; } = default!;
        public string? BlogAdress { get; set; } = default!;
        public string? TwitterAdress { get; set; } = default!;
        public string? FacebookAdress { get; set; } = default!;
        public string? InstagramAdress { get; set; } = default!;
        public string? GoogleplusAdress { get; set; } = default!;
        public string? Hakkinda { get; set; } = default!;
        public string? DeneyimVeCalismalari { get; set; } = default!;

        // SEO
        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }


        public PagesDetailVm PageType { get; set; } = default!;
    }
}
