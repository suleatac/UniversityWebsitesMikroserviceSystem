namespace Microservice.Web.ViewModels.Ogrenci
{
    /// <summary>
    /// Ogrenci API'den gelen tam entity yerine sadece sayaclar icin gereken alanlar.
    /// (Entity base64 foto alanlari iceriyor; butununu cekmek gereksiz bedel olusturur.)
    /// </summary>
    public class OgrenciSayiVm
    {
        public string? Durum { get; set; }
        public DateTime? MezuniyetTarihi { get; set; }
    }
}
