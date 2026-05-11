using AutoMapper;
using Mikroservice.Site.Application.DTOs.PopupDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class PopupMapping : Profile
    {
        public PopupMapping()
        {
            CreateMap<Popup, PopupDto>();
            CreateMap<Popup, PopupDetailDto>();
        }
    }
}