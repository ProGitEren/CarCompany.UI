using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Params;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Helpers;
using Infrastucture.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrderVehicleService 
    {
       // Task<IReadOnlyList<OrderVehicleViewModel>> GetOrderVehiclesAsync();
        Task<Pagination<OrderVehicleDto>> GetOrderVehiclesAsync(OrderVehicleParams orderVehicleParams);
        Task<OrderVehicleViewModel> CreateAsync(CreateOrderVehicleViewModel viewModel);
        Task<OrderVehicleViewModel> GetOrderVehicleAsync(int? Id);
        Task<OrderVehicleViewModel> UpdateOrderVehicleAsync(UpdateOrderVehicleViewModel model);
        Task<string> DeleteOrderVehicleAsync(int? Id);
        Task<string> AddFileAsync(AddFileViewModel model);
        Task<string> DeleteFileAsync(DeleteFileViewModel model);
    }
}
