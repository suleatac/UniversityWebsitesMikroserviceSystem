using Microservice.Personel.Domain.Entities;

namespace Microservice.Personel.Domain.SeedData;

/// <summary>
/// Personel entity'si için örnek test verileri sağlar
/// </summary>
public static class PersonelSeedData
{
    /// <summary>
    /// Örnek personel 1: Akademik personel - Profesör
    /// </summary>
    public static Entities.Personel Personel1 => new Entities.Personel
    {
        Id = 1,
        PersonId = 1001,
        TcNumarasi = "11122233344",
        Adi = "Mehmet",
        Soyadi = "Demir",
        Uyruk = "T.C.",
        Cinsiyeti = "Erkek",
        Eposta = "mehmet.demir@university.edu.tr",
        TelefonCep = "+90 532 111 2233",
        TelefonDahili = "1234",
        TelefonDahiliNumara = "1234",
        Adres = "Bahçelievler Mahallesi, Beşevler Caddesi No:78, Ankara",
        BabaAdi = "Hasan",
        AnaAdi = "Emine",
        DogumYeri = "İstanbul",
        DogumTarihi = new DateTime(1975, 3, 10),
        KanGrubu = "AB+",
        SehitGaziYakini = "Hayır",
        PersonelTipiId = 1,
        PersonelTipi = "Akademik",
        GorevYeriId = 1,
        GorevYeri = "Mühendislik Fakültesi",
        GoreveBaslamaTarihi = new DateTime(2005, 9, 1),
        KadroTipiId = 1,
        KadroTipi = "Kadrolu",
        KadroKodu = "A-001",
        IdariGorevler = "Bölüm Başkanı",
        IliskiliOlduguPozisyonlar = "Akademik Kurul Üyesi",
        EmekliSicilKodu = "ESK-001",
        UnvanId = 1,
        AsliUnvan = "Prof. Dr.",
        EkGosterge = 1200,
        GorevUnvaniId = 1,
        GorevUnvan = "Profesör",
        KadroUnvanId = 1,
        KadroUnvan = "Profesör",
        KurumSicilNo = "P-2005-001",
        UstGorevYeriId = 1,
        UstGorevYeriAdi = "Üniversite Rektörlüğü",
        UstGorevBirimId = 1,
        UstGorevBirimAdi = "Mühendislik Fakültesi Dekanlığı",
        KadroBirimId = 10,
        KadroBirimi = "Bilgisayar Mühendisliği Bölümü",
        KadroUstBirimId = 1,
        KadroUstBirim = "Mühendislik Fakültesi",
        Username = "mehmet.demir",
        PersonEncryptedId = "encrypted_1001",
        BrutUcret = 45000.00m,
        SonGuncellemeTarihi = DateTime.Now,
        KisiselEposta = "mehmet.demir@gmail.com",
        KisiselTelefon = "+90 532 111 2233",
        Aktif = true,
        PersonBase64ImageModifiedOn = DateTime.Now.ToString("yyyy-MM-dd"),
        PersonBase64Image = null,
        Asili = "Evet",
        CovidBagisik = true,
        SonTestZamani = DateTime.Now.AddMonths(-2),
        HesDurumu = "Mavi"
    };

        /// <summary>
        /// Örnek personel 2: Akademik personel - Doçent
        /// </summary>
        public static Entities.Personel Personel2 => new Entities.Personel
        {
            Id = 2,
            PersonId = 1002,
            TcNumarasi = "55566677788",
            Adi = "Ayşe",
            Soyadi = "Yıldırım",
            Uyruk = "T.C.",
            Cinsiyeti = "Kadın",
            Eposta = "ayse.yildirim@university.edu.tr",
            TelefonCep = "+90 505 444 5566",
            TelefonDahili = "5678",
            TelefonDahiliNumara = "5678",
            Adres = "Çankaya Mahallesi, Atatürk Bulvarı No:156, Ankara",
            BabaAdi = "Mustafa",
            AnaAdi = "Zehra",
            DogumYeri = "Ankara",
            DogumTarihi = new DateTime(1982, 7, 20),
            KanGrubu = "A+",
            SehitGaziYakini = "Hayır",
            PersonelTipiId = 1,
            PersonelTipi = "Akademik",
            GorevYeriId = 2,
            GorevYeri = "Fen Bilimleri Enstitüsü",
            GoreveBaslamaTarihi = new DateTime(2012, 2, 15),
            KadroTipiId = 1,
            KadroTipi = "Kadrolu",
            KadroKodu = "A-002",
            IdariGorevler = "Enstitü Müdür Yardımcısı",
            IliskiliOlduguPozisyonlar = "Yönetim Kurulu Üyesi",
            EmekliSicilKodu = "ESK-002",
            UnvanId = 2,
            AsliUnvan = "Doç. Dr.",
            EkGosterge = 1000,
            GorevUnvaniId = 2,
            GorevUnvan = "Doçent",
            KadroUnvanId = 2,
            KadroUnvan = "Doçent",
            KurumSicilNo = "P-2012-045",
            UstGorevYeriId = 1,
            UstGorevYeriAdi = "Üniversite Rektörlüğü",
            UstGorevBirimId = 2,
            UstGorevBirimAdi = "Fen Bilimleri Enstitüsü Müdürlüğü",
            KadroBirimId = 25,
            KadroBirimi = "Yazılım Mühendisliği Anabilim Dalı",
            KadroUstBirimId = 2,
            KadroUstBirim = "Fen Bilimleri Enstitüsü",
            Username = "ayse.yildirim",
            PersonEncryptedId = "encrypted_1002",
            BrutUcret = 38000.00m,
            SonGuncellemeTarihi = DateTime.Now.AddDays(-3),
            KisiselEposta = "ayse.yildirim@outlook.com",
            KisiselTelefon = "+90 505 444 5566",
            Aktif = true,
            PersonBase64ImageModifiedOn = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"),
            PersonBase64Image = null,
            Asili = "Evet",
            CovidBagisik = true,
            SonTestZamani = DateTime.Now.AddMonths(-1),
            HesDurumu = "Mavi"
        };

