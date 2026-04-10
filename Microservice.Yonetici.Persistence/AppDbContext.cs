using Microservice.Yonetici.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Yonetici.Persistence;

namespace Microservice.Yonetici.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<YoneticiTipi> YoneticiTipleri { get; set; }
        public DbSet<YoneticiDuyuru> YoneticiDuyurular { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
            base.OnModelCreating(modelBuilder);
        }






    }
}
