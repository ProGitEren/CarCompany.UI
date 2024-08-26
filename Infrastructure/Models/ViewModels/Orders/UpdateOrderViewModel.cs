using Infrastructure.DTO.DTO_Orders;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.OrderEnums;

namespace Infrastructure.Models.ViewModels.Orders
{
    public class UpdateOrderViewModel
    {
        public int Id { get; set; }
        public int OrderVehicleId { get; set; }
        public string? BuyerEmail { get; set; }
        public string SellerEmail { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Active;
        public OrderType OrderType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public UpdateOrderViewModel OrderVehicle { get; set; }
        public TransferAddress TransferAddress { get; set; }
    }
}
