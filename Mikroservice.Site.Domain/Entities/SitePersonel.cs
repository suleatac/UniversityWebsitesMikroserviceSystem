namespace Mikroservice.Site.Domain.Entities
{
    public class SitePersonel
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PersonelTipId { get; set; }
        public string ResimUrl { get; set; } = default!;
        public string IlgiAlanlari { get; set; } = default!;
        public string BlogAdress { get; set; } = default!;
        public string TwitterAdress { get; set; } = default!;
        public string FacebookAdress { get; set; } = default!;
        public string InstagramAdress { get; set; } = default!;
        public string GoogleplusAdress { get; set; } = default!;
        public string Hakkinda { get; set; } = default!;
        public string DeneyimVeCalismalari { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;

        public PersonelTip PersonelTip { get; set; } = default!; // 🔥 bool yerine

        // NAVIGATION
        public Site Site { get; set; } = default!;
        public Unvan Unvan { get; set; } = default!;
        public ICollection<PersonelTelefon> PersonelTelefons { get; set; } = new List<PersonelTelefon>();
    }
}
