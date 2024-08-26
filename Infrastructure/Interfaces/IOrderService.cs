using Infrastructure.DTO.DTO_Orders;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Models.ViewModels.Orders;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Params;
using Infrastucture.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrderService 
    {
        Task<Pagination<OrderDto>> GetOrdersAsync(OrderParams orderParams);
        Task<OrderViewModel> CreateAsync(CreateOrderViewModel viewModel);
        Task<OrderViewModel> GetOrderAsync(int? Id);
        Task<OrderViewModel> UpdateOrderAsync(UpdateOrderViewModel model);
        Task<string> DeleteOrderAsync(int? Id);
    }
}
