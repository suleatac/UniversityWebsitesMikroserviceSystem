using AutoMapper;
using Mikroservice.Site.Application.DTOs.VideoDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class VideoMapping : Profile
    {
        public VideoMapping()
        {
            CreateMap<Video, VideoDto>();
            CreateMap<Video, VideoDetailDto>();
        }
    }
}