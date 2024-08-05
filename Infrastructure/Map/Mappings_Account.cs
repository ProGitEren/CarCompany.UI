using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models.ViewModels;

namespace Infrastructure.Map
{
    public class Mappings_Account : Profile
    {
        public Mappings_Account()
        {
            CreateMap<RegisterDto, RegisterViewModel>().ReverseMap();
            
            CreateMap<AddressDto,AddressViewModel>().ReverseMap();
            
            CreateMap<UserwithaddressDto, UserViewModel>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ReverseMap().ForMember(x => x.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<UsernotokenDto, UserViewModel>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ReverseMap().ForMember(x => x.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<UpdateUserDto, UpdateUserViewModel>().ReverseMap();
            CreateMap<UserDto, UpdateUserViewModel>().ReverseMap();

            CreateMap<UpdateUserDto,UserwithaddressDto>().ReverseMap();

            CreateMap<UpdateUserViewModel,UserViewModel>().ReverseMap();

            CreateMap<LoginDto, LoginViewModel>()
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.EncryptedPassword))
                .ReverseMap();
        }
    }



}

