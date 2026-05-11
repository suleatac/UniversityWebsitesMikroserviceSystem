namespace Microservice.Admin.ViewModels.SitePersonel
{
    public class CreateSitePersonelVm
    {
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
    }
}