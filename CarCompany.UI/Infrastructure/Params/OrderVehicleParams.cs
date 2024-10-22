using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Params
{
    public class OrderVehicleParams : PaginationParams
    {
      

        //Filter By Model
        public string? VehicleId { get; set; }

        //Sorting
        public string? Sorting { get; set; }
        //search 

      



    }
}
