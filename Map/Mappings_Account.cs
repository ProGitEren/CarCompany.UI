using AutoMapper;
using CarCompany.UI.DTO;
using CarCompany.UI.Models.ViewModels;

namespace CarCompany.UI.Map
{
    public class Mappings_Account : Profile
    {
        public Mappings_Account()
        {
            
            CreateMap<AddressDto,AddressViewModel>().ReverseMap();
            CreateMap<UserwithaddressDto, UserViewModel>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ReverseMap().ForMember(x => x.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<UsernotokenDto, UserViewModel>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ReverseMap().ForMember(x => x.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();
        }


    }
}
