
using Microsoft.EntityFrameworkCore;

namespace Microservice.Ogrenci.Domain.Entities
{
    [Index(nameof(ogrenciprogramid), IsUnique = true)]
    [Index(nameof(adi), nameof(soyadi))]
    [Index(nameof(eposta))]
    [Index(nameof(username))]
    [Index(nameof(songuncellemetarihi))]
    public class Ogrenci
    {
        public int id { get; set; }
        public int ogrenciprogramid { get; set; }
        public string? tcnumarasi { get; set; }
        public string? uyruk { get; set; }
        public string? adi { get; set; }
        public string? soyadi { get; set; }
        public string? cinsiyeti { get; set; }
        public string? eposta { get; set; }
        public string? telefoncep { get; set; }
        public string? adres { get; set; }
        public string? babaadi { get; set; }
        public string? anaadi { get; set; }
        public string? dogumyeri { get; set; }
        public DateTime dogumtarihi { get; set; }
        public string? kangrubu { get; set; }
        public string? sehitgaziyakini { get; set; }
        public string? resimkodu { get; set; }
        public Nullable<int> fakulteid { get; set; }
        public string? fakulte { get; set; }
        public string? bolum { get; set; }
        public string? fakulteingilizceadi { get; set; }
        public string? bolumingilizceadi { get; set; }
        public Nullable<int> sinif { get; set; }
        public string? studentno { get; set; }
        public string? akademikprogram { get; set; }
        public string? programtipi { get; set; }
        public string? scholarship { get; set; }
        public string? durum { get; set; }
        public string? ogretimtipi { get; set; }
        public string? programturu { get; set; }
        public Nullable<decimal> transcriptnotortalamasi { get; set; }
        public DateTime kayittarihi { get; set; }
        public Nullable<int> bolumid { get; set; }
        public Nullable<int> yoksisogrenciid { get; set; }
        public Nullable<int> yoksisid { get; set; }
        public string? username { get; set; }
        public DateTime songuncellemetarihi { get; set; }
        public string? kisiseleposta { get; set; }
        public string? kisiseltelefon { get; set; }
        public string? personbase64image { get; set; }
    }
}
