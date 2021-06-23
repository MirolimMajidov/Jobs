using AutoMapper;
using IdentityService.Models;

namespace IdentityService.Mapping
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            //DTO to Model 
            CreateMap<UserDTO, User>()
                .ForMember(dto => dto.HashPassword, opt => opt.MapFrom(src => src.Password));

            //Model to DTO
            CreateMap<User, UserDTO>();
        }
    }
}
