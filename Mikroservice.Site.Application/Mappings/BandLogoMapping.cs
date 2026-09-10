using AutoMapper;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class BandLogoMapping : Profile
    {
        public BandLogoMapping()
        {
            CreateMap<BandLogo, BandLogoDto>();
            CreateMap<BandLogo, BandLogoDetailDto>();
        }
    }
}
