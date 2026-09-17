namespace Mikroservice.Site.Domain.Entities
{
    /// <summary>
    /// Galeri resimleri: Icerik (TPH) tabanli.
    /// ResimUrl = resmin adresi, Kategori = liste sayfasindaki filtre kategorisi.
    /// </summary>
    public class GaleriResim : Icerik
    {
        public string? Kategori { get; set; }
    }
}
