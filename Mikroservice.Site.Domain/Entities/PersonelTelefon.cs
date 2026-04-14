namespace Mikroservice.Site.Domain.Entities
{
    public class PersonelTelefon
    {
        public int Id { get; set; }
        public int SitePersonelId { get; set; }
        public string TelefonNo { get; set; } = default!;

        // NAVIGATION
        public SitePersonel SitePersonel { get; set; } = default!;

    }
}
