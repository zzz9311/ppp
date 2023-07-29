using AutoMapper;
using DAL.Entities;
using DAL.Models;
using PetPPP.BLL.Interfaces.DTO;
using PetPPP.BLL.Interfaces.Users;

namespace PetPPP.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, UserChangeableDTO>();
            CreateMap<LoginModel, LoginDTO>();
            CreateMap<UserChangeableDTO, AppUser>().ReverseMap();
        }
    }
}
