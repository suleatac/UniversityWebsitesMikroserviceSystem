using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.ViewModels.BandLogo
{
    public class GetBandLogoVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Ad { get; set; } = default!;
        public string ImgUrl { get; set; } = default!;
        public string? Link { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EklenmeTarihi { get; set; }
    }
}
