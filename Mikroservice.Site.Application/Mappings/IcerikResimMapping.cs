using AutoMapper;
using Mikroservice.Site.Application.DTOs.IcerikResimDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class IcerikResimMapping : Profile
    {
        public IcerikResimMapping()
        {
            // Duyuru/Haber detay DTO'larindaki Resimler kumesi icin gerekli
            CreateMap<IcerikResim, IcerikResimDto>();
        }
    }
}
