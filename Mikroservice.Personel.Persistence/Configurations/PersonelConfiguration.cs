using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Personel.Persistence.Configurations
{
    public class PersonelConfiguration : IEntityTypeConfiguration<Microservice.Personel.Domain.Entities.Personel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Microservice.Personel.Domain.Entities.Personel> builder)
        {
            builder.HasKey(o => o.Id);
         
            builder.Property(o => o.DogumTarihi).IsRequired().HasColumnType("timestamp without time zone");
       

            builder.Property(p => p.SonGuncellemeTarihi)
       .HasColumnType("timestamp without time zone");

            builder.Property(p => p.KurumdanAyrilisTarihi)
                .HasColumnType("timestamp without time zone");

            builder.Property(p => p.GoreveBaslamaTarihi)
                .HasColumnType("timestamp without time zone");
        }
    }
}
