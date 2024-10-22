using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels.OrderVehicles
{
    public class CreateOrderVehicleViewModel
    {
        public List<IFormFile> Images { get; set; }

        public string VehicleId { get; set; }

        public string ModelName { get; set; }

        public decimal Price { get; set; }
    }
}
