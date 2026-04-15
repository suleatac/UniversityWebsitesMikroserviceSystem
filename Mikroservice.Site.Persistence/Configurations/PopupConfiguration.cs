using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PopupConfiguration : IEntityTypeConfiguration<Popup>
    {
        public void Configure(EntityTypeBuilder<Popup> builder)
        {
            builder.Property(x => x.TamEkranMi)
                .HasDefaultValue(false);

            builder.Property(x => x.GosterimSuresiSaniye)
                .HasDefaultValue(5);
        }
    }
}
