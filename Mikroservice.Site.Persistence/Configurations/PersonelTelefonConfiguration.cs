using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PersonelTelefonConfiguration : IEntityTypeConfiguration<PersonelTelefon>
    {
        public void Configure(EntityTypeBuilder<PersonelTelefon> builder)
        {
            builder.HasKey(x => x.Id);
            // =========================
            // Telefon (ZORUNLU)
            // =========================
            builder.HasOne(x => x.SitePersonel)
                .WithMany(x => x.PersonelTelefons)
                .HasForeignKey(x => x.SitePersonelId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.TelefonNo)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
