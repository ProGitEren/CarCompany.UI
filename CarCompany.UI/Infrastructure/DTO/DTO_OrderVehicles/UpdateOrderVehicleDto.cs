using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.DTO_OrderVehicles
{
    public class UpdateOrderVehicleDto
    {
        public int Id { get; set; }
        //public List<IFormFile>? Images { get; set; }

        public string VehicleId { get; set; }

        public string ModelName { get; set; }

        public decimal Price { get; set; }
    }
}
