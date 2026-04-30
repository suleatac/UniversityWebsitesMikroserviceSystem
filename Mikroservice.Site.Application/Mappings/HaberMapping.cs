using AutoMapper;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class HaberMapping:Profile
    {
        public HaberMapping()
        {
            // READ
            CreateMap<Haber, HaberDto>();
            CreateMap<Haber, HaberDetailDto>();
        }
    }
}
