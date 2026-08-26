using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class DilSeedData
    {
        public static List<Dil> Diller => new List<Dil>
       {
            new Dil {  Ad = "Türkçe", InternationalAd = "Turkish", Kod = "TR" },
            new Dil {  Ad = "İngilizce", InternationalAd = "English", Kod = "EN" }
        };

        public static List<Dil> GetDilSeedDatas()
        {
            return Diller;
        }
    }
}
