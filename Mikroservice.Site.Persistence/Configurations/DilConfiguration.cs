using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class DilConfiguration : IEntityTypeConfiguration<Dil>
    {
        public void Configure(EntityTypeBuilder<Dil> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(d => d.Ad).IsRequired().HasMaxLength(200);
            builder.Property(d => d.InternationalAd).IsRequired().HasMaxLength(200);
            builder.Property(d => d.Kod).IsRequired().HasMaxLength(10);
            builder.Property(d => d.IsActive).IsRequired();
        }
    }
}
