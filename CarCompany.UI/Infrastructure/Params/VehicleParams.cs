using Infrastructure.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Params
{
    public class VehicleParams : PaginationParams
    {

        //Filter By Model
        public string? VehicleId { get; set; }
        public int? ModelId { get; set; }
        public int? EngineId { get; set; }
        public string? UserId { get; set; }
        public string? Role { get; set; }

        public string? UserName { get; set; }
        

    }
}
