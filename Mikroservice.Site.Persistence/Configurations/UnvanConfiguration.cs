using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class UnvanConfiguration:IEntityTypeConfiguration<Unvan>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Unvan> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.KisaAd)
                .HasMaxLength(50);

            builder.Property(x => x.Sira)
                .IsRequired();



        }
    }
}
