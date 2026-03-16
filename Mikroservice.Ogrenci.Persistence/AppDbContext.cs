using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Ogrenci.Persistence
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Microservice.Ogrenci.Domain.Entities.Ogrenci> Ogrencis { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
