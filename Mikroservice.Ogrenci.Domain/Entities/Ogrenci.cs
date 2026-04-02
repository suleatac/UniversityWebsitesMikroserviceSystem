using Microsoft.EntityFrameworkCore;

namespace Microservice.Ogrenci.Domain.Entities
{
    [Index(nameof(OgrenciProgramId))]
    [Index(nameof(Adi), nameof(Soyadi))]
    [Index(nameof(Eposta))]
    [Index(nameof(Username))]
    [Index(nameof(SonGuncellemeTarihi))]
    public class Ogrenci
    {
        // ── Internal / DB fields ─────────────────────────────
        public int Id { get; set; }
        public string? Username { get; set; }
        public DateTime? SonGuncellemeTarihi { get; set; }
        public string? KisiselEposta { get; set; }
        public string? KisiselTelefon { get; set; }

        // ── API fields (names match JSON keys exactly) ────────
        public int? OgrenciProgramId { get; set; }
        public string? TcNumarasi { get; set; }
        public string? Uyruk { get; set; }
        public string? Adi { get; set; }
        public string? Soyadi { get; set; }
        public string? Cinsiyeti { get; set; }
        public string? Eposta { get; set; }
        public string? TelefonCep { get; set; }
        public string? Adres { get; set; }
        public string? BabaAdi { get; set; }
        public string? AnaAdi { get; set; }
        public string? DogumYeri { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string? KanGrubu { get; set; }
        public string? SehitGaziYakini { get; set; }
        public string? ResimKodu { get; set; }
        public int? FakulteId { get; set; }
        public string? Fakulte { get; set; }
        public string? Bolum { get; set; }
        public string? FakulteIngilizceAdi { get; set; }
        public string? BolumIngilizceAdi { get; set; }
        public int? Sinif { get; set; }
        public string? StudentNo { get; set; }
        public string? PersonBase64Image { get; set; }
        public string? AkademikProgram { get; set; }
        public string? ProgramTipi { get; set; }
        public string? Scholarship { get; set; }
        public string? Durum { get; set; }
        public string? DurumDetail { get; set; }
        public DateTime? MezuniyetTarihi { get; set; }
        public DateTime? IlisikKesmeTarihi { get; set; }
        public string? OgretimTipi { get; set; }
        public string? ProgramTuru { get; set; }
        public decimal? TranscriptNotOrtalamasi { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public int? BolumId { get; set; }
        public int? YoksisOgrenciId { get; set; }
        public int? YoksisId { get; set; }
    }
}
