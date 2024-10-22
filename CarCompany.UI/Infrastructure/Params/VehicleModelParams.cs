using Infrastructure.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Params
{
    public class VehiclemodelParams : PaginationParams
    {
       
        //Filter By Model
        public int? ModelId { get; set; }
        public int? StartingModelYear { get; set; }
        public int? EndingModelYear { get; set; }

        public string? VehicleType { get; set; }

        //public string? ManufacturerName { get; set; }



    }
}

