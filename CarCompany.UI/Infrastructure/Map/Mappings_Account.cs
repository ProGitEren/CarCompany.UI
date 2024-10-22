using AutoMapper;
using Infrastructure.DTO.Dto_Addresses;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Models.ViewModels.Users;
using Infrastructure.Models.ViewModels.Vehicles;

namespace Infrastructure.Map
{
    public class Mappings_Account : Profile
    {
        public Mappings_Account()
        {
            CreateMap<RegisterDto, RegisterViewModel>().ReverseMap();



            CreateMap<UserwithdetailDto, UserViewModel>()
              .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressDto))
              .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.VehiclesDto.Select(vehicleDto => new VehicleViewModel
              {
                  Vin = vehicleDto.Vin,
                  Averagefuelin = vehicleDto.Averagefuelin,
                  Averagefuelout = vehicleDto.Averagefuelout,
                  COemmission = vehicleDto.COemmission,
                  FuelCapacity = vehicleDto.FuelCapacity,
                  MaxAllowedWeight = vehicleDto.MaxAllowedWeight,
                  MinWeight = vehicleDto.MinWeight,
                  BaggageVolume = vehicleDto.BaggageVolume,
                  DrivenKM = vehicleDto.DrivenKM,
                  ModelId = vehicleDto.ModelId,
                  EngineId = vehicleDto.EngineId
              }).ToList()))
              .ReverseMap()
              .ForMember(dest => dest.AddressDto, opt => opt.MapFrom(src => src.Address))
              .ForMember(dest => dest.VehiclesDto, opt => opt.MapFrom(src => src.Vehicles.Select(vehicle => new VehicleDto
              {
                  Vin = vehicle.Vin,
                  Averagefuelin = vehicle.Averagefuelin,
                  Averagefuelout = vehicle.Averagefuelout,
                  COemmission = vehicle.COemmission,
                  FuelCapacity = vehicle.FuelCapacity,
                  MaxAllowedWeight = vehicle.MaxAllowedWeight,
                  MinWeight = vehicle.MinWeight,
                  BaggageVolume = vehicle.BaggageVolume,
                  DrivenKM = vehicle.DrivenKM,
                  ModelId = vehicle.ModelId,
                  EngineId = vehicle.EngineId
              }).ToList()));




            CreateMap<UsernotokenDto, UserViewModel>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressDto))
                 .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.VehiclesDto.Select(vehicleDto => new VehicleViewModel
                 {
                     Vin = vehicleDto.Vin,
                     Averagefuelin = vehicleDto.Averagefuelin,
                     Averagefuelout = vehicleDto.Averagefuelout,
                     COemmission = vehicleDto.COemmission,
                     FuelCapacity = vehicleDto.FuelCapacity,
                     MaxAllowedWeight = vehicleDto.MaxAllowedWeight,
                     MinWeight = vehicleDto.MinWeight,
                     BaggageVolume = vehicleDto.BaggageVolume,
                     DrivenKM = vehicleDto.DrivenKM,
                     ModelId = vehicleDto.ModelId,
                     EngineId = vehicleDto.EngineId,
                     UserId = vehicleDto.UserId

                 }).ToList()))
                 .ReverseMap()
                 .ForMember(dest => dest.AddressDto, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.VehiclesDto, opt => opt.MapFrom(src => src.Vehicles.Select(vehicle => new VehicleDto
                 {
                     Vin = vehicle.Vin,
                     Averagefuelin = vehicle.Averagefuelin,
                     Averagefuelout = vehicle.Averagefuelout,
                     COemmission = vehicle.COemmission,
                     FuelCapacity = vehicle.FuelCapacity,
                     MaxAllowedWeight = vehicle.MaxAllowedWeight,
                     MinWeight = vehicle.MinWeight,
                     BaggageVolume = vehicle.BaggageVolume,
                     DrivenKM = vehicle.DrivenKM,
                     ModelId = vehicle.ModelId,
                     EngineId = vehicle.EngineId,
                     UserId = vehicle.UserId
                 }).ToList()));

            CreateMap<UpdateUserDto, UpdateUserViewModel>().ReverseMap();
            CreateMap<UserDto, UpdateUserViewModel>().ReverseMap();

            CreateMap<UpdateUserDto,UserwithdetailDto>().ReverseMap();

            CreateMap<UpdateUserViewModel,UserViewModel>().ReverseMap();

            CreateMap<LoginDto, LoginViewModel>()
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.EncryptedPassword))
                .ReverseMap();
        }
    }



}

