using AutoMapper;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class SitePersonelMapping : Profile
    {
        public SitePersonelMapping()
        {

            CreateMap<SitePersonel, SitePersonelDetailDto>()
                .ForMember(dest => dest.UnvanAd, opt => opt.MapFrom(src => src.Unvan != null ? src.Unvan.Ad : null))
                .ForMember(dest => dest.PersonelTipAd, opt => opt.MapFrom(src => src.PersonelTip != null ? src.PersonelTip.Ad : null));
        }
    }
}
