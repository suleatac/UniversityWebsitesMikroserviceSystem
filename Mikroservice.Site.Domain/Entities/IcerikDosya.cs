namespace Mikroservice.Site.Domain.Entities
{
    /// <summary>
    /// Icerige (Haber, Duyuru vb.) bagli ek dosyalar.
    /// Admin panelinden icerik formu uzerine istenilen kadar dosya yuklenebilir;
    /// site tarafinda sidebar "Dosyalar" bolumunda listelenir.
    /// </summary>
    public class IcerikDosya
    {
        public int Id { get; set; }

        public int IcerikId { get; set; }

        // Listede gorunecek baslik (bos birakilirsa DosyaAdi kullanilir)
        public string? Baslik { get; set; }

        // MinIO ustundeki tam erisim adresi
        public string DosyaUrl { get; set; } = default!;

        // Kullaniciyi yukleyen orijinal dosya adi
        public string DosyaAdi { get; set; } = default!;

        public long DosyaBoyut { get; set; }

        // Uzatim / icerik turu kisa kodu (pdf, docx, xlsx ...)
        public string? DosyaTuru { get; set; }

        public int Sira { get; set; } = 0;

        public DateTime YuklemeTarihi { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Icerik Icerik { get; set; } = default!;
    }
}
