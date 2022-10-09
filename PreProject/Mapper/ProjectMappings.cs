using AutoMapper;
using PreProject.Models;
using PreProject.Models.DTOs;

namespace PreProject.Mapper
{
    public class ProjectMappings : Profile
    {
        public ProjectMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        }
    }
}
