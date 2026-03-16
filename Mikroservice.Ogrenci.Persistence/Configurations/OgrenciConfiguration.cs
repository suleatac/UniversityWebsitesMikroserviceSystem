using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Persistence.Configurations
{
    public class PersonelConfiguration : IEntityTypeConfiguration<Microservice.Ogrenci.Domain.Entities.Ogrenci>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Microservice.Ogrenci.Domain.Entities.Ogrenci> builder)
        {
            builder.HasKey(o => o.id);
            builder.Property(o => o.ogrenciprogramid).IsRequired();
           
            builder.Property(o => o.dogumtarihi).IsRequired().HasColumnType("timestamp without time zone");
         


            builder.Property(p => p.songuncellemetarihi)
       .HasColumnType("timestamp without time zone");

            builder.Property(p => p.kayittarihi)
                .HasColumnType("timestamp without time zone");

        }
    }
}
