using AutoMapper;
using Mikroservice.Site.Application.DTOs.SikcaSorulanSoruDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class SikcaSorulanSoruMapping : Profile
    {
        public SikcaSorulanSoruMapping()
        {
            // READ
            CreateMap<SikcaSorulanSoru, SikcaSorulanSoruDto>();
            CreateMap<SikcaSorulanSoru, SikcaSorulanSoruDetailDto>();
        }
    }
}
