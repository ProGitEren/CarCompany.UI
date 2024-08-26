using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels.OrderVehicles
{
    public class OrderVehicleViewModel
    {
        public int Id { get; set; }
        public List<string> PicturePaths { get; set; }

        public string VehicleId { get; set; }

        public string ModelName { get; set; }

        public decimal Price { get; set; }
    }
}
