using Infrastructure.DTO.DTO_Orders;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Models.ViewModels.OrderVehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.OrderEnums;

namespace Infrastructure.Models.ViewModels.Orders
{
    public class CreateOrderViewModel
    {
        public string? BuyerEmail { get; set; }
        public string SellerEmail { get; set; }
        public OrderType OrderType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public CreateOrderVehicleViewModel OrderVehicle { get; set; }
        public TransferAddress TransferAddress { get; set; }
    }
}
