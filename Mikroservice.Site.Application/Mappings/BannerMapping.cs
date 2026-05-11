using AutoMapper;
using Mikroservice.Site.Application.DTOs.BannerDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class BannerMapping : Profile
    {
        public BannerMapping()
        {
            CreateMap<Banner, BannerDto>();
            CreateMap<Banner, BannerDetailDto>();
        }
    }
}