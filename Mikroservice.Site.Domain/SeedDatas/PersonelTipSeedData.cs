using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class PersonelTipSeedData
    {
        public static List<PersonelTip> PersonelTipler => new List<PersonelTip>
{
            new PersonelTip { Id = 1, Ad = "İdari"},
            new PersonelTip { Id = 2, Ad = "Akademik"}
        };

        public static List<PersonelTip> GetYoneticiTipiSeedDatas()
        {
            return PersonelTipler;
        }
    }
}
