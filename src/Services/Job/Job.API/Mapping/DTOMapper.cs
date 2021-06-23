using AutoMapper;
using JobService.Models;

namespace JobService.Mapping
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Job, JobDTO>().ReverseMap();
        }
    }
}
