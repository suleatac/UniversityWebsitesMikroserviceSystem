using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Etkinlik
{
    public class GetEtkinlikVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? HedefId { get; set; }
        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string? ResimUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime YayimTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BaslamaTarihi { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BitisTarihi { get; set; }
    }
}