            /// <summary>
            /// Örnek personel 3: İdari personel
            /// </summary>
            public static Entities.Personel Personel3 => new Entities.Personel
            {
                Id = 3,
                PersonId = 1003,
                TcNumarasi = "99988877766",
                Adi = "Can",
                Soyadi = "Öztürk",
                Uyruk = "T.C.",
                Cinsiyeti = "Erkek",
                Eposta = "can.ozturk@university.edu.tr",
                TelefonCep = "+90 533 777 8899",
                TelefonDahili = "9012",
                TelefonDahiliNumara = "9012",
                Adres = "Kızılay Mahallesi, Gazi Caddesi No:234, Ankara",
                BabaAdi = "Ahmet",
                AnaAdi = "Sevgi",
                DogumYeri = "Bursa",
                DogumTarihi = new DateTime(1988, 12, 5),
                KanGrubu = "0+",
                SehitGaziYakini = "Hayır",
                PersonelTipiId = 2,
                PersonelTipi = "İdari",
                GorevYeriId = 1,
                GorevYeri = "Mühendislik Fakültesi",
                GoreveBaslamaTarihi = new DateTime(2015, 6, 1),
                KadroTipiId = 2,
                KadroTipi = "4/C Sözleşmeli",
                KadroKodu = "I-001",
                IdariGorevler = "Öğrenci İşleri Sorumlusu",
                IliskiliOlduguPozisyonlar = "Öğrenci İşleri Müdürlüğü",
                EmekliSicilKodu = "ESK-003",
                UnvanId = 10,
                AsliUnvan = "Memur",
                EkGosterge = 0,
                GorevUnvaniId = 10,
                GorevUnvan = "Memur",
                KadroUnvanId = 10,
                KadroUnvan = "Memur",
                KurumSicilNo = "P-2015-089",
                UstGorevYeriId = 1,
                UstGorevYeriAdi = "Üniversite Rektörlüğü",
                UstGorevBirimId = 1,
                UstGorevBirimAdi = "Mühendislik Fakültesi Dekanlığı",
                KadroBirimId = 50,
                KadroBirimi = "Öğrenci İşleri",
                KadroUstBirimId = 1,
                KadroUstBirim = "Mühendislik Fakültesi",
                Username = "can.ozturk",
                PersonEncryptedId = "encrypted_1003",
                BrutUcret = 18000.00m,
                SonGuncellemeTarihi = DateTime.Now.AddDays(-1),
                KisiselEposta = "can.ozturk@hotmail.com",
                KisiselTelefon = "+90 533 777 8899",
                Aktif = true,
                PersonBase64ImageModifiedOn = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                PersonBase64Image = null,
                Asili = "Evet",
                CovidBagisik = false,
                SonTestZamani = DateTime.Now.AddMonths(-3),
                HesDurumu = "Yeşil"
            };

            /// <summary>
            /// Tüm örnek personelleri içeren liste
            /// </summary>
            public static List<Entities.Personel> OrnekPersoneller => new List<Entities.Personel>
            {
                Personel1,
                Personel2,
                Personel3
            };

            /// <summary>
            /// Örnek personelleri döndürür (metod versiyonu)
            /// </summary>
            public static List<Entities.Personel> GetOrnekPersoneller()
            {
                return OrnekPersoneller;
            }
        }
