using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Persistence;

namespace Microservice.Site.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BandLogo> BandLogos { get; set; }
        public DbSet<Banner> Bannerler { get; set; }
        public DbSet<Bilgi> Bilgiler { get; set; }
        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Dil> Diller { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Haber> Haberler { get; set; }
        public DbSet<Hedef> Hedefler { get; set; }
        public DbSet<Menu> Menuler { get; set; }
        public DbSet<PersonelTelefon> PersonelTelefonlar { get; set; }
        public DbSet<PersonelTip> PersonelTipleri { get; set; }
        public DbSet<Popup> Popuplar { get; set; }
        public DbSet<SikcaSorulanSoru> SikcaSorulanSorular { get; set; }
        public DbSet<Mikroservice.Site.Domain.Entities.Site> Siteler { get; set; }
        public DbSet<SiteOzellikleri> SiteOzellikleri { get; set; }
        public DbSet<SitePersonel> SitePersonelleri { get; set; }
        public DbSet<Template> Templateler { get; set; }
        public DbSet<Unvan> Unvanlar { get; set; }
        public DbSet<Video> Videolar { get; set; }
        public DbSet<YoneticiSite> YoneticiSiteler { get; set; }
        public DbSet<YonetimDuyuru> YonetimDuyurular { get; set; }
        public DbSet<YonetimDuyuruOkundu> YonetimDuyuruOkunduBilgileri { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
            base.OnModelCreating(modelBuilder);
        }





    }
}
