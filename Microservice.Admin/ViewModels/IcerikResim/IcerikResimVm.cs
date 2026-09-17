namespace Microservice.Admin.ViewModels.IcerikResim
{
    /// <summary>
    /// Mikroservice.Site.Application DTOs.IcerikResimDtos.IcerikResimInputDto karsiligi.
    /// Icerik (Haber/Duyuru) formuna eklenen galeri resmi satirlari icin kullanilir.
    /// Id = 0 -> yeni resim, Id > 0 -> mevcut resim guncelleme, listeden cikarilan -> silme.
    /// </summary>
    public class IcerikResimVm
    {
        public int Id { get; set; }

        public string? Baslik { get; set; }

        public string ResimUrl { get; set; } = default!;

        public int Sira { get; set; }
    }
}
