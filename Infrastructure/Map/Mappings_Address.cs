using AutoMapper;
using Infrastructure.DTO.Dto_Addresses;
using Infrastructure.Models.ViewModels.Addresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mappings_Address : Profile
    {

        public Mappings_Address()
        {
            CreateMap<AddressDto, AddressViewModel>().ReverseMap();


        }
    }
}
