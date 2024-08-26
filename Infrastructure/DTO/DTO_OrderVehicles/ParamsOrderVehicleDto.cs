using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.DTO_OrderVehicles
{
    public class ParamsOrderVehicleDto
    {
        public int TotalItems { get; set; }

        public int PageItemCount { get; set; }

        public List<OrderVehicleDto> OrderVehicleDtos { get; set; }
    }
}
