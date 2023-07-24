using AutoMapper;
using DAL.Entities;
using DAL.Models;
using PetPPP.BLL.Interfaces.DTO;

namespace PetPPP.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, AppUserDTO>();
            CreateMap<LoginModel, LoginDTO>();
            CreateMap<AppUserDTO, AppUser>();
        }
    }
}
