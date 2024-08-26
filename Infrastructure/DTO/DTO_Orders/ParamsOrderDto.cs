using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.DTO_Orders
{
    public class ParamsOrderDto
    {
        public int TotalItems { get; set; }

        public int PageItemCount { get; set; }

        public List<OrderDto> OrderDtos { get; set; }
    }
}
