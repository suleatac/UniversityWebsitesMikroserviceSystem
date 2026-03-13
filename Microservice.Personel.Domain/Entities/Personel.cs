using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Domain.Entities
{
    public class Personel
    {
        public int id { get; set; }
        public int personid { get; set; } = default!;
        public string tcnumarasi { get; set; } = default!;
        public string adi { get; set; } = default!;
        public string soyadi { get; set; } = default!;
        public string uyruk { get; set; } = default!;
        public string cinsiyeti { get; set; } = default!;
        public string? eposta { get; set; }
        public string? telefoncep { get; set; }
        public string? telefondahili { get; set; }
        public string? telefondahilinumara { get; set; }
        public string? adres { get; set; }
        public string babaadi { get; set; } = default!;
        public string anaadi { get; set; } = default!;
        public string dogumyeri { get; set; } = default!;
        public DateTime dogumtarihi { get; set; } = default!;
        public string? kangrubu { get; set; }
        public string? sehitgaziyakini { get; set; }
        public int? personeltipiid { get; set; }
        public string? personeltipi { get; set; }
        public int? gorevyeriid { get; set; }
        public string? gorevyeri { get; set; }
        public DateTime? gorevebaslamatarihi { get; set; }
        public DateTime? kurumdanayrilistarihi { get; set; }
        public int? kadrotipiid { get; set; }
        public string kadrotipi { get; set; } = default!;
        public string? kadrokodu { get; set; }
        public string? idarigorevler { get; set; }
        public string? iliskilioldugupozisyonlar { get; set; }
        public string? emeklisicilkodu { get; set; }
        public int? unvanid { get; set; }
        public string? asliunvan { get; set; }
        public int? ekgosterge { get; set; }
        public int? gorevunvaniid { get; set; }
        public string? gorevunvan { get; set; }
        public int? kadrounvanid { get; set; }
        public string kadrounvan { get; set; } = default!;
        public string? kurumsicilno { get; set; }
        public int? ustgorevyeriid { get; set; }
        public string? ustgorevyeriadi { get; set; }
        public int? ustgorevbirimid { get; set; }
        public string? ustgorevbirimadi { get; set; }
        public int? kadrobirimid { get; set; }
        public string? kadrobirimi { get; set; }
        public int? kadroustbirimid { get; set; }
        public string? kadroustbirim { get; set; }
        public string? username { get; set; }
        public string personencryptedid { get; set; } = default!;
        public decimal? brutucret { get; set; }
        public DateTime? songuncellemetarihi { get; set; }
        public string? kisiseleposta { get; set; }
        public string? kisiseltelefon { get; set; }
        public bool? aktif { get; set; }
        public string? personbase64imagemodifiedon { get; set; }
        public string? personbase64image { get; set; }
        public string? asili { get; set; }

    }
}
