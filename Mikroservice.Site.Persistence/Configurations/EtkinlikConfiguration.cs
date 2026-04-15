using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class EtkinlikConfiguration : IEntityTypeConfiguration<Etkinlik>
    {
        public void Configure(EntityTypeBuilder<Etkinlik> builder)
        {


        }
    }
}
