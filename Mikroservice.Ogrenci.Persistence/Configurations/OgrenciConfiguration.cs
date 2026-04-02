using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Persistence.Configurations
{
    public class OgrenciConfiguration : IEntityTypeConfiguration<Microservice.Ogrenci.Domain.Entities.Ogrenci>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Microservice.Ogrenci.Domain.Entities.Ogrenci> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OgrenciProgramId);
           
            builder.Property(o => o.DogumTarihi).HasColumnType("timestamp without time zone");

            builder.Property(p => p.SonGuncellemeTarihi).HasColumnType("timestamp without time zone");

            builder.Property(p => p.KayitTarihi).HasColumnType("timestamp without time zone");
            builder.Property(p => p.MezuniyetTarihi).HasColumnType("timestamp without time zone");
            builder.Property(p => p.IlisikKesmeTarihi).HasColumnType("timestamp without time zone");

        }
    }
}
