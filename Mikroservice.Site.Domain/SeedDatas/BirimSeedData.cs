using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class BirimSeedData
    {
        public static List<Birim> YoneticiTipleri => new List<Birim>
 {
            new Birim { Id = 1, ParentId = null, Ad = "Rektörlük", Sira=1 },
            new Birim { Id = 2, ParentId = 1, Ad = "Bilgi İşlem Daire Başkanlığı", Sira=2}
        };

        public static List<Birim> GetBirimSeedDatas()
        {
            return YoneticiTipleri;
        }
    }
}
