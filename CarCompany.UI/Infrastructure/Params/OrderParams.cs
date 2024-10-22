using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Params
{
    public class OrderParams : PaginationParams
    {

        //Filter By Model
        public string? VehicleId { get; set; }
        public string? OrderType { get; set; }
        public string? SellerEmail { get; set; }
        public string? BuyerEmail { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentMethod { get; set; }


    }
}
