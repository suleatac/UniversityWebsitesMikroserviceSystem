namespace Microservice.Admin.ViewModels.SitePersonel
{
    public class CreateSitePersonelVm
    {
        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PersonelTipId { get; set; }

        public string? ResimUrl { get; set; } 
        public string? IlgiAlanlari { get; set; } 
        public string? BlogAdress { get; set; } 
        public string? TwitterAdress { get; set; } 
        public string? FacebookAdress { get; set; } 
        public string? InstagramAdress { get; set; } 
        public string? GoogleplusAdress { get; set; } 
        public string? Hakkinda { get; set; } 
        public string? DeneyimVeCalismalari { get; set; } 
    }
}