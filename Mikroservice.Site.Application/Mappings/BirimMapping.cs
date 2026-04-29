using AutoMapper;
using Mikroservice.Site.Application.DTOs.BirimDtos;

namespace Mikroservice.Site.Application.Mappings
{
    public class BirimMapping:Profile
    {
        public BirimMapping()
        {
            // READ
            CreateMap<Domain.Entities.Birim, BirimDto>();
            CreateMap<Domain.Entities.Birim, BirimDetailDto>();
        }
    }
}
