using AutoMapper;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class ShortcutButtonMapping : Profile
    {
        public ShortcutButtonMapping()
        {
            // READ
            CreateMap<ShortcutButton, ShortcutButtonDetailDto>();
            CreateMap<ShortcutButton, ShortcutButtonDto>();
        }
    }
}
