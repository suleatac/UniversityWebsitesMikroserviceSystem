using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Persistence;

namespace Microservice.Site.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<YoneticiTipi> YoneticiTipleri { get; set; }
        public DbSet<YonetimDuyuru> YoneticiDuyurular { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
            base.OnModelCreating(modelBuilder);
        }






    }
}
