using AutoMapper;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mappings_Pagination : Profile
    {

        public Mappings_Pagination()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
           
        }
    }
}
