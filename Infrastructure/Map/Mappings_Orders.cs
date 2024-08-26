using AutoMapper;
using Infrastructure.DTO.DTO_Orders;
using Infrastructure.Models.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mappings_Orders :Profile
    {

        public Mappings_Orders()
        {
            CreateMap<CreateOrderDto, CreateOrderViewModel>()
                .ForMember(d => d.OrderVehicle, src => src.MapFrom(x => x.OrderVehicleDto))
                .ReverseMap()
                .ForMember(d => d.OrderVehicleDto, src => src.MapFrom(x => x.OrderVehicle));
            CreateMap<UpdateOrderDto, UpdateOrderViewModel>()
                .ForMember(d => d.OrderVehicle, src => src.MapFrom(x => x.OrderVehicleDto))
                .ReverseMap()
                .ForMember(d => d.OrderVehicleDto, src => src.MapFrom(x => x.OrderVehicle));

            CreateMap<OrderDto, OrderViewModel>()
                .ReverseMap();




            

        }
    }
}
