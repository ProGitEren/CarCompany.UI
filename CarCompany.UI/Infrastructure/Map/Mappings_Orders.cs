using AutoMapper;
using Infrastructure.DTO.DTO_Orders;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Models.ViewModels.Orders;
using Newtonsoft.Json;
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
           
            CreateMap<OrderDto, OrderViewModel>()
                .ReverseMap();

            CreateMap<CreateOrderDto, MultiFormCreateOrderDto>()
                 .ForMember(dest => dest.OrderVehicleDtoJson, opt => opt.MapFrom(src =>
                     JsonConvert.SerializeObject(new CreateOrderVehicleDtoWithoutFiles
                     {
                         VehicleId = src.OrderVehicleDto.VehicleId,
                         ModelName = src.OrderVehicleDto.ModelName,
                         Price = src.OrderVehicleDto.Price
                     })))
                 .ForMember(dest => dest.TransferAddressJson, opt => opt.MapFrom(src =>
                     JsonConvert.SerializeObject(src.TransferAddress)))
                 .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.OrderVehicleDto.Images));


            CreateMap<UpdateOrderViewModel, OrderViewModel>()
                .ReverseMap()
                .ForMember(x => x.OrderVehicle, src => src.Ignore());

            CreateMap<UpdateOrderViewModel, UpdateOrderDto>()
                .ForMember(d => d.OrderVehicleDto, o => o.MapFrom(src => src.OrderVehicle))
                .ReverseMap()
                .ForMember(d => d.OrderVehicle, o => o.MapFrom(src => src.OrderVehicleDto));

        }
    }
}
