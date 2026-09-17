using AutoMapper;
using Mikroservice.Site.Application.DTOs.IcerikDosyaDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class IcerikDosyaMapping : Profile
    {
        public IcerikDosyaMapping()
        {
            // Duyuru/Haber detay DTO'larindaki Dosyalar kumesi icin gerekli
            CreateMap<IcerikDosya, IcerikDosyaDto>();
        }
    }
}
