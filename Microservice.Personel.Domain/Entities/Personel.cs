using Microsoft.EntityFrameworkCore;

namespace Microservice.Personel.Domain.Entities
{
   
    [Index(nameof(adi), nameof(soyadi))]
    [Index(nameof(eposta))]
    [Index(nameof(username))]
    [Index(nameof(songuncellemetarihi))]
    public class Personel
    {
        public int id { get; set; }
        public Nullable<int> personid { get; set; }
        public string? tcnumarasi { get; set; }
        public string? adi { get; set; }
        public string? soyadi { get; set; }
        public string? uyruk { get; set; }
        public string? cinsiyeti { get; set; }
        public string? eposta { get; set; }
        public string? telefoncep { get; set; }
        public string? telefondahili { get; set; }
        public string? telefondahilinumara { get; set; }
        public string? adres { get; set; }
        public string? babaadi { get; set; }
        public string? anaadi { get; set; }
        public string? dogumyeri { get; set; }
        public DateTime dogumtarihi { get; set; }
        public string? kangrubu { get; set; }
        public string? sehitgaziyakini { get; set; }
        public Nullable<int> personeltipiid { get; set; }
        public string? personeltipi { get; set; }
        public Nullable<int> gorevyeriid { get; set; }
        public string? gorevyeri { get; set; }
        public DateTime? gorevebaslamatarihi { get; set; }
        public DateTime? kurumdanayrilistarihi { get; set; }
        public Nullable<int> kadrotipiid { get; set; }
        public string? kadrotipi { get; set; }
        public string? kadrokodu { get; set; }
        public string? idarigorevler { get; set; }
        public string? iliskilioldugupozisyonlar { get; set; }
        public string? emeklisicilkodu { get; set; }
        public Nullable<int> unvanid { get; set; }
        public string? asliunvan { get; set; }
        public Nullable<int> ekgosterge { get; set; }
        public Nullable<int> gorevunvaniid { get; set; }
        public string? gorevunvan { get; set; }
        public Nullable<int> kadrounvanid { get; set; }
        public string? kadrounvan { get; set; }
        public string? kurumsicilno { get; set; }
        public Nullable<int> ustgorevyeriid { get; set; }
        public string? ustgorevyeriadi { get; set; }
        public Nullable<int> ustgorevbirimid { get; set; }
        public string? ustgorevbirimadi { get; set; }
        public Nullable<int> kadrobirimid { get; set; }
        public string? kadrobirimi { get; set; }
        public Nullable<int> kadroustbirimid { get; set; }
        public string? kadroustbirim { get; set; }
        public string? username { get; set; }
        public string? personencryptedid { get; set; }
        public Nullable<decimal> brutucret { get; set; }
        public DateTime? songuncellemetarihi { get; set; }
        public string? kisiseleposta { get; set; }
        public string? kisiseltelefon { get; set; }
        public Nullable<bool> aktif { get; set; }
        public string? personbase64imagemodifiedon { get; set; }
        public string? personbase64image { get; set; }
        public string? asili { get; set; }

    }
}
