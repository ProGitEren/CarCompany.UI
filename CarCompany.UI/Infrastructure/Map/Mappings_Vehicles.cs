using AutoMapper;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Models.ViewModels.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mappings_Vehicles : Profile
    {


        public Mappings_Vehicles()
        {
            CreateMap<VehicleDto, VehicleViewModel>().ReverseMap();
            CreateMap<RegisterVehicleDto, RegisterVehicleViewModel>().ReverseMap();
        }
    }
}
