using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastucture.DTO.Dto_VehicleModels;
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
    public interface IVehicleModelService 
    { 
        Task<IReadOnlyList<VehicleModelUserViewModel>> GetModelsAsync();
        Task<Pagination<VehicleModelDto>> GetModelsAsync(VehiclemodelParams modelParams);
        Task<VehicleModelViewModel> CreateAsync(RegisterVehicleModelViewModel viewModel);
        Task<VehicleModelViewModel> GetModelAsync(int? Id);
        Task<VehicleModelViewModel> UpdateVehicleModelAsync(UpdateVehicleModelViewModel model);
        Task<string> DeleteVehicleModelAsync(int? Id);

    }
}
