using Microsoft.EntityFrameworkCore;

namespace Microservice.Personel.Domain.Entities;

[Index(nameof(Adi), nameof(Soyadi))]
[Index(nameof(Eposta))]
[Index(nameof(Username))]
[Index(nameof(SonGuncellemeTarihi))]
public class Personel
{
    public int Id { get; set; }
    public int? PersonId { get; set; }
    public string? TcNumarasi { get; set; }
    public string? Adi { get; set; }
    public string? Soyadi { get; set; }
    public string? Uyruk { get; set; }
    public string? Cinsiyeti { get; set; }
    public string? Eposta { get; set; }
    public string? TelefonCep { get; set; }
    public string? TelefonDahili { get; set; }
    public string? TelefonDahiliNumara { get; set; }
    public string? Adres { get; set; }
    public string? BabaAdi { get; set; }
    public string? AnaAdi { get; set; }
    public string? DogumYeri { get; set; }
    public DateTime? DogumTarihi { get; set; }
    public string? KanGrubu { get; set; }
    public string? SehitGaziYakini { get; set; }
    public int? PersonelTipiId { get; set; }
    public string? PersonelTipi { get; set; }
    public int? GorevYeriId { get; set; }
    public string? GorevYeri { get; set; }
    public DateTime? GoreveBaslamaTarihi { get; set; }
    public DateTime? KurumdanAyrilisTarihi { get; set; }
    public int? KadroTipiId { get; set; }
    public string? KadroTipi { get; set; }
    public string? KadroKodu { get; set; }
    public string? IdariGorevler { get; set; }
    public string? IliskiliOlduguPozisyonlar { get; set; }
    public string? EmekliSicilKodu { get; set; }
    public int? UnvanId { get; set; }
    public string? AsliUnvan { get; set; }
    public int? EkGosterge { get; set; }
    public int? GorevUnvaniId { get; set; }
    public string? GorevUnvan { get; set; }
    public int? KadroUnvanId { get; set; }
    public string? KadroUnvan { get; set; }
    public string? KurumSicilNo { get; set; }
    public int? UstGorevYeriId { get; set; }
    public string? UstGorevYeriAdi { get; set; }
    public int? UstGorevBirimId { get; set; }
    public string? UstGorevBirimAdi { get; set; }
    public int? KadroBirimId { get; set; }
    public string? KadroBirimi { get; set; }
    public int? KadroUstBirimId { get; set; }
    public string? KadroUstBirim { get; set; }
    public string? Username { get; set; }
    public string? PersonEncryptedId { get; set; }
    public decimal? BrutUcret { get; set; }
    public DateTime? SonGuncellemeTarihi { get; set; }
    public string? KisiselEposta { get; set; }
    public string? KisiselTelefon { get; set; }
    public bool? Aktif { get; set; }
    public string? PersonBase64ImageModifiedOn { get; set; }
    public string? PersonBase64Image { get; set; }
    public string? Asili { get; set; }
    public bool? CovidBagisik { get; set; }
    public DateTime? SonTestZamani { get; set; }
    public string? HesDurumu { get; set; }
}