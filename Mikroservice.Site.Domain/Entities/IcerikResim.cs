namespace Mikroservice.Site.Domain.Entities
{
    /// <summary>
    /// Icerige (Haber, Duyuru vb.) bagli galeri resimleri.
    /// Admin panelinden icerik formu uzerine istenilen kadar resim secilebilir;
    /// site tarafinda icerik metninin altindaki slider bolumunde gosterilir.
    /// </summary>
    public class IcerikResim
    {
        public int Id { get; set; }

        public int IcerikId { get; set; }

        // Slider ustunda gorunecek baslik / alt yazi (opsiyonel)
        public string? Baslik { get; set; }

        // MinIO ustundeki tam erisim adresi
        public string ResimUrl { get; set; } = default!;

        public int Sira { get; set; } = 0;

        public DateTime YuklemeTarihi { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Icerik Icerik { get; set; } = default!;
    }
}
