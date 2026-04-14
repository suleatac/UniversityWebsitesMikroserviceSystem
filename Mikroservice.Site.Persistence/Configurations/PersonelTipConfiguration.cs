using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PersonelTipConfiguration:IEntityTypeConfiguration<PersonelTip>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PersonelTip> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(150);
            // 🔥 UNIQUE constraint (çok önemli)
            builder.HasIndex(x => x.Ad)
                .IsUnique();

            // 1 → N ilişki
            builder.HasMany(x => x.SitePersonels)
                .WithOne(x => x.PersonelTip)
                .HasForeignKey(x => x.PersonelTipId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
