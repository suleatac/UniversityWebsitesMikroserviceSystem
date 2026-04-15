using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class HedefConfiguration : IEntityTypeConfiguration<Hedef>
    {
        public void Configure(EntityTypeBuilder<Hedef> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Tag)
                    .IsRequired()
                    .HasMaxLength(100);
            builder.Property(h => h.Aciklama)
                    .HasMaxLength(500);
        }
    }
}
