using AutoMapper;
using Mikroservice.Site.Application.DTOs.UnvanDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class UnvanMapping:Profile
    {
        public UnvanMapping()
        {
            // READ
            CreateMap<Unvan, UnvanDto>();
            CreateMap<Unvan, UnvanDetailDto>();
        }
    }
}
