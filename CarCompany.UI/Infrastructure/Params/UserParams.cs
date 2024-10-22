using Infrastructure.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Params
{
    public class UserParams : PaginationParams
    {
       
        //Filter By Model
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? VehicleId { get; set; }

        public string? Role { get; set; }

        public Guid? AddressId { get; set; }



    }
}
