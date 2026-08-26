using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class HedefSeedData
    {
        public static List<Hedef> Hedefler => new List<Hedef>
        {
            new Hedef {  Tag = "_self", Aciklama = "Aynı pencerede aç" },
            new Hedef {  Tag = "_blank", Aciklama = "Yeni pencerede aç" }
        };

        public static List<Hedef> GetHedefSeedDatas()
        {
            return Hedefler;
        }
    }
}
