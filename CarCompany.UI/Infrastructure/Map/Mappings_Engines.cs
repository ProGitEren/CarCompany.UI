using AutoMapper;
using Infrastructure.Models.ViewModels.Engines;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.DTO.Dto_VehicleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mappings_Engines : Profile

    {
        public Mappings_Engines()
        {
            CreateMap<EngineDto,EngineViewModel>().ReverseMap();
            CreateMap<RegisterEngineDto,RegisterEngineViewModel>().ReverseMap();

        }
    }
}
