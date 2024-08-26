using AutoMapper;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Models.ViewModels.OrderVehicles;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Map
{
    public class Mapping_OrderVehicles : Profile
    {
        public Mapping_OrderVehicles()
        {
            CreateMap<OrderVehicleViewModel, OrderVehicleDto>()
                .ReverseMap();

            CreateMap<CreateOrderVehicleDto, CreateOrderVehicleViewModel>().ReverseMap();
            CreateMap<UpdateOrderVehicleDto, UpdateOrderVehicleViewModel>().ReverseMap();
            CreateMap<AddFileViewModel, AddFileDto>().ReverseMap();
            CreateMap<DeleteFileViewModel, DeleteFileDto>().ReverseMap();


            CreateMap<OrderVehicleViewModel, UpdateOrderVehicleViewModel>()
                .ForMember(d => d.ExistingImagePaths, src => src.MapFrom(x => x.PicturePaths))
                .ForMember(d => d.AddFile, src => src.Ignore())
                .ReverseMap();
        }
    }
}
