using AutoMapper;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class DuyuruMapping : Profile
    {
        public DuyuruMapping()
        {
            CreateMap<Duyuru, DuyuruDto>();
            CreateMap<Duyuru, DuyuruDetailDto>();
        }
    }
}