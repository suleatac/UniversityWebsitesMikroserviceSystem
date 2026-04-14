namespace Microservice.Site.Domain.SeedDatas
{
    public class YoneticiTipiSeedData
    {

        public static List<Entities.YoneticiTipi> YoneticiTipleri => new List<Entities.YoneticiTipi>
        {
            new Entities.YoneticiTipi { Id = 1, TipAdi = "Admin", Value = 1 },
            new Entities.YoneticiTipi { Id = 2, TipAdi = "Birim Web Admini", Value = 2 }
        };

        public static List<Entities.YoneticiTipi> GetYoneticiTipiSeedDatas()
        {
            return YoneticiTipleri;
        }

    }
}
