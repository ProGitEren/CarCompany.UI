using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Models.ViewModels.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IVehicleService
    {
        Task<IReadOnlyList<VehicleViewModel>> GetUserVehiclesAsync();

    }
}
