using AutoMapper;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastucture.DTO.Dto_VehicleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mappings_VehicleModels : Profile
    {
        public Mappings_VehicleModels()
        {
            CreateMap<VehicleModelViewModel,VehicleModelDto>().ReverseMap();
            CreateMap<RegisterVehicleModelViewModel, VehicleModelDto>().ReverseMap();
            CreateMap<RegisterVehicleModelViewModel, RegisterVehicleModelDto>().ReverseMap();
            CreateMap<VehicleModelUserViewModel,VehicleModelDto>().ReverseMap();
        }

    }
}
