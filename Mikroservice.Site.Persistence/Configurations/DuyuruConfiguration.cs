using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class DuyuruConfiguration : IEntityTypeConfiguration<Duyuru>
    {
        public void Configure(EntityTypeBuilder<Duyuru> builder)
        {

        }
    }
}
