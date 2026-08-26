using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Domain.SeedDatas
{
    public class PersonelTipSeedData
    {
        public static List<PersonelTip> PersonelTipler => new List<PersonelTip>
{
            new PersonelTip {  Ad = "İdari"},
            new PersonelTip {  Ad = "Akademik"}
        };

        public static List<PersonelTip> GetYoneticiTipiSeedDatas()
        {
            return PersonelTipler;
        }
    }
}
