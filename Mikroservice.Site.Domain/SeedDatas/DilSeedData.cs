using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class DilSeedData
    {
        public static List<Dil> Diller => new List<Dil>
       {
            new Dil { Id = 1, Ad = "Türkçe", InternationalAd = "Turkish", Kod = "TR" },
            new Dil { Id = 2, Ad = "İngilizce", InternationalAd = "English", Kod = "EN" }
        };

        public static List<Dil> GetDilSeedDatas()
        {
            return Diller;
        }
    }
}
