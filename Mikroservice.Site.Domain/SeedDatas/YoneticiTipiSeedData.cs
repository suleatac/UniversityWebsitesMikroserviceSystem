using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Domain.SeedDatas
{
    public class YoneticiTipiSeedData
    {

        public static List<YoneticiTipi> YoneticiTipleri => new List<YoneticiTipi>
        {
            new YoneticiTipi { Id = 1, TipAdi = "Admin", Value = 1 },
            new YoneticiTipi { Id = 2, TipAdi = "Birim Web Admini", Value = 2 }
        };

        public static List<YoneticiTipi> GetYoneticiTipiSeedDatas()
        {
            return YoneticiTipleri;
        }

    }
}
