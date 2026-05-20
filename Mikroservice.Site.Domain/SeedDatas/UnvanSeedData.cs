using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class UnvanSeedData
    {
        public static List<Unvan> Unvanlar => new List<Unvan>
        {
            new Unvan { Id = 1, TipId = 1, Ad = "REKTÖR",KisaAd="Rektör",Sira=1 }
        };

        public static List<Unvan> GetUnvanSeedDatas()
        {
            return Unvanlar;
        }
    }
}
