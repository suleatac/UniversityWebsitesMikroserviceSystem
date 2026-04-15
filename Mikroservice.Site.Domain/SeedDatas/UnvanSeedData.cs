using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class UnvanSeedData
    {
        public static List<Unvan> Unvanlar => new List<Unvan>
        {
            new Unvan { Id = 1, TipId = 1, Ad = "REKTÖR",KisaAd="Rektör",Sira=1 },
            new Unvan { Id = 2, TipId = 1, Ad = "REKTÖR YARDIMCISI",KisaAd="Rektör Yrd.",Sira=2 },
            new Unvan { Id = 3, TipId = 1, Ad = "REKTÖR YARDIMCISI",KisaAd="Rektör Yrd.",Sira=2 }
        };

        public static List<Unvan> GetUnvanSeedDatas()
        {
            return Unvanlar;
        }
    }
}
