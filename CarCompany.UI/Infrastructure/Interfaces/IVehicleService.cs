using Infrastructure.DTO.Dto_Users;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Models.ViewModels.Vehicles;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IVehicleService 
    {
        Task<IReadOnlyList<VehicleViewModel>> GetUserVehiclesAsync();
        Task<Pagination<VehicleDto>> GetVehiclesAsync(VehicleParams vehicleParams);
        Task<VehicleViewModel> CreateAsync(RegisterVehicleViewModel viewmodel);
        Task<IReadOnlyList<VehicleViewModel>> GetVehicleDetailAsync(string? email);
        Task<IReadOnlyList<VehicleViewModel>> GetVehiclesAsync();
        Task<VehicleViewModel> GetVehicleAsync(string? Id);
        Task<VehicleViewModel> UpdateVehicleAsync(VehicleViewModel model);
        Task<string> DeleteVehicleAsync(string? Id);
    }
}
