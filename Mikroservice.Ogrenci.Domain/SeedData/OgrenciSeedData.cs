using Microservice.Ogrenci.Domain.Entities;
namespace Microservice.Ogrenci.Domain.SeedData
{
    /// <summary>
    /// Ogrenci entity'si için örnek test verileri sağlar
    /// </summary>
    public static class OgrenciSeedData
    {
        /// <summary>
        /// Örnek öğrenci 1: Lisans öğrencisi
        /// </summary>
        public static Entities.Ogrenci Ogrenci1 => new Entities.Ogrenci
        {
            Id = 1,
            Username = "ahmet.yilmaz",
            SonGuncellemeTarihi = DateTime.Now,
            KisiselEposta = "ahmet.yilmaz@gmail.com",
            KisiselTelefon = "+90 532 123 4567",
            OgrenciProgramId = 101,
            TcNumarasi = "12345678901",
            Uyruk = "T.C.",
            Adi = "Ahmet",
            Soyadi = "Yılmaz",
            Cinsiyeti = "Erkek",
            Eposta = "ahmet.yilmaz@university.edu.tr",
            TelefonCep = "+90 532 123 4567",
            Adres = "Atatürk Mahallesi, Cumhuriyet Caddesi No:45, Ankara",
            BabaAdi = "Mehmet",
            AnaAdi = "Ayşe",
            DogumYeri = "Ankara",
            DogumTarihi = new DateTime(2002, 5, 15),
            KanGrubu = "A+",
            SehitGaziYakini = "Hayır",
            ResimKodu = "IMG_001",
            FakulteId = 1,
            Fakulte = "Mühendislik Fakültesi",
            Bolum = "Bilgisayar Mühendisliği",
            FakulteIngilizceAdi = "Faculty of Engineering",
            BolumIngilizceAdi = "Computer Engineering",
            Sinif = 3,
            StudentNo = "2021010123",
            PersonBase64Image = null,
            AkademikProgram = "Lisans",
            ProgramTipi = "Normal Öğretim",
            Scholarship = "%50 Burslu",
            Durum = "Aktif",
            DurumDetail = "Öğrenim Devam Ediyor",
            MezuniyetTarihi = null,
            IlisikKesmeTarihi = null,
            OgretimTipi = "Örgün Öğretim",
            ProgramTuru = "Tam Zamanlı",
            TranscriptNotOrtalamasi = 3.45m,
            KayitTarihi = new DateTime(2021, 9, 20),
            BolumId = 10,
            YoksisOgrenciId = 1001,
            YoksisId = 2001
        };

        /// <summary>
        /// Örnek öğrenci 2: Yüksek lisans öğrencisi
        /// </summary>
        public static Entities.Ogrenci Ogrenci2 => new Entities.Ogrenci
        {
            Id = 2,
            Username = "zeynep.kara",
            SonGuncellemeTarihi = DateTime.Now.AddDays(-5),
            KisiselEposta = "zeynep.kara@outlook.com",
            KisiselTelefon = "+90 505 987 6543",
            OgrenciProgramId = 202,
            TcNumarasi = "98765432109",
            Uyruk = "T.C.",
            Adi = "Zeynep",
            Soyadi = "Kara",
            Cinsiyeti = "Kadın",
            Eposta = "zeynep.kara@university.edu.tr",
            TelefonCep = "+90 505 987 6543",
            Adres = "İnönü Bulvarı, Kızılay Mahallesi No:123, Ankara",
            BabaAdi = "Ali",
            AnaAdi = "Fatma",
            DogumYeri = "İzmir",
            DogumTarihi = new DateTime(1998, 11, 22),
            KanGrubu = "0+",
            SehitGaziYakini = "Hayır",
            ResimKodu = "IMG_002",
            FakulteId = 2,
            Fakulte = "Fen Bilimleri Enstitüsü",
            Bolum = "Yazılım Mühendisliği",
            FakulteIngilizceAdi = "Graduate School of Natural and Applied Sciences",
            BolumIngilizceAdi = "Software Engineering",
            Sinif = 1,
            StudentNo = "2023020045",
            PersonBase64Image = null,
            AkademikProgram = "Yüksek Lisans",
            ProgramTipi = "Tezli",
            Scholarship = "Tam Burslu",
            Durum = "Aktif",
            DurumDetail = "Öğrenim Devam Ediyor",
            MezuniyetTarihi = null,
            IlisikKesmeTarihi = null,
            OgretimTipi = "Örgün Öğretim",
            ProgramTuru = "Tam Zamanlı",
            TranscriptNotOrtalamasi = 3.82m,
            KayitTarihi = new DateTime(2023, 2, 15),
            BolumId = 25,
            YoksisOgrenciId = 1002,
            YoksisId = 2002
        };

        /// <summary>
        /// Tüm örnek öğrencileri içeren liste
        /// </summary>
        public static List<Entities.Ogrenci> OrnekOgrenciler => new List<Entities.Ogrenci>
        {
            Ogrenci1,
            Ogrenci2
        };

        /// <summary>
        /// Örnek öğrencileri döndürür (metod versiyonu)
        /// </summary>
        public static List<Entities.Ogrenci> GetOrnekOgrenciler()
        {
            return OrnekOgrenciler;
        }
    }
}
