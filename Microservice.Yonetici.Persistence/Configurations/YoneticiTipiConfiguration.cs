using Microservice.Yonetici.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Yonetici.Persistence.Configurations
{
    public class YoneticiTipiConfiguration : IEntityTypeConfiguration<YoneticiTipi>
    {
        public void Configure(EntityTypeBuilder<YoneticiTipi> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.TipAdi).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Value).IsRequired();
        }
    }
}
