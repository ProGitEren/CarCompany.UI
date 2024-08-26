using Infrastructure.DTO.DTO_Orders;
using Infrastructure.Models.Enums;
using Infrastructure.Models.ViewModels.OrderVehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.OrderEnums;

namespace Infrastructure.Models.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int OrderVehicleId { get; set; }
        public string? BuyerEmail { get; set; }
        public string SellerEmail { get; set; }
        public DateTime? OrderedDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Active;
        public OrderType OrderType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public TransferAddress TransferAddress { get; set; }
    }
}